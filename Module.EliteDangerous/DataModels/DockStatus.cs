using Artemis.Core;
using Artemis.Core.DataModelExpansions;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class DockStatus {
        [DataModelProperty(Description = "Whether the ship is currently docked (at a station).")]
        public bool IsDocked { get; internal set; }

        [DataModelProperty(Description = "Whether the ship is currently landed (on a planet).")]
        public bool IsLanded { get; internal set; }


        [DataModelProperty(Description = "Fired when the ship docks at a station.")]
        public DataModelEvent Docked { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the pilot cancels a station docking request.")]
        public DataModelEvent DockingRequestCancelled { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the station denies a pilot's docking request.")]
        public DataModelEvent DockingRequestDenied { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the station grants a pilot's docking request.")]
        public DataModelEvent DockingRequestGranted { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when a pilot requests docking access to a station.")]
        public DataModelEvent DockingRequested { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the alloted time on a docking request expires.")]
        public DataModelEvent DockingRequestTimeout { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the ship undocks from a station.")]
        public DataModelEvent Undocked { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the ship touches down on a planet surface.")]
        public DataModelEvent Touchdown { get; } = new DataModelEvent();

        [DataModelProperty(Description = "Fired when the ship takes off from a planet's surface.")]
        public DataModelEvent Liftoff { get; } = new DataModelEvent();
    }
}
