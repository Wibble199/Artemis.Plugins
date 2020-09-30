using Module.EliteDangerous.Status;

namespace Module.EliteDangerous.DataModels {
    public class HUD {
        public GuiPanel ActivePanel { get; internal set; }
        public bool AnalysisMode { get; internal set; }
        public bool NightVision { get; internal set; }
    }
}
