using Module.EliteDangerous.DataModels;

namespace Module.EliteDangerous.Journal {

    [JournalEventType("ShieldState")]
    internal sealed class ShieldStateEvent : IJournalEvent {
        public bool ShieldsUp;
        public void ApplyUpdate(EliteDangerousDataModel model) => model.Ship.ShieldsActive = ShieldsUp;
    }
}
