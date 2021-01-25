using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace DataModelExpansion.Mqtt.DataModels.Dynamic {

    /// <summary>
    /// Class that creates a class derived from <see cref="DataModel"/> with the given properties.
    /// </summary>
    /// <remarks>
    /// Uses a custom class builder instead of <see cref="DataModel.AddDynamicChild{T}(T, string, string?, string?)"/> because
    /// that restricts dynamic children to be DataModels themselves, but I want to be able to dynamically add simple types.
    /// </remarks>
    public static class DynamicDataModelBuilder {

        // Builders
        private static readonly AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicDataModels"), AssemblyBuilderAccess.RunAndCollect);
        private static readonly ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        // Reflection
        private static readonly MethodInfo propogateValueAbstract = typeof(DynamicDataModelBase).GetMethod(nameof(DynamicDataModelBase.PropogateValue));
        private static readonly MethodInfo stringEquals = typeof(string).GetMethod("op_Equality");
        private static readonly MethodInfo guidEquals = typeof(Guid).GetMethod("op_Equality");
        private static readonly MethodInfo getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
        private static readonly MethodInfo convertChangeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) });
        private static readonly ConstructorInfo mqttDynamicDataModelCtor = typeof(DynamicDataModelBase).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        private static readonly MethodInfo objectEquals = typeof(object).GetMethod(nameof(object.Equals), new[] { typeof(object), typeof(object) });
        private static readonly ConstructorInfo guidCtor = typeof(Guid).GetConstructor(new[] { typeof(string) });

        /// <summary>
        /// Creates a new type with the given fields.
        /// </summary>
        public static Type Build(StructureDefinitionNode dataModelNode) {
            if (dataModelNode.Type != null)
                throw new ArgumentException("Root data model node must not be a concrete type.");

            // Create dynamic type
            var typeBuilder = moduleBuilder.DefineType("DynamicDataModel_" + Guid.NewGuid().ToString("N"), TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout);
            typeBuilder.SetParent(typeof(DynamicDataModelBase));

            // Implement instance constructor to create new instances of child data models and events
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);
            var ctorIl = ctor.GetILGenerator();

            // Implement static constructor to create new instance of server GUIDs
            // (faster to create static to compare against, rather than creating new GUID each comparison https://stackoverflow.com/a/38064035/14406924)
            var sctor = typeBuilder.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes);
            var sctorIl = sctor.GetILGenerator();

            // Implement PropogateValue to handle setting topic values
            var propogateValue = typeBuilder.DefineMethod(nameof(DynamicDataModelBase.PropogateValue), MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { typeof(Guid), typeof(string), typeof(object) });
            var pvIl = propogateValue.GetILGenerator();

            // Store reference to any static GUID fields we may need
            var serverGuidStatics = new Dictionary<Guid, FieldBuilder>();

            // Create property for each child
            foreach (var node in dataModelNode.Children) {
                // If child is a value-like (e.g. int, string, etc.)
                // Also make PropogateValue check to see if topic matches child topic, if so set the value
                if (node.Type != null) {
                    var field = CreateProperty(typeBuilder, node.Label, node.Type);
                    var convertedValue = pvIl.DeclareLocal(node.Type);
                    var endTopicIf = pvIl.DefineLabel();
                    var endEqualityIf = pvIl.DefineLabel();

                    //-- if (sourceServer == <child.Server> && topic == "<child.Topic>") {
                    if (node.Server.HasValue) {
                        pvIl.Emit(Ldarg_1);
                        pvIl.Emit(Ldsfld, serverGuidStatics.GetServerGuidField(typeBuilder, sctorIl, node.Server.Value));
                        pvIl.Emit(Call, guidEquals);
                        pvIl.Emit(Brfalse_S, endTopicIf);
                    }

                    pvIl.Emit(Ldarg_2);
                    pvIl.Emit(Ldstr, node.Topic);
                    pvIl.Emit(Call, stringEquals);
                    pvIl.Emit(Brfalse_S, endTopicIf);

                    //-- try {
                    pvIl.BeginExceptionBlock();

                    //-- tmpLocal = (<child.Type>)Convert.ChangeType(value, typeof(<child.Type>));
                    pvIl.Emit(Ldarg_3);
                    pvIl.Emit(Ldtoken, node.Type);
                    pvIl.Emit(Call, getTypeFromHandle);
                    pvIl.Emit(Call, convertChangeType);
                    pvIl.Emit(Unbox_Any, node.Type);
                    pvIl.Emit(Stloc, convertedValue);

                    //-- if (!object.Equals(tmpLocal, <field>)) {
                    pvIl.Emit(Ldarg_0);
                    pvIl.Emit(Ldfld, field);
                    if (node.Type.IsValueType)
                        pvIl.Emit(Box, node.Type);
                    pvIl.Emit(Ldloc, convertedValue);
                    if (node.Type.IsValueType)
                        pvIl.Emit(Box, node.Type);
                    pvIl.Emit(Call, objectEquals);
                    pvIl.Emit(Brtrue_S, endEqualityIf);

                    //-- <field> = tmpLocal;
                    pvIl.Emit(Ldarg_0);
                    pvIl.Emit(Ldloc, convertedValue);
                    pvIl.Emit(Stfld, field);

                    if (node.GenerateEvent) {
                        var eventArgsType = typeof(MqttPropertyChangeEventArgs<>).MakeGenericType(node.Type);
                        var eventType = typeof(DataModelEvent<>).MakeGenericType(eventArgsType);
                        var eventField = CreateProperty(typeBuilder, node.Label + "_Changed", eventType, createSetter: false);

                        // (Also make ctor initialise event field)
                        EmitNewFieldInstance(ctorIl, eventField);

                        //-- <fieldChangeEvent>.Trigger(new MqttPropertyChangeEventArgs<<child.Type>>(topic, tmpLocal));
                        pvIl.Emit(Ldarg_0);
                        pvIl.Emit(Ldfld, eventField);
                        pvIl.Emit(Ldarg_2);
                        pvIl.Emit(Ldloc, convertedValue);
                        pvIl.Emit(Newobj, eventArgsType.GetConstructor(new[] { typeof(string), node.Type }));
                        pvIl.Emit(Callvirt, eventType.GetMethod("Trigger"));
                    }
                    //-- }
                    pvIl.MarkLabel(endEqualityIf);
                    pvIl.Emit(Leave_S, endTopicIf);

                    //-- } catch {
                    pvIl.BeginCatchBlock(typeof(object));
                    pvIl.Emit(Pop);
                    pvIl.Emit(Leave_S, endTopicIf);

                    //-- }
                    pvIl.EndExceptionBlock();
                    pvIl.MarkLabel(endTopicIf);
                }
                
                // Else if child needs to be converted to a nested datamodel
                // Make ctor initialise the value to a new instance
                // Also make PropogateValue call PropogateValue on this nested MqttDynamicDataModel
                else {
                    var childType = Build(node);
                    var field = CreateProperty(typeBuilder, node.Label, childType);

                    EmitNewFieldInstance(ctorIl, field);
                    
                    pvIl.Emit(Ldarg_0);
                    pvIl.Emit(Ldfld, field);
                    pvIl.Emit(Ldarg_1);
                    pvIl.Emit(Ldarg_2);
                    pvIl.Emit(Ldarg_3);
                    pvIl.Emit(Callvirt, propogateValueAbstract);
                }
            }

            // Finish ctor (call base constructor)
            ctorIl.Emit(Ldarg_0);
            ctorIl.Emit(Call, mqttDynamicDataModelCtor);
            ctorIl.Emit(Ret);

            // Finish static ctor
            sctorIl.Emit(Ret);

            // Finish the PropogateValue function
            pvIl.Emit(Ret);

            // Implement abstract method
            typeBuilder.DefineMethodOverride(propogateValue, propogateValueAbstract);

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates a property on the given builder with the given name and type.
        /// </summary>
        /// <returns>The <see cref="FieldBuilder"/> for the property backing field.</returns>
        private static FieldBuilder CreateProperty(TypeBuilder typeBuilder, string name, Type type, bool createGetter = true, bool createSetter = true) {
            // Backing field to store value
            var backingField = typeBuilder.DefineField($"{name}__BackingField", type, FieldAttributes.Private);

            // Property
            var property = typeBuilder.DefineProperty(name, default, type, null);

            // Property getter
            if (createGetter) {
                var getter = typeBuilder.DefineMethod($"get_{name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, type, Type.EmptyTypes);
                var gIl = getter.GetILGenerator();
                gIl.Emit(Ldarg_0);
                gIl.Emit(Ldfld, backingField);
                gIl.Emit(Ret);

                property.SetGetMethod(getter);
            }

            // Property setter
            if (createSetter) {
                var setter = typeBuilder.DefineMethod($"set_{name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void), new[] { type });
                var sIl = setter.GetILGenerator();
                sIl.Emit(Ldarg_0);
                sIl.Emit(Ldarg_1);
                sIl.Emit(Stfld, backingField);
                sIl.Emit(Ret);

                property.SetSetMethod(setter);
            }

            return backingField;
        }

        /// <summary>
        /// Adds code to initialise the given <paramref name="field"/> using the field type's parameterless constructor.
        /// </summary>
        /// <param name="ilGen"><see cref="ILGenerator"/> of an instance method/constructor.</param>
        private static void EmitNewFieldInstance(ILGenerator ilGen, FieldInfo field) {
            var ctor = field.FieldType.GetConstructor(Type.EmptyTypes);
            ilGen.Emit(Ldarg_0);
            ilGen.Emit(Newobj, ctor);
            ilGen.Emit(Stfld, field);
        }

        /// <summary>
        /// Gets the <see cref="FieldBuilder"/> of the field on the given type that represents the given static guid value, creating it if it does not exist.
        /// </summary>
        /// <param name="serverGuidStatics">A collection of already-created GUID-representing static fields.</param>
        /// <param name="staticCtorIlGen">The <see cref="ILGenerator"/> of the static constructor where the code for the initialisation of the static field will be emitted.</param>
        /// <param name="guid">The GUID to be stored in the static field.</param>
        /// <returns></returns>
        private static FieldBuilder GetServerGuidField(this Dictionary<Guid, FieldBuilder> serverGuidStatics, TypeBuilder typeBuilder, ILGenerator staticCtorIlGen, Guid guid) {
            // Check if created already
            if (serverGuidStatics.TryGetValue(guid, out var fb))
                return fb;

            // Create field
            var guidStr = guid.ToString();
            var field = typeBuilder.DefineField($"staticServerGuid__{guidStr}", typeof(Guid), FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

            // Create static constructor IL to initialise field
            staticCtorIlGen.Emit(Ldstr, guidStr);
            staticCtorIlGen.Emit(Newobj, guidCtor);
            staticCtorIlGen.Emit(Stsfld, field);

            serverGuidStatics.Add(guid, field);
            return field;
        }
    }
}
