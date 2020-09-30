using Artemis.Core.DataModelExpansions;

namespace Module.EliteDangerous.DataModels {
    public class EliteDangerousDataModel : DataModel {
        public Player Player { get; } = new Player();
        public Ship Ship { get; } = new Ship();
        public SRV SRV { get; } = new SRV();
        public HUD HUD { get; } = new HUD();
    }
}