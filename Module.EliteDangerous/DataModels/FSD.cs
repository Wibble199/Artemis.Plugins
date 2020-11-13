using Artemis.Core;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class FSD {
        public bool IsCharging { get; internal set; }
        public bool IsJumping { get; internal set; }
        public bool IsCoolingDown { get; internal set; }
        public bool IsMassLocked { get; internal set; }

        public DataModelEvent StartJump { get; } = new DataModelEvent();
        public DataModelEvent Jump { get; } = new DataModelEvent();
        public DataModelEvent Target { get; } = new DataModelEvent();
    }
}
