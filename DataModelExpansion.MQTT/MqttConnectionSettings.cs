using Artemis.Core;
using System;

namespace DataModelExpansion.Mqtt {

    public sealed class MqttConnectionSettings : CorePropertyChanged, ICloneable {

        private string displayName = "";
        private string serverUrl = "";
        private int serverPort = 1883;
        private string clientId = "Artemis";
        private string username = "";
        private string password = "";

        /// <summary>
        /// Creates a new MQTT server connection entry with the default values.
        /// </summary>
        public MqttConnectionSettings() {
            ServerId = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new MQTT server connection entry from the given values.
        /// </summary>
        public MqttConnectionSettings(Guid serverId, string displayName, string serverUrl, int serverPort, string clientId, string username, string password) {
            ServerId = serverId;
            this.displayName = displayName;
            this.serverUrl = serverUrl;
            this.serverPort = serverPort;
            this.clientId = clientId;
            this.username = username;
            this.password = password;
        }

        // Note that private setter allows value to be set by JSON serializer.
        public Guid ServerId { get; set; }

        public string DisplayName {
            get => displayName;
            set => SetAndNotify(ref displayName, value);
        }

        public string ServerUrl {
            get => serverUrl;
            set => SetAndNotify(ref serverUrl, value);
        }

        public int ServerPort {
            get => serverPort;
            set => SetAndNotify(ref serverPort, value);
        }

        public string ClientId {
            get => clientId;
            set => SetAndNotify(ref clientId, value);
        }

        public string Username {
            get => username;
            set => SetAndNotify(ref username, value);
        }

        public string Password {
            get => password;
            set => SetAndNotify(ref password, value);
        }

        public MqttConnectionSettings Clone() => new MqttConnectionSettings(
            ServerId,
            DisplayName,
            ServerUrl,
            ServerPort,
            ClientId,
            Username,
            Password
        );
        object ICloneable.Clone() => Clone();
    }
}
