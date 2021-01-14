using DataModelExpansion.Mqtt.Settings;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModelExpansion.Mqtt.ViewModels {

    /// <summary>
    /// ViewModel for a <see cref="MqttDynamicDataModelStructureNode"/>.
    /// </summary>
    public class MqttDataModelStructureConfigurationViewModel : Screen {

        private string label;
        private string topic;
        private Type type;

        private MqttDataModelStructureConfigurationViewModel() { }
        public MqttDataModelStructureConfigurationViewModel(MqttDynamicDataModelStructureNode model) {
            label = model.Label;
            topic = model.Topic;
            type = model.Type;
            if (model.Children != null)
                Children = new BindableCollection<MqttDataModelStructureConfigurationViewModel>(model.Children.Select(c => new MqttDataModelStructureConfigurationViewModel(c)));
        }

        /// <summary>
        /// Gets or sets the label used to identify this node.
        /// </summary>
        public string Label {
            get => label;
            set => SetAndNotify(ref label, value);
        }

        /// <summary>
        /// For leaf/value nodes, the topic that the node is bound to.
        /// </summary>
        public string Topic {
            get => topic;
            set => SetAndNotify(ref topic, value);
        }

        /// <summary>
        /// For leaf/value nodes, the Type that this property will materialise to.
        /// </summary>
        public Type Type {
            get => type;
            set => SetAndNotify(ref type, value);
        }

        /// <summary>
        /// For non-leaf/group nodes, a list of child ViewModels that are in this group.
        /// </summary>
        public BindableCollection<MqttDataModelStructureConfigurationViewModel> Children { get; init; }

        /// <summary>
        /// Gets whether this ViewModel represents a group node.
        /// </summary>
        public bool IsGroup => Children != null;

        /// <summary>
        /// Gets whether this ViewModel represents a value node.
        /// </summary>
        public bool IsValue => Children == null;


        /// <summary>
        /// Attempts to create a new node group ViewModel in this node's children collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this node is a value node.</exception>
        public void AddGroup() {
            if (IsValue)
                throw new InvalidOperationException("Cannot add a child item to an item that does not support children.");
            Children.Add(new MqttDataModelStructureConfigurationViewModel {
                Label = "New Group",
                Children = new BindableCollection<MqttDataModelStructureConfigurationViewModel>()
            });
        }

        /// <summary>
        /// Attempts to create a new node value ViewModel in this node's children collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">If this node is a value node.</exception>
        public void AddValue() {
            if (IsValue)
                throw new InvalidOperationException("Cannot add a child item to an item that does not support children.");
            Children.Add(new MqttDataModelStructureConfigurationViewModel {
                Label = "New Value",
                Topic = "",
                Type = typeof(string)
            });
        }

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
