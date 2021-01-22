using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using DataModelExpansion.Mqtt.DataModels;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using MQTTnet;
using System;
using System.ComponentModel;

namespace DataModelExpansion.Mqtt {

    public class MqttDataModelExpansion : DataModelExpansion<RootDataModel> {

        private readonly MqttConnector client;

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

            client = new();
            client.OnMessageReceived += OnMqttClientMessageReceived;
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
            await client.Start(new MqttConnectionSettings(
                Guid.NewGuid(),
                serverUrlSetting.Value,
                serverPortSetting.Value,
                clientIdSetting.Value,
                usernameSetting.Value,
                passwordSetting.Value
            ));
        }

        private async void StopMqttClient() {
            await client?.Stop();
        }

        private void OnMqttClientMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e) {
            // Pass incoming messages to the root DataModel's HandleMessage method.
            DataModel.HandleMessage(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());
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