namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Ship {

        public bool InSupercruise { get; internal set; }

        public bool InDanger { get; internal set; }
        public bool BeingInterdicted { get; internal set; }

        public ShipSystems Systems { get; } = new ShipSystems();
        public DockStatus DockStatus { get; } = new DockStatus();
        public Fuel Fuel { get; } = new Fuel();
        public FSD FSD { get; } = new FSD();
    }
}
