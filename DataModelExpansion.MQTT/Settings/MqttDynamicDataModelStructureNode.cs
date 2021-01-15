using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModelExpansion.Mqtt.Settings {

    /// <summary>
    /// Class that defines a single node of the DataModel structure.
    /// </summary>
    public class MqttDynamicDataModelStructureNode {

        /// <summary>
        /// The display name this node will appear as in the Artemis Data Model.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The topic that can be used to set the value of this node.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The type of value stored in this node.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Whether or not a change event will be generated for the property represented by this node.
        /// </summary>
        public bool GenerateEvent { get; set; }

        /// <summary>
        /// Any children this node has.
        /// </summary>
        public List<MqttDynamicDataModelStructureNode> Children { get; set; }

        /// <summary>
        /// Returns a list of all topics that are being listened to by this node and its child nodes.
        /// </summary>
        public IEnumerable<string> GetTopics() {
            return Children == null
                ? string.IsNullOrWhiteSpace(Topic) ? Array.Empty<string>() : new[] { Topic }
                : Children.SelectMany(c => c.GetTopics()).Distinct();
        }
    }
}
