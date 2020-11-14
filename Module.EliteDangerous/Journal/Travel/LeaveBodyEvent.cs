using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {
    internal class LeaveBodyEvent : IJournalEvent {

        public string StarSystem;
        public string Body;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Not sure if it is possible for the current body to be changed between an ApproachBody and a LeaveBody
            // event firing, but just to be sure, don't clear the current body if it has been set to something else.
            if (model.Navigation.CurrentBody == Body)
                model.Navigation.UpdateLocation(StarSystem, null);
            model.Navigation.LeaveBody.Trigger();
        }
    }
}
