namespace Artemis.Plugins.Modules.EliteDangerous.Status {
    public sealed class StatusJson {
        public StatusFlags Flags;
        public int[] Pips = new[] { 0, 0, 0 };
        public int FireGroup;
        public GuiPanel GuiFocus;
        public Fuel Fuel = new Fuel();
        public float Cargo;
        public LegalState LegalState;
        public float Latitude;
        public float Longitude;
        public float Altitude;
        public float Heading;
    }

    public sealed class Fuel {
        public float FuelMain;
        public float FuelReservoir;
    }
}
