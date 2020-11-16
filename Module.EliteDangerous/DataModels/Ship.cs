namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Ship {

        public bool IsInSupercruise { get; internal set; }

        public bool IsInDanger { get; internal set; }
        public bool IsBeingInterdicted { get; internal set; }

        public ShipSystems Systems { get; } = new();
        public Fuel Fuel { get; } = new();
        public FSD FSD { get; } = new();
    }
}
