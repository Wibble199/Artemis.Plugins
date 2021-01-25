using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt {

    /// <summary>
    /// MQTT client that has a connection to a single server.
    /// </summary>
    public sealed class MqttConnector : IDisposable {

        private static readonly MqttFactory clientFactory = new();
        private readonly IManagedMqttClient client;

        public MqttConnector() {
            client = clientFactory.CreateManagedMqttClient();
            client.UseApplicationMessageReceivedHandler(OnClientMessageReceived);
            client.UseConnectedHandler(OnClientConnected);
            client.UseDisconnectedHandler(OnClientDisconnected);
        }

        /// <summary>
        /// Event that fires when this connector receives a message from the server it is connected to.
        /// </summary>
        public event EventHandler<MqttApplicationMessageReceivedEventArgs> MessageReceived;

        public event EventHandler<MqttClientConnectedEventArgs> Connected;
        public event EventHandler<MqttClientDisconnectedEventArgs> Disconnected;

        /// <summary>
        /// The ID of the server this connector is connected to. <para/>
        /// Based on the 'ServerId' property from the settings object passed to <see cref="Start(MqttConnectionSettings)"/>.
        /// </summary>
        public Guid ServerId { get; private set; }

        /// <summary>
        /// Whether or not this connector is currently connected to a server.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Sets up and starts listening with the MQTT client behind this connector. <para/>
        /// If the client is already connected, it will first be stopped then restarted with the new settings.
        /// </summary>
        /// <param name="settings">Settings to use to initialise the client.</param>
        public async Task Start(MqttConnectionSettings settings) {
            // Setup client options
            ServerId = settings.ServerId;

            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(settings.ServerUrl, settings.ServerPort)
                .WithClientId(settings.ClientId);

            if (!string.IsNullOrEmpty(settings.Username))
                clientOptions.WithCredentials(settings.Username, settings.Password);


            // Build options and start client
            var managedClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(clientOptions.Build())
                .Build();

            await client.StopAsync();
            await client.StartAsync(managedClientOptions);
            await client.SubscribeAsync("#");
        }

        /// <summary>
        /// Disconnects the connector from the server it is connected to.
        /// </summary>
        public Task Stop() {
            return client.StopAsync();
        }

        private void OnClientMessageReceived(MqttApplicationMessageReceivedEventArgs e) {
            MessageReceived?.Invoke(this, e);
        }

        private void OnClientConnected(MqttClientConnectedEventArgs e) {
            IsConnected = true;
            Connected?.Invoke(this, e);
        }

        private void OnClientDisconnected(MqttClientDisconnectedEventArgs e) {
            IsConnected = false;
            Disconnected?.Invoke(this, e);
        }

        public void Dispose() {
            client.Dispose();
        }
    }
}
