namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {
    public class Game : ChildDataModel {

        public Game(TruckSimulatorDataModel root) : base(root) { }

        public bool Paused => Telemetry.paused != 0;
        public ulong Timestamp => Telemetry.timestamp;
        public SCSGame CurrentGame => (SCSGame)Telemetry.currentGame;
    }

    public enum SCSGame {
        Unknown,
        ETS2,
        ATS
    }
}
