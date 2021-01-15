using Artemis.UI.Shared.Services;
using FluentValidation;
using Stylet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt.ViewModels {

    public class MqttNodeConfigurationViewModel : DialogViewModelBase {

        private static readonly Type[] supportedTypes = new[] { typeof(string), typeof(bool), typeof(int), typeof(double) };

        private string label;
        private string topic;
        private Type type;
        private bool generateEvent;

        public MqttNodeConfigurationViewModel(IModelValidator<MqttNodeConfigurationViewModel> validator, bool isGroup) : base(validator) {
            label = "";
            topic = isGroup ? null : "";
            type = isGroup ? null : supportedTypes[0];
            IsGroup = isGroup;
        }

        public MqttNodeConfigurationViewModel(IModelValidator<MqttNodeConfigurationViewModel> validator, MqttDataModelStructureConfigurationViewModel target) : base(validator) {
            label = target.Label;
            topic = target.Topic;
            type = target.Type;
            generateEvent = target.GenerateEvent;
            IsGroup = target.IsGroup;
        }

        public string Label {
            get => label;
            set => SetAndNotify(ref label, value);
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

        public async Task Save() {
            await ValidateAsync();

            if (!HasErrors)
                Session.Close(new DialogResult(Label, Topic, Type, GenerateEvent));
        }

        /// <summary>
        /// POCO that contains the result of a successful MqttNodeConfiguration dialog.
        /// </summary>
        public record DialogResult(string Label, string Topic, Type Type, bool GenerateEvent);
    }


    public class MqttNodeConfigurationViewModelValidator : AbstractValidator<MqttNodeConfigurationViewModel> {
        public MqttNodeConfigurationViewModelValidator() {
            RuleFor(m => m.Label).NotEmpty().WithMessage("Label cannot be blank.");

            When(m => !m.IsGroup, () => {
                RuleFor(m => m.Type).NotNull();
            });
        }
    }
}
