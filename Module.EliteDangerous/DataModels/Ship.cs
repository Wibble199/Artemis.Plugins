using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.EliteDangerous.Journal;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Ship {

        public string Name { get; internal set; }
        public string Ident { get; internal set; }
        public ShipType Type { get; internal set; }
        public ShipSize Size { get; internal set; }

        public bool IsInSupercruise { get; internal set; }

        public bool IsInDanger { get; internal set; }
        public bool IsBeingInterdicted { get; internal set; }

        [DataModelProperty(Description = "Event that occurs when the player successfully evades an interdiction or is interdicted by another ship. ")]
        public DataModelEvent<InterdictionEventArgs> Interdiction { get; } = new();

        public ShipSystems Systems { get; } = new();
        public Fuel Fuel { get; } = new();
        public FSD FSD { get; } = new();
    }


    public class InterdictionEventArgs : DataModelEventArgs {
        public string Interdictor { get; init; }
        public bool InterdictorIsPlayer { get; init; }
        public bool Escaped { get; init; }
        public bool Submitted { get; init; }
    }
}
