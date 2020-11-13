using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Other {

    // Special event that occurs when the journal file grows to 500k lines.
    internal class ContinuedEvent : IJournalEvent {

        public int Part;

        public void ApplyUpdate(EliteDangerousDataModel model) { }
    }
}
