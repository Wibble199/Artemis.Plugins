using Module.EliteDangerous.DataModels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Module.EliteDangerous.Journal {

    internal class JournalParser : FileReaderBase {

        private readonly string dataDirectory;

        public JournalParser(string dataDirectory) : base(true) {
            this.dataDirectory = dataDirectory;
        }

        /// <summary>
        /// Returns the filename of the newest journal log in the given journal directory.
        /// </summary>
        private string LatestJournal {
            get {
                var logFiles = Directory.GetFiles(dataDirectory, "Journal.*.log");
                Array.Sort(logFiles);
                return logFiles[^1];
            }
        }

        /// <summary>
        /// When the module is activated, begin reading the latest journal file.
        /// </summary>
        public override void Activate() {
            OpenFile(LatestJournal);

            // TODO: Add support for when a new journal log file is created
        }

        /// <summary>
        /// Parses a single journal line and if it is a known event applies it to the datamodel;.
        /// </summary>
        protected override void OnContentRead(EliteDangerousDataModel dataModel, string line) {
            var @event = JsonConvert.DeserializeObject<IJournalEvent>(line, JournalEvent.JournalEventSettings);
            @event?.ApplyUpdate(dataModel);
        }
    }
}
