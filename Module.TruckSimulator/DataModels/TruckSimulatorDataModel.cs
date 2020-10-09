using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.TruckSimulator.Telemetry;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {

    public partial class TruckSimulatorDataModel : DataModel {

        public TruckSimulatorDataModel() {
            Game = new Game(this);
            Truck = new TruckModel(this);
            Trailer = new Trailer(this);
        }

        internal TruckSimulatorMemoryStruct Telemetry { get; set; }

        public Game Game { get; }
        public TruckModel Truck { get; }
        public Trailer Trailer { get; }
    }

    public abstract class ChildDataModel {
        protected TruckSimulatorDataModel DataModelRoot { get; }
        private protected TruckSimulatorMemoryStruct Telemetry => DataModelRoot.Telemetry;
        protected ChildDataModel(TruckSimulatorDataModel root) => DataModelRoot = root;
    }
}