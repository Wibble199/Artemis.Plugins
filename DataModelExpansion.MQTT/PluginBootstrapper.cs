using Artemis.Core;
using Artemis.UI.Shared;
using DataModelExpansion.Mqtt.Screens;

namespace DataModelExpansion.Mqtt {

    public class PluginBootstrapper : IPluginBootstrapper {

        public void Enable(Plugin plugin) {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<MqttPluginConfigurationViewModel>();
        }

        public void Disable(Plugin plugin) { }
    }
}
