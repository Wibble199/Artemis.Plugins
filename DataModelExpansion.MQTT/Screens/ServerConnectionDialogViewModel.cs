using Artemis.UI.Shared.Services;
using FluentValidation;
using Stylet;

namespace DataModelExpansion.Mqtt.Screens {

    public class ServerConnectionDialogViewModel : DialogViewModelBase {

        public ServerConnectionDialogViewModel(IModelValidator<ServerConnectionDialogViewModel> validator) : base(validator) {
            ConnectionSettings = new MqttConnectionSettings();
        }

        public ServerConnectionDialogViewModel(IModelValidator<ServerConnectionDialogViewModel> validator, MqttConnectionSettings initialValues) : base(validator) {
            ConnectionSettings = initialValues.Clone();
        }

        public MqttConnectionSettings ConnectionSettings { get; }

        public async void Save() {
            await ValidateAsync();

            if (!HasErrors)
                Session.Close(ConnectionSettings);
        }
    }

    public class ServerConnectionDialogViewModelValidator : AbstractValidator<ServerConnectionDialogViewModel> {
        public ServerConnectionDialogViewModelValidator() {
            RuleFor(m => m.ConnectionSettings.DisplayName).NotEmpty().WithMessage("Enter a name for this server");
            RuleFor(m => m.ConnectionSettings.ServerUrl).NotEmpty().WithMessage("Server URL cannot be empty");
            RuleFor(m => m.ConnectionSettings.ServerPort).GreaterThanOrEqualTo(1).LessThanOrEqualTo(65535).WithMessage("Enter an integer between 1 and 65535");
            RuleFor(m => m.ConnectionSettings.ClientId).NotEmpty().WithMessage("Client ID cannot be blank");
        }
    }
}
