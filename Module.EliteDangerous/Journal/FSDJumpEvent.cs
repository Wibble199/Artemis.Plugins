using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal {

    [JournalEventType("FSDJump")]
    internal sealed class FSDJumpEvent : IJournalEvent {
        public void ApplyUpdate(EliteDangerousDataModel model) { }
    }
}
