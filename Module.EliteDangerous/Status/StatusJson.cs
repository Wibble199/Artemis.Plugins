namespace Module.EliteDangerous.Status {
    public sealed class StatusJson {
        public StatusFlags Flags;
        public int[] Pips;
        public int FireGroup;
        public int GuiFocus;
        public Fuel Fuel;
        public float Cargo;
    }

    public sealed class Fuel {
        public float FuelMain;
        public float FuelReservoir;
    }
}
