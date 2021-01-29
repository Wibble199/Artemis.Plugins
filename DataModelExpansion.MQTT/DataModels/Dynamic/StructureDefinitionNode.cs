using System;
using System.Collections.Generic;

namespace DataModelExpansion.Mqtt.DataModels.Dynamic {

    /// <summary>
    /// Class that defines a single node of the DataModel structure.
    /// </summary>
    public class StructureDefinitionNode {

        /// <summary>
        /// The display name this node will appear as in the Artemis Data Model.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The server that will be used to subscribe to <see cref="Topic"/>.
        /// </summary>
        public Guid? Server { get; set; }

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
        public List<StructureDefinitionNode> Children { get; set; }

        /// <summary>
        /// Returns a default root <see cref="StructureDefinitionNode"/>.
        /// </summary>
        public static StructureDefinitionNode RootDefault => new StructureDefinitionNode {
            Label = "Root",
            Children = new List<StructureDefinitionNode>()
        };
    }
}
