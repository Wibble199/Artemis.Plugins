using Artemis.UI.Shared.Services;
using DataModelExpansion.Mqtt.DataModels.Dynamic;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModelExpansion.Mqtt.Screens {

    /// <summary>
    /// ViewModel representing a <see cref="DynamicStructureNode"/> model.
    /// </summary>
    public class StructureNodeViewModel : PropertyChangedBase {

        private readonly IDialogService dialogService;
        private readonly StructureNodeViewModel parent;

        private string label;
        private Guid? server;
        private string topic;
        private Type type;
        private bool generateEvent;

        /// <summary>
        /// Creates a new, blank ViewModel that represents a non-materialized <see cref="DynamicStructureNode"/>.
        /// </summary>
        private StructureNodeViewModel(IDialogService dialogService, StructureNodeViewModel parent) {
            this.dialogService = dialogService;
            this.parent = parent;
        }

        /// <summary>
        /// Creates a new ViewModel that represents the given <see cref="DynamicStructureNode"/>.
        /// </summary>
        public StructureNodeViewModel(IDialogService dialogService, StructureNodeViewModel parent, StructureDefinitionNode model) : this(dialogService, parent) {
            label = model.Label;
            server = model.Server;
            topic = model.Topic;
            type = model.Type;
            generateEvent = model.GenerateEvent;
            if (model.Children != null)
                Children = new BindableCollection<StructureNodeViewModel>(
                    model.Children.Select(c => new StructureNodeViewModel(dialogService, this, c))
                );
        }

        #region Properties
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

        public BindableCollection<StructureNodeViewModel> Children { get; init; }

        public bool IsGroup => Children != null;
        public bool IsValue => Children == null;
        #endregion

        #region Actions
        /// <summary>
        /// Triggers a dialog to edit this ViewModel, and stores changes on confirm.
        /// </summary>
        public async Task EditNode() {
            var result = await dialogService.ShowDialogAt<StructureNodeConfigurationDialogViewModel>("MqttConfigHost", new() { ["target"] = this });
            if (result is StructureNodeConfigurationDialogViewModel.DialogResult r) {
                Label = r.Label;
                Server = r.Server;
                Topic = r.Topic;
                Type = r.Type;
                generateEvent = r.GenerateEvent;
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

            var result = await dialogService.ShowDialogAt<StructureNodeConfigurationDialogViewModel>("MqttConfigHost", new() { ["isGroup"] = addGroup });
            if (result is StructureNodeConfigurationDialogViewModel.DialogResult r)
                Children.Add(new StructureNodeViewModel(dialogService, this) {
                    Label = r.Label,
                    Server = addGroup ? null : r.Server,
                    Topic = addGroup ? null : r.Topic,
                    Type = addGroup ? null : r.Type,
                    GenerateEvent = !addGroup && generateEvent,
                    Children = addGroup ? new BindableCollection<StructureNodeViewModel>() : null
                });
        }
        #endregion

        /// <summary>
        /// Converts this ViewModel into a <see cref="DynamicStructureNode"/> model that can be saved, and used
        /// by the <see cref="DynamicClassBuilder"/>.
        /// </summary>
        public StructureDefinitionNode ViewModelToModel() => new StructureDefinitionNode {
            Label = label,
            Server = server,
            Topic = topic,
            Type = type,
            GenerateEvent = generateEvent,
            Children = IsGroup ? new List<StructureDefinitionNode>(Children.Select(c => c.ViewModelToModel())) : null
        };
    }
}
