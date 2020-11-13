namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Ship {

        public bool InSupercruise { get; internal set; }

        public bool InDanger { get; internal set; }
        public bool BeingInterdicted { get; internal set; }

        public ShipSystems Systems { get; } = new();
        public Fuel Fuel { get; } = new();
        public FSD FSD { get; } = new();
    }
}
