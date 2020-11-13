using Artemis.Core;
using Artemis.Core.DataModelExpansions;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class SRV {

        public bool TurretViewActive { get; internal set; }

        public bool HandbrakeActive { get; internal set; }
        public bool LightsOn { get; internal set; }
        public bool HighBeamActive { get; internal set; }
        public bool DriveAssistActive { get; internal set; }

        [DataModelProperty(Description = "Whether the SRV's turret is retracted (e.g. when close to the mothership).")]
        public bool TurretRetracted { get; internal set; }

        public DataModelEvent Launched { get; } = new DataModelEvent();
        public DataModelEvent Docked { get; } = new DataModelEvent();
    }
}
