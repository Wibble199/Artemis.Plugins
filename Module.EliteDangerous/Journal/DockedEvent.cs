using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal {

    internal class DockedEvent : IJournalEvent {

        public string StationName;
        public string StarSystem;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update IsDocked, this is done via Status.json.
            model.Ship.DockStatus.Docked.Trigger();
        }
    }
}
