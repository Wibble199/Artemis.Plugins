using Artemis.UI.Shared.Services;
using DataModelExpansion.Mqtt.Settings;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt.ViewModels {

    /// <summary>
    /// ViewModel for a <see cref="MqttDynamicDataModelStructureNode"/>.
    /// </summary>
    public class MqttDataModelStructureConfigurationViewModel : Screen {

        private readonly IDialogService dialogService;
        private readonly MqttDataModelStructureConfigurationViewModel parent;

        private string label;
        private string topic;
        private Type type;

        /// <summary>
        /// Creates a new, blank ViewModel that represents a non-materialized <see cref="MqttDynamicDataModelStructureNode"/>.
        /// </summary>
        private MqttDataModelStructureConfigurationViewModel(IDialogService dialogService, MqttDataModelStructureConfigurationViewModel parent) {
            this.dialogService = dialogService;
            this.parent = parent;
        }

        /// <summary>
        /// Creates a new ViewModel that represents the given <see cref="MqttDynamicDataModelStructureNode"/>.
        /// </summary>
        public MqttDataModelStructureConfigurationViewModel(IDialogService dialogService, MqttDataModelStructureConfigurationViewModel parent, MqttDynamicDataModelStructureNode model) : this(dialogService, parent) {
            label = model.Label;
            topic = model.Topic;
            type = model.Type;
            if (model.Children != null)
                Children = new BindableCollection<MqttDataModelStructureConfigurationViewModel>(
                    model.Children.Select(c => new MqttDataModelStructureConfigurationViewModel(dialogService, this, c))
                );
        }

        #region Properties
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

        public BindableCollection<MqttDataModelStructureConfigurationViewModel> Children { get; init; }

        public bool IsGroup => Children != null;
        public bool IsValue => Children == null;
        #endregion

        #region Actions
        /// <summary>
        /// Triggers a dialog to edit this ViewModel, and stores changes on confirm.
        /// </summary>
        public async Task EditNode() {
            var result = await dialogService.ShowDialogAt<MqttNodeConfigurationViewModel>("MqttConfigHost", new() { ["target"] = this });
            if (result is MqttNodeConfigurationViewModel.DialogResult r) {
                Label = r.Label;
                Topic = r.Topic;
                Type = r.Type;
            }
        }

        /// <summary>
        /// Trigers a dialog that asks the user to confirm to delete this structural node.
        /// </summary>
        public async Task DeleteNode() {
            // If Children is null or does not have this child, throw an error
            if (parent.Children?.Contains(this) != true)
                throw new InvalidOperationException("This node does not support child or child does not exist in this node.");

            var result = await dialogService.ShowConfirmDialogAt(
                "MqttConfigHost",
                $"Delete {(IsGroup ? "Group" : "Value")}",
                "Are you sure you wish to delete this " + (IsGroup
                    ? "group?" + Environment.NewLine + Environment.NewLine + $"This will also delete {Children.Count} child item(s)."
                    : "value?"),
                "Delete",
                "Don't delete"
            );
            if (result)
                parent.Children.Remove(this);
        }

        /// <summary>
        /// Attempts to add a new child node ViewModel to this node's children collection.
        /// </summary>
        /// <param name="addGroup">If <c>true</c>, adds a new group node. If <c>false</c>, adds a new value node.</param>
        /// <exception cref="InvalidOperationException">If this node is a value-type node that does not support children.</exception>
        public async Task AddChildNode(bool addGroup) {
            if (IsValue)
                throw new InvalidOperationException("Cannot add a child item to an item that does not support children.");

            var result = await dialogService.ShowDialogAt<MqttNodeConfigurationViewModel>("MqttConfigHost", new() { ["isGroup"] = addGroup });
            if (result is MqttNodeConfigurationViewModel.DialogResult r)
                Children.Add(new MqttDataModelStructureConfigurationViewModel(dialogService, this) {
                    Label = r.Label,
                    Topic = addGroup ? null : r.Topic,
                    Type = addGroup ? null : r.Type,
                    Children = addGroup ? new BindableCollection<MqttDataModelStructureConfigurationViewModel>() : null
                });
        }
        #endregion

        /// <summary>
        /// Converts this ViewModel into a <see cref="MqttDynamicDataModelStructureNode"/> model that can be saved and used
        /// by the <see cref="MqttDynamicDataModelClassBuilder"/>.
        /// </summary>
        public MqttDynamicDataModelStructureNode ViewModelToModel() => new MqttDynamicDataModelStructureNode {
            Label = label,
            Topic = topic,
            Type = type,
            Children = IsGroup ? new List<MqttDynamicDataModelStructureNode>(Children.Select(c => c.ViewModelToModel())) : null
        };
    }
}
