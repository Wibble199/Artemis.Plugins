using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.DataModels;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.ComponentModel;

namespace DataModelExpansion.Mqtt {

    public class MqttDataModelExpansion : DataModelExpansion<RootDataModel> {

        private IManagedMqttClient client;
        private readonly PluginSetting<string> serverUrlSetting;
        private readonly PluginSetting<int> serverPortSetting;
        private readonly PluginSetting<string> clientIdSetting;
        private readonly PluginSetting<string> usernameSetting;
        private readonly PluginSetting<string> passwordSetting;
        private readonly PluginSetting<StructureDefinitionNode> dynamicDataModelStructure;

        public MqttDataModelExpansion(PluginSettings settings) {
            (serverUrlSetting, serverPortSetting, clientIdSetting, usernameSetting, passwordSetting, dynamicDataModelStructure) = settings.GetMqqtSettings();
            serverUrlSetting.PropertyChanged += OnServerSettingChanged;
            serverPortSetting.PropertyChanged += OnServerSettingChanged;
            clientIdSetting.PropertyChanged += OnServerSettingChanged;
            usernameSetting.PropertyChanged += OnServerSettingChanged;
            passwordSetting.PropertyChanged += OnServerSettingChanged;
            dynamicDataModelStructure.PropertyChanged += OnDataModelStructureChanged;
        }

        public override async void Enable() {
            DataModel.UpdateDataModel(dynamicDataModelStructure.Value);
            RestartMqttClient();
        }

        public override void Disable() {
            StopMqttClient();
        }

        public override void Update(double deltaTime) { }


        private async void RestartMqttClient() {
            // Create Mqtt client if it is not already created
            if (client == null) {
                client = new MqttFactory().CreateManagedMqttClient();
                client.UseApplicationMessageReceivedHandler(OnMqttClientMessageReceived);
                client.UseConnectedHandler(OnMqttClientConnected);
                client.UseDisconnectedHandler(OnMqttClientDisconnected);
            }

            // Create options for the client
            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(serverUrlSetting.Value, serverPortSetting.Value)
                .WithClientId(clientIdSetting.Value);

            if (!string.IsNullOrWhiteSpace(usernameSetting.Value))
                clientOptions = clientOptions.WithCredentials(usernameSetting.Value, passwordSetting.Value);

            var managedClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(clientOptions.Build())
                .Build();

            // Restart the client with the new options
            await client.StopAsync();
            await client.StartAsync(managedClientOptions);

            foreach (var topic in dynamicDataModelStructure.Value.GetTopics())
                await client.SubscribeAsync(topic);
        }

        private async void StopMqttClient() {
            await client?.StopAsync();
        }

        private void OnMqttClientMessageReceived(MqttApplicationMessageReceivedEventArgs e) {
            // Pass incoming messages to the root DataModel's HandleMessage method.
            DataModel.HandleMessage(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());
        }

        private void OnMqttClientConnected(MqttClientConnectedEventArgs e) {
            DataModel.IsConnected = true;
        }

        private void OnMqttClientDisconnected(MqttClientDisconnectedEventArgs e) {
            DataModel.IsConnected = false;
        }

        private void OnServerSettingChanged(object sender, PropertyChangedEventArgs e) {
            RestartMqttClient();
        }

        private async void OnDataModelStructureChanged(object sender, PropertyChangedEventArgs e) {
            // Rebuild the Artemis Data Model with the new structure
            DataModel.UpdateDataModel(dynamicDataModelStructure.Value);

            // Restart the Mqtt client incase it needs to change which topics it's subscribed to
            RestartMqttClient();
        }
    }
}