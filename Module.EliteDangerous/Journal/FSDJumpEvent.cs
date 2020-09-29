using Module.EliteDangerous.DataModels;

namespace Module.EliteDangerous.Journal {

    [JournalEventType("FSDJump")]
    internal sealed class FSDJumpEvent : IJournalEvent {
        public void ApplyUpdate(EliteDangerousDataModel model) { }
    }
}
