using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    internal class TouchdownEvent : IJournalEvent {

        public bool PlayerControlled;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update IsLanded, this is done via Status.json.
            model.Ship.DockStatus.Touchdown.Trigger();
        }
    }
}
