using Artemis.Core;
using Artemis.UI.Shared.Services;
using FluentValidation;
using Stylet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt.Screens {

    /// <summary>
    /// ViewModel for single node edit dialog.
    /// </summary>
    public class StructureNodeConfigurationDialogViewModel : DialogViewModelBase {

        private static readonly Type[] supportedTypes = new[] { typeof(string), typeof(bool), typeof(int), typeof(double) };

        private string label;
        private Guid? server;
        private string topic;
        private Type type;
        private bool generateEvent;

        public StructureNodeConfigurationDialogViewModel(IModelValidator<StructureNodeConfigurationDialogViewModel> validator, PluginSettings settingsService, bool isGroup) : base(validator) {
            label = "";
            server = Guid.Empty;
            topic = isGroup ? null : "";
            type = isGroup ? null : supportedTypes[0];
            IsGroup = isGroup;
            ServerConnectionsSetting = settingsService.GetSetting<List<MqttConnectionSettings>>("ServerConnections");
        }

        public StructureNodeConfigurationDialogViewModel(IModelValidator<StructureNodeConfigurationDialogViewModel> validator, PluginSettings settingsService, StructureNodeViewModel target) : base(validator) {
            label = target.Label;
            server = target.Server;
            topic = target.Topic;
            type = target.Type;
            generateEvent = target.GenerateEvent;
            IsGroup = target.IsGroup;
            ServerConnectionsSetting = settingsService.GetSetting<List<MqttConnectionSettings>>("ServerConnections");
        }

        public string Label {
            get => label;
            set => SetAndNotify(ref label, value);
        }

        public Guid? Server {
            get => server;
            set => SetAndNotify(ref server, value);
        }

        public string Topic {
            get => topic;
            set => SetAndNotify(ref topic, value);
        }

        public Type Type {
            get => type;
            set => SetAndNotify(ref type, value);
        }

        public bool GenerateEvent {
            get => generateEvent;
            set => SetAndNotify(ref generateEvent, value);
        }

        public bool IsGroup { get; }
        public bool IsValue => !IsGroup;

        public IEnumerable<Type> SupportedValueTypes => supportedTypes;
        public PluginSetting<List<MqttConnectionSettings>> ServerConnectionsSetting { get; }

        public async Task Save() {
            await ValidateAsync();

            if (!HasErrors)
                Session.Close(new DialogResult(Label, Server, Topic, Type, GenerateEvent));
        }

        /// <summary>
        /// POCO that contains the result of a successful MqttNodeConfiguration dialog.
        /// </summary>
        public record DialogResult(string Label, Guid? Server, string Topic, Type Type, bool GenerateEvent);
    }


    public class MqttNodeConfigurationViewModelValidator : AbstractValidator<StructureNodeConfigurationDialogViewModel> {
        public MqttNodeConfigurationViewModelValidator() {
            RuleFor(m => m.Label).NotEmpty().WithMessage("Label cannot be blank");

            When(m => !m.IsGroup, () => {
                RuleFor(m => m.Type).NotNull().WithMessage("Choose a type for this data model property");
            });
        }
    }
}
