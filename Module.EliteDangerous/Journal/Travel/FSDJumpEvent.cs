using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    internal sealed class FSDJumpEvent : IJournalEvent {

        public string StarSystem;
        public float JumpDist;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update FSD statuses (charging, etc.), this is done via Status.json.
            model.Ship.FSD.Jump.Trigger();
        }
    }
}
