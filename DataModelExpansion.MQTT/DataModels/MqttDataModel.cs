using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.Settings;
using System;

namespace DataModelExpansion.Mqtt.DataModels {

    public class MqttDataModel : DataModel {

        public bool IsConnected { get; set; }

        /// <summary>
        /// Handles an incoming message for a particular topic.
        /// </summary>
        internal void HandleMessage(string topic, object data) {
            DynamicChild<MqttDynamicDataModel>("DynamicData").PropogateValue(topic, data);
        }

        /// <summary>
        /// Removes and rebuilds the dynamically created DataModel.
        /// </summary>
        internal void UpdateDataModel(MqttDynamicDataModelStructureNode dataModelStructure) {
            // Remove existing
            RemoveDynamicChildByKey("DynamicData");

            // Build and add new structure
            var type = MqttDynamicDataModelClassBuilder.Build(dataModelStructure);
            var inst = (MqttDynamicDataModel)Activator.CreateInstance(type);
            AddDynamicChild(inst, "DynamicData", "Data");
        }
    }
}