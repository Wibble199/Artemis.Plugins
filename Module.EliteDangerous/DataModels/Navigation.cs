using Artemis.Core;
using Artemis.Plugins.Modules.EliteDangerous.Journal;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Navigation {

        public string CurrentSystem { get; internal set; }
        public string CurrentBody { get; internal set; }
        public BodyType CurrentBodyType { get; internal set; }
        public string CurrentStation { get; internal set; }

        public double? Latitude { get; internal set; }
        public double? Longitude { get; internal set; }
        public double? Altitude { get; internal set; }
        public double? Heading { get; internal set; }

        public int RemainingJumpsInRoute { get; internal set; }

        public DataModelEvent ApproachBody { get; } = new();
        public DataModelEvent LeaveBody { get; } = new();

        public DataModelEvent EnterSupercruise { get; } = new();
        public DataModelEvent ExitSupercruise { get; } = new();

        public DataModelEvent FSDTarget { get; } = new();

        public DockStatus DockStatus { get; } = new();
    }
}
