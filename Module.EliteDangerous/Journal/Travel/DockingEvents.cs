using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    internal class DockedEvent : IJournalEvent {

        public string StationName;
        public string StarSystem;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update IsDocked, this is done via Status.json.
            model.Navigation.DockStatus.Docked.Trigger();
        }
    }

    internal class DockingCancelledEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestCancelled.Trigger();
        }
    }

    internal class DockingDeniedEvent : IJournalEvent {

        public string StationName;
        public DockingDenyReason Reason;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestDenied.Trigger();
        }
    }

    internal class DockingGrantedEvent : IJournalEvent {

        public string StationName;
        public int LandingPad;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestGranted.Trigger();
        }
    }

    internal class DockingRequestedEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequested.Trigger();
        }
    }

    internal class DockingTimeoutEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestTimeout.Trigger();
        }
    }
}
