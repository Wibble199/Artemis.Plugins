using Module.EliteDangerous.DataModels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Module.EliteDangerous.Journal {

    internal class JournalParser : IDisposable {

        // Location of journal log and status files.
        private static readonly string EliteDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @"Saved Games\Frontier Developments\Elite Dangerous"
        );

        // Lock for the stream to make sure that we don't end up reading from it while trying to dispose it
        private readonly object streamLock = new object();
        private bool isStreamOpen = false;

        private FileStream journalFileStream;
        private StreamReader journalStreamReader;

        public JournalParser() {
            OpenJournal(LatestJournal);

            // TODO: Add support for when a new journal log file is created
        }

        /// <summary>
        /// Should be called during update. Will read remaining unread lines in the journal.
        /// </summary>
        public void PerfomRead(EliteDangerousDataModel dataModel) {
            lock (streamLock) {
                while (isStreamOpen && !journalStreamReader.EndOfStream)
                    ParseJournalLine(journalStreamReader.ReadLine(), dataModel);
            }
        }

        /// <summary>
        /// Closes file and reader streams on an existing journal (if present) and opens new ones.
        /// </summary>
        private void OpenJournal(string journal) {
            lock (streamLock) {
                CloseStreams();
                journalFileStream = File.Open(journal, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                journalStreamReader = new StreamReader(journalFileStream);
                isStreamOpen = true;
            }
        }

        /// <summary>
        /// Returns the filename of the newest journal log in the given journal directory.
        /// </summary>
        private static string LatestJournal {
            get {
                var logFiles = Directory.GetFiles(EliteDataDirectory, "Journal.*.log");
                Array.Sort(logFiles);
                return logFiles[^1];
            }
        }

        /// <summary>
        /// Parses a single journal line and if it is a known event, adds it to the pending queue.
        /// </summary>
        private void ParseJournalLine(string line, EliteDangerousDataModel dataModel) {
            var @event = JsonConvert.DeserializeObject<IJournalEvent>(line, JournalEvent.JournalEventSettings);
            @event?.ApplyUpdate(dataModel);
        }

        /// <summary>
        /// Closes and disposes of the streams that were being used to read the journal.
        /// </summary>
        private void CloseStreams() {
            lock (streamLock) {
                isStreamOpen = false;
                journalStreamReader?.Close();
                journalFileStream?.Close();
            }
        }

        public void Dispose() => CloseStreams();
    }
}
