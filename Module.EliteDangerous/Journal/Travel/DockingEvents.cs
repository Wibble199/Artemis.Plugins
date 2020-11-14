using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    internal class DockedEvent : IJournalEvent {

        public string StationName;
        public string StarSystem;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update IsDocked, this is done via Status.json.
            model.Navigation.DockStatus.Docked.Trigger(new DockingEventArgs {
                StationName = StationName
            });
        }
    }

    internal class DockingCancelledEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestCancelled.Trigger(new DockingEventArgs {
                StationName = StationName
            });
        }
    }

    internal class DockingDeniedEvent : IJournalEvent {

        public string StationName;
        public DockingDenyReason Reason;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestDenied.Trigger(new DockingRequestDeniedEventArgs {
                StationName = StationName,
                Reason = Reason
            });
        }
    }

    internal class DockingGrantedEvent : IJournalEvent {

        public string StationName;
        public int LandingPad;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestGranted.Trigger(new DockingRequestGrantedEventArgs {
                StationName = StationName,
                LandingPad = LandingPad
            });
        }
    }

    internal class DockingRequestedEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequested.Trigger(new DockingEventArgs {
                StationName = StationName
            });
        }
    }

    internal class DockingTimeoutEvent : IJournalEvent {

        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.DockStatus.DockingRequestTimeout.Trigger(new DockingEventArgs {
                StationName = StationName
            });
        }
    }
}
