using Module.EliteDangerous.DataModels;

namespace Module.EliteDangerous.Journal {

    [JournalEventType("SupercruiseEntry")]
    internal sealed class SupercruiseEntryEvent : IJournalEvent {
        public void ApplyUpdate(EliteDangerousDataModel model) => model.Ship.InSupercruise = true;
    }

    [JournalEventType("SupercruiseExit")]
    internal sealed class SupercruiseExitEvent : IJournalEvent {
        public void ApplyUpdate(EliteDangerousDataModel model) => model.Ship.InSupercruise = false;
    }
}
