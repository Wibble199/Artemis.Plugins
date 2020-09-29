using Module.EliteDangerous.DataModels;

namespace Module.EliteDangerous.Journal {

    [JournalEventType("StartJump")]
    // Occurs when the user _initiates_ the jump, I.E. when the countdown starts ("frameshift drive charging")
    internal sealed class StartJumpEvent : IJournalEvent {
        public JumpType JumpType;
        public StarClass StarClass; // Only when JumpType = Hyperspace

        public void ApplyUpdate(EliteDangerousDataModel model) {
        }
    }
}
