using Artemis.Core;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using Stylet;
using System.Collections.Generic;
using System.Linq;

namespace DataModelExpansion.Mqtt.Screens {

    /// <summary>
    /// ViewModel for the main MQTT plugin configuration view.
    /// </summary>
    public class MqttPluginConfigurationViewModel : PluginConfigurationViewModel {

        private readonly IDialogService dialogService;

        private readonly PluginSetting<List<MqttConnectionSettings>> serverConnectionsSetting;
        private readonly PluginSetting<StructureDefinitionNode> dynamicDataModelStructureSetting;

        public MqttPluginConfigurationViewModel(Plugin plugin, PluginSettings settings, IDialogService dialogService) : base(plugin) {
            this.dialogService = dialogService;

            serverConnectionsSetting = settings.GetSetting("ServerConnections", new List<MqttConnectionSettings>());
            ServerConnections = new BindableCollection<MqttConnectionSettings>(serverConnectionsSetting.Value);

            dynamicDataModelStructureSetting = settings.GetSetting("DynamicDataModelStructure", StructureDefinitionNode.RootDefault);
            DynamicDataModelStructureRoot = new StructureNodeViewModel(dialogService, null, dynamicDataModelStructureSetting.Value);
        }

        public BindableCollection<MqttConnectionSettings> ServerConnections { get; }
        public StructureNodeViewModel DynamicDataModelStructureRoot { get; }

        public async void AddServerConnection() {
            var result = await dialogService.ShowDialogAt<ServerConnectionDialogViewModel>("MqttConfigHost");
            if (result is MqttConnectionSettings resultSettings)
                ServerConnections.Add(resultSettings);
        }

        public async void EditServerConnection(MqttConnectionSettings connection) {
            var result = await dialogService.ShowDialogAt<ServerConnectionDialogViewModel>("MqttConfigHost", new() { ["initialValues"] = connection });
            if (result is MqttConnectionSettings resultSettings) {
                var idx = ServerConnections.IndexOf(connection);
                ServerConnections.Remove(connection);
                ServerConnections.Insert(idx, resultSettings);
            }
        }

        public async void DeleteServerConnection(MqttConnectionSettings connection) {
            var result = await dialogService.ShowConfirmDialogAt("MqttConfigHost", "Delete Connection", "Are you sure you wish to delete this connection?", "Delete", "Don't delete");
            if (result)
                ServerConnections.Remove(connection);
        }

        public void Save() {
            serverConnectionsSetting.Value = ServerConnections.ToList();
            serverConnectionsSetting.Save();

            dynamicDataModelStructureSetting.Value = DynamicDataModelStructureRoot.ViewModelToModel();
            dynamicDataModelStructureSetting.Save();
        }
    }
}
