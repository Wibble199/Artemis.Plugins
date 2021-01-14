using Artemis.Core;
using Artemis.UI.Shared;
using Artemis.UI.Shared.Services;
using DataModelExpansion.Mqtt.Settings;

namespace DataModelExpansion.Mqtt.ViewModels {

    public class MqttConfigurationViewModel : PluginConfigurationViewModel {

        private readonly PluginSetting<MqttDynamicDataModelStructureNode> dynamicDataModelStructureSetting;

        public MqttConfigurationViewModel(Plugin plugin, PluginSettings settings, IDialogService dialogService) : base(plugin) {
            (ServerUrlSetting, ServerPortSetting, ClientIdSetting, UsernameSetting, PasswordSetting, dynamicDataModelStructureSetting) = settings.GetMqqtSettings();
            DynamicDataModelStructureRoot = new MqttDataModelStructureConfigurationViewModel(dialogService, null, dynamicDataModelStructureSetting.Value);
        }

        public PluginSetting<string> ServerUrlSetting { get; }
        public PluginSetting<int> ServerPortSetting { get; }
        public PluginSetting<string> ClientIdSetting { get; }
        public PluginSetting<string> UsernameSetting { get; }
        public PluginSetting<string> PasswordSetting { get; }

        public MqttDataModelStructureConfigurationViewModel DynamicDataModelStructureRoot { get; }

        protected override void OnInitialActivate() {
            base.OnInitialActivate();
            ServerUrlSetting.AutoSave = true;
            ServerPortSetting.AutoSave = true;
            ClientIdSetting.AutoSave = true;
            UsernameSetting.AutoSave = true;
            PasswordSetting.AutoSave = true;
            dynamicDataModelStructureSetting.AutoSave = true;
        }

        public void Save() {
            dynamicDataModelStructureSetting.Value = DynamicDataModelStructureRoot.ViewModelToModel();
        }
    }
}
