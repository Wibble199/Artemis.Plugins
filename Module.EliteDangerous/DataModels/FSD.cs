using Artemis.Core;
using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.EliteDangerous.Journal;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class FSD {
        public bool IsCharging { get; internal set; }
        public bool IsJumping { get; internal set; }
        public bool IsCoolingDown { get; internal set; }
        public bool IsMassLocked { get; internal set; }

        [DataModelProperty(Description = "Occurs when the user begins charging the FSD for a jump (i.e. when the countdown starts).")]
        public DataModelEvent<StartJumpEventArgs> StartJump { get; } = new();

        [DataModelProperty(Description = "Occurs when a hyperspace jump completes (i.e. as you enter a new system).")]
        public DataModelEvent<JumpEventArgs> Jump { get; } = new();

        [DataModelProperty(Description = "Occurs when selecting a new star in the galaxy map. Also occurs mid-jump to update the route.")]
        public DataModelEvent Target { get; } = new();
    }


    public class StartJumpEventArgs : DataModelEventArgs {
        public JumpType JumpType { get; init; }
        public string StarSystem { get; init; }
        public StarClass? StarClass { get; init; }
    }

    public class JumpEventArgs : DataModelEventArgs {
        public StarClass StarClass { get; init; }
        public float JumpDistance { get; init; }
        public float FuelUsed { get; init; }
    }
}
