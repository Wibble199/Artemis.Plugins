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

        [DataModelProperty(Description = "Fired when the ship touches down on a planet surface.")]
        public DataModelEvent Touchdown { get; } = new DataModelEvent();
    }
}
