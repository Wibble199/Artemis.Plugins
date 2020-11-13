using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.EliteDangerous.Journal;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class EliteDangerousDataModel : DataModel {
        public Player Player { get; } = new Player();
        public Ship Ship { get; } = new Ship();
        public SRV SRV { get; } = new SRV();
        public Navigation Navigation { get; } = new Navigation();
        public HUD HUD { get; } = new HUD();
    }
}