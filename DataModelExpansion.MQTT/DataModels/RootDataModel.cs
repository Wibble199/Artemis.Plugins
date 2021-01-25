using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using System;

namespace DataModelExpansion.Mqtt.DataModels {

    public class RootDataModel : DataModel {

        /// <summary>
        /// Handles an incoming message for a particular server and topic.
        /// </summary>
        internal void HandleMessage(Guid sourceServer, string topic, object data) {
            DynamicChild<DynamicDataModelBase>("DynamicData").PropogateValue(sourceServer, topic, data);
        }

        /// <summary>
        /// Removes and rebuilds the dynamically created DataModel.
        /// </summary>
        internal void UpdateDataModel(StructureDefinitionNode dataModelStructure) {
            // Remove existing
            RemoveDynamicChildByKey("DynamicData");

            // Build and add new structure
            var type = DynamicDataModelBuilder.Build(dataModelStructure);
            var inst = (DynamicDataModelBase)Activator.CreateInstance(type);
            AddDynamicChild(inst, "DynamicData", "Data");
        }
    }
}