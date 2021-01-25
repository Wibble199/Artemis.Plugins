using System;
using System.Collections;
using System.Collections.Generic;

namespace DataModelExpansion.Mqtt.DataModels {

    /// <summary>
    /// A specialized collection that holds the status of each active connector and allows them
    /// to be presented to the data model.
    /// </summary>
    public class MqttConnectorStatusCollection : IEnumerable<MqttConnectorStatus> {

        private readonly Dictionary<Guid, MqttConnectorStatus> statuses = new();

        /// <summary>
        /// Clears and re-creates the internal list of server connector statuses.
        /// </summary>
        /// <param name="serverList"></param>
        internal void UpdateConnectorList(List<MqttConnectionSettings> serverList) {
            statuses.Clear();
            foreach (var server in serverList)
                statuses.Add(server.ServerId, new MqttConnectorStatus(server.DisplayName));
        }

        /// <summary>
        /// Fetches the status object for a single MQTT server connector.
        /// </summary>
        internal MqttConnectorStatus this[Guid serverId] => statuses[serverId];

        // IEnumerable methods for data model
        public IEnumerator<MqttConnectorStatus> GetEnumerator() => statuses.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// Holds data about a single MQTT connector.
    /// </summary>
    public class MqttConnectorStatus {
        public MqttConnectorStatus(string name) {
            Name = name;
        }

        public string Name { get; }
        public bool IsConnected { get; internal set; }
    }
}
