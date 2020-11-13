using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    // Occurs when the user _initiates_ the jump, I.E. when the countdown starts ("frameshift drive charging")
    internal sealed class StartJumpEvent : IJournalEvent {

        public JumpType JumpType;
        public string StarSystem;
        public StarClass? StarClass; // Only when JumpType = Hyperspace

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update FSD statuses (charging, cooldown, etc), this is done via Status.json.
            model.Ship.FSD.StartJump.Trigger();
        }
    }
}
