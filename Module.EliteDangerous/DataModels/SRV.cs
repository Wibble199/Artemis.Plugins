using Artemis.Core.DataModelExpansions;

namespace Module.EliteDangerous.DataModels {
    public class SRV {

        public bool TurretViewActive { get; internal set; }

        public bool HandbrakeActive { get; internal set; }
        public bool LightsOn { get; internal set; }
        public bool HighBeamActive { get; internal set; }
        public bool DriveAssistActive { get; internal set; }

        [DataModelProperty(Description = "The SRV's turret is retracted when close to the mothership.")]
        public bool TurretRetracted { get; internal set; }
    }
}
