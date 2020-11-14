using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.EliteDangerous.Journal;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Navigation {

        public string CurrentSystem { get; private set; }
        public string CurrentBody { get; private set; }
        public BodyType? CurrentBodyType { get; private set; }
        public string CurrentStation { get; private set; }

        public double? Latitude { get; internal set; }
        public double? Longitude { get; internal set; }
        public double? Altitude { get; internal set; }
        public double? Heading { get; internal set; }

        public int RemainingJumpsInRoute { get; internal set; }

        public DataModelEvent ApproachBody { get; } = new();
        public DataModelEvent LeaveBody { get; } = new();

        public DataModelEvent EnterSupercruise { get; } = new();
        public DataModelEvent ExitSupercruise { get; } = new();

        [DataModelProperty(Description = "Occurs when a location is selected in the galaxy map. Also occurs just after entering a hyperspace jump when on a multi-stop route.")]
        public DataModelEvent FSDTarget { get; } = new();

        public DockStatus DockStatus { get; } = new();

        internal void UpdateLocation(string system, string body = null, BodyType? bodyType = null, string station = null) {
            CurrentSystem = system;
            CurrentBody = body;
            CurrentBodyType = bodyType;
            CurrentStation = station;
        }
    }
}
