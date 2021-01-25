using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.DataModels;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt {

    public class MqttDataModelExpansion : DataModelExpansion<RootDataModel> {

        private readonly List<MqttConnector> connectors = new();

        private readonly PluginSetting<List<MqttConnectionSettings>> serverConnectionsSetting;
        private readonly PluginSetting<StructureDefinitionNode> dynamicDataModelStructureSetting;

        public MqttDataModelExpansion(PluginSettings settings) {
            serverConnectionsSetting = settings.GetSetting("ServerConnections", new List<MqttConnectionSettings>());
            serverConnectionsSetting.PropertyChanged += OnSeverConnectionListChanged;

            dynamicDataModelStructureSetting = settings.GetSetting("DynamicDataModelStructure", StructureDefinitionNode.RootDefault);
            dynamicDataModelStructureSetting.PropertyChanged += OnDataModelStructureChanged;
        }

        public override async void Enable() {
            DataModel.UpdateDataModel(dynamicDataModelStructureSetting.Value);
            DataModel.Statuses.UpdateConnectorList(serverConnectionsSetting.Value);
            await RestartConnectors();
        }

        public override async void Disable() {
            await StopConnectors();
        }

        public override void Update(double deltaTime) { }


        private Task RestartConnectors() {
            // Resize connectors to match number of setup servers
            // - Remove extraneous connectors if there are more connectors than there are server connections
            if (connectors.Count > serverConnectionsSetting.Value.Count) {
                var amountToRemove = connectors.Count - serverConnectionsSetting.Value.Count;
                foreach (var connector in connectors.Take(amountToRemove)) {
                    connector.MessageReceived -= OnMqttClientMessageReceived;
                    connector.Connected -= OnMqttClientConnected;
                    connector.Disconnected -= OnMqttClientDisconnected;
                    connector.Dispose();
                }
                connectors.RemoveRange(0, amountToRemove);
            }

            // - Add new connectors if there are less connectors than there are server connections
            else if (connectors.Count < serverConnectionsSetting.Value.Count) {
                for (var i = connectors.Count; i < serverConnectionsSetting.Value.Count; i++) {
                    var connector = new MqttConnector();
                    connector.MessageReceived += OnMqttClientMessageReceived;
                    connector.Connected += OnMqttClientConnected;
                    connector.Disconnected += OnMqttClientDisconnected;
                    connectors.Add(connector);
                }
            }


            // Start each connector with relevant settings
            return Task.WhenAll(
                connectors.Select((connector, i) => connector.Start(serverConnectionsSetting.Value[i]))
            );
        }

        private Task StopConnectors() {
            return Task.WhenAll(
                connectors.Select(connector => connector.Stop())
            );
        }

        private void OnMqttClientMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e) {
            // Pass incoming messages to the root DataModel's HandleMessage method.
            DataModel.HandleMessage((sender as MqttConnector).ServerId, e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());
        }

        private void OnMqttClientConnected(object sender, MqttClientConnectedEventArgs e) {
            DataModel.Statuses[(sender as MqttConnector).ServerId].IsConnected = true;
        }

        private void OnMqttClientDisconnected(object sender, MqttClientDisconnectedEventArgs e) {
            DataModel.Statuses[(sender as MqttConnector).ServerId].IsConnected = false;
        }

        private void OnSeverConnectionListChanged(object sender, PropertyChangedEventArgs e) {
            RestartConnectors();
            DataModel.Statuses.UpdateConnectorList(serverConnectionsSetting.Value);
        }

        private async void OnDataModelStructureChanged(object sender, PropertyChangedEventArgs e) {
            // Rebuild the Artemis Data Model with the new structure
            DataModel.UpdateDataModel(dynamicDataModelStructureSetting.Value);

            // Restart the Mqtt client incase it needs to change which topics it's subscribed to
            await RestartConnectors();
        }

        protected override void Dispose(bool disposing) {
            connectors.ForEach(connector => {
                connector.MessageReceived -= OnMqttClientMessageReceived;
                connector.Dispose();
            });
            connectors.Clear();
        }
    }
}