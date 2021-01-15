using Artemis.Core.DataModelExpansions;

namespace DataModelExpansion.Mqtt.DataModels.Dynamic {

    /// <summary>
    /// Base MqttDataModel of which concrete classes will be dynamically created from at runtime.
    /// </summary>
    public abstract class DynamicDataModelBase : DataModel {

        /// <summary>
        /// Takes an incoming topic and value and populates any values that use that topic.
        /// Calls <see cref="PropogateValue(string, object)"/> on any child MqttDynamicDataModels.
        /// </summary>
        public abstract void PropogateValue(string topic, object value);
    }
}
