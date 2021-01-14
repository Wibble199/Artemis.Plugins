using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.DataModels;
using System;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.Emit.OpCodes;

namespace DataModelExpansion.Mqtt.Settings {

    /// <summary>
    /// Class that creates a class derived from <see cref="DataModel"/> with the given properties.
    /// </summary>
    /// <remarks>
    /// Uses a custom class builder instead of <see cref="DataModel.AddDynamicChild{T}(T, string, string?, string?)"/> because
    /// that restricts dynamic children to be DataModels themselves, but I want to be able to dynamically add simple types.
    /// </remarks>
    public class MqttDynamicDataModelClassBuilder {

        // Builders
        private static readonly AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicDataModels"), AssemblyBuilderAccess.RunAndCollect);
        private static readonly ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        // Reflection
        private static readonly MethodInfo propogateValueAbstract = typeof(MqttDynamicDataModel).GetMethod(nameof(MqttDynamicDataModel.PropogateValue));
        private static readonly MethodInfo stringEquals = typeof(string).GetMethod("op_Equality");
        private static readonly MethodInfo getTypeFromHandle = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle));
        private static readonly MethodInfo convertChangeType = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) });
        private static readonly ConstructorInfo mqttDynamicDataModelCtor = typeof(MqttDynamicDataModel).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        /// <summary>
        /// Creates a new type with the given fields.
        /// </summary>
        public static Type Build(MqttDynamicDataModelStructureNode dataModelNode) {
            if (dataModelNode.Type != null)
                throw new ArgumentException("Root data model node must not be a concrete type.");
            return RecursiveBuild(dataModelNode);
        }

        private static Type RecursiveBuild(MqttDynamicDataModelStructureNode dataModelNode) {
            // Create dynamic type
            var typeBuilder = moduleBuilder.DefineType("DynamicDataModelType_" + Guid.NewGuid().ToString("N"), TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout);
            typeBuilder.SetParent(typeof(MqttDynamicDataModel));

            // Implement ctor to create new instances of child data models
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);
            var ctorIl = ctor.GetILGenerator();

            // Implement PropogateValue to handle setting topic values
            var propogateValue = typeBuilder.DefineMethod(nameof(MqttDynamicDataModel.PropogateValue), MethodAttributes.Public | MethodAttributes.Virtual, typeof(void), new[] { typeof(string), typeof(object) });
            var pvIl = propogateValue.GetILGenerator();
            var pvIfCond = pvIl.DeclareLocal(typeof(bool));

            // Create property for each child
            foreach (var child in dataModelNode.Children) {
                // If child is a value-like (e.g. int, string, etc.)
                // Also make PropogateValue check to see if topic matches child topic, if so set the value
                if (child.Type != null) {
                    var field = CreateProperty(typeBuilder, child.Label, child.Type);
                    var endIf = pvIl.DefineLabel();

                    // if (topic == "<child.Topic>") {
                    pvIl.Emit(Ldarg_1);
                    pvIl.Emit(Ldstr, child.Topic);
                    pvIl.Emit(Call, stringEquals);
                    pvIl.Emit(Stloc, pvIfCond);
                    pvIl.Emit(Ldloc, pvIfCond);
                    pvIl.Emit(Brfalse_S, endIf);

                    // try {
                    pvIl.BeginExceptionBlock();

                    // <field> = (<child.Type>)Convert.ChangeType(value, typeof(<child.Type>));
                    pvIl.Emit(Ldarg_0);
                    pvIl.Emit(Ldarg_2);
                    pvIl.Emit(Ldtoken, child.Type);
                    pvIl.Emit(Call, getTypeFromHandle);
                    pvIl.Emit(Call, convertChangeType);
                    pvIl.Emit(Unbox_Any, child.Type);
                    pvIl.Emit(Stfld, field);

                    // } catch {
                    pvIl.BeginCatchBlock(typeof(object));
                    pvIl.Emit(Pop);
                    pvIl.Emit(Leave_S, endIf);

                    // }
                    pvIl.EndExceptionBlock();
                    pvIl.MarkLabel(endIf);
                }
                
                // Else if child needs to be converted to a nested datamodel
                // Make ctor initialise the value to a new instance
                // Also make PropogateValue call PropogateValue on this nested MqttDynamicDataModel
                else {
                    var childType = RecursiveBuild(child);
                    var field = CreateProperty(typeBuilder, child.Label, childType);

                    ctorIl.Emit(Ldarg_0);
                    ctorIl.Emit(Newobj, childType.GetConstructor(Type.EmptyTypes));
                    ctorIl.Emit(Stfld, field);
                    
                    pvIl.Emit(Ldarg_0);
                    pvIl.Emit(Ldfld, field);
                    pvIl.Emit(Ldarg_1);
                    pvIl.Emit(Ldarg_2);
                    pvIl.Emit(Callvirt, propogateValueAbstract);
                }
            }

            // Finish ctor (call base constructor)
            ctorIl.Emit(Ldarg_0);
            ctorIl.Emit(Call, mqttDynamicDataModelCtor);
            ctorIl.Emit(Ret);

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
        private static FieldBuilder CreateProperty(TypeBuilder typeBuilder, string name, Type type) {
            // Backing field to store value
            var backingField = typeBuilder.DefineField($"{name}__BackingField", type, FieldAttributes.Private);

            // Property getter
            var getter = typeBuilder.DefineMethod($"get_{name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, type, Type.EmptyTypes);
            var gIl = getter.GetILGenerator();
            gIl.Emit(Ldarg_0);
            gIl.Emit(Ldfld, backingField);
            gIl.Emit(Ret);

            // Property setter
            var setter = typeBuilder.DefineMethod($"set_{name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void), new[] { type });
            var sIl = setter.GetILGenerator();
            sIl.Emit(Ldarg_0);
            sIl.Emit(Ldarg_1);
            sIl.Emit(Stfld, backingField);
            sIl.Emit(Ret);

            // Property
            var property = typeBuilder.DefineProperty(name, default, type, null);
            property.SetGetMethod(getter);
            property.SetSetMethod(setter);

            return backingField;
        }
    }
}
