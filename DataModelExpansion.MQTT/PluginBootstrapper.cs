using Artemis.Core;
using Artemis.UI.Shared;
using DataModelExpansion.Mqtt.ViewModels;

namespace DataModelExpansion.Mqtt {

    public class PluginBootstrapper : IPluginBootstrapper {

        public void Enable(Plugin plugin) {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<MqttConfigurationViewModel>();
        }

        public void Disable(Plugin plugin) { }
    }
}
