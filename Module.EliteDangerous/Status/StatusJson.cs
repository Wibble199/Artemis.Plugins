namespace Module.EliteDangerous.Status {
    public sealed class StatusJson {
        public StatusFlags Flags;
        public int[] Pips = new[] { 0, 0, 0 };
        public int FireGroup;
        public GuiPanel GuiFocus;
        public Fuel Fuel = new Fuel();
        public float Cargo;
    }

    public sealed class Fuel {
        public float FuelMain;
        public float FuelReservoir;
    }
}
