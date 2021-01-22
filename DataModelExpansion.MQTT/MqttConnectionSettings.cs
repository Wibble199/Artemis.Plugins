using System;

namespace DataModelExpansion.Mqtt {

    public record MqttConnectionSettings(
        Guid ServerId,
        string ServerUrl,
        int ServerPort,
        string ClientId,
        string Username,
        string Password
    );
}
