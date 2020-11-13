using Artemis.Core;
using Artemis.Plugins.Modules.TruckSimulator.ViewModels;
using Artemis.UI.Shared;

namespace Artemis.Plugins.Modules.TruckSimulator {
    public class PluginBootstrapper : IPluginBootstrapper {
        public void Enable(Plugin plugin) {
            plugin.ConfigurationDialog = new PluginConfigurationDialog<TruckSimulatorConfigurationViewModel>(); ;
        }

        public void Disable(Plugin plugin) { }
    }
}
