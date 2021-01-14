using Artemis.Core;
using DataModelExpansion.Mqtt.Settings;
using System.Collections.Generic;

namespace DataModelExpansion.Mqtt {
    internal static class Utils {
        public static (
            PluginSetting<string> serverUrlSetting,
            PluginSetting<int> serverPortSetting,
            PluginSetting<string> clientIdSetting,
            PluginSetting<string> usernameSetting,
            PluginSetting<string> passwordSetting,
            PluginSetting<MqttDynamicDataModelStructureNode> dynamicDataModelStructure
        ) GetMqqtSettings(this PluginSettings settings) => (
            settings.GetSetting("ServerUrl", "localhost"),
            settings.GetSetting("ServerPort", 1883),
            settings.GetSetting("ClientId", "Artemis"),
            settings.GetSetting("Username", ""),
            settings.GetSetting("Password", ""),
            settings.GetSetting("DynamicDataModelStructure", new MqttDynamicDataModelStructureNode {
                Label = "Root",
                Children = new List<MqttDynamicDataModelStructureNode>()
            })
        );
    }
}
