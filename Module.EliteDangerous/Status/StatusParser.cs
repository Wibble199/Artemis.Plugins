using Module.EliteDangerous.DataModels;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Module.EliteDangerous.Status {
    public class StatusParser : IDisposable {

        private readonly object streamLock = new object();
        private bool isStreamOpen;
        private readonly FileStream fileStream;
        private readonly StreamReader streamReader;

        public StatusParser(string dataDirectory) {
            fileStream = File.Open(Path.Combine(dataDirectory, "Status.json"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            streamReader = new StreamReader(fileStream);
            isStreamOpen = true;
        }

        public void PerfomRead(EliteDangerousDataModel dataModel) {
            lock (streamLock) {
                if (!isStreamOpen) return;

                // Read the entirety of Status.json
                fileStream.Seek(0, SeekOrigin.Begin);
                var json = streamReader.ReadToEnd();
                var status = JsonConvert.DeserializeObject<StatusJson>(json);

                // Sometimes if it's read at the wrong time, it's null for a frame.
                if (status == null) return;

                // Update the datamodel based on the parsed values
                dataModel.Ship.SystemPips = status.Pips[0] / 2f;
                dataModel.Ship.EnginePips = status.Pips[1] / 2f;
                dataModel.Ship.WeaponPips = status.Pips[2] / 2f;
                dataModel.Ship.FuelMain = status.Fuel.FuelMain;
                dataModel.Ship.FuelReservoir = status.Fuel.FuelReservoir;
                dataModel.Ship.InSupercruise = status.Flags.HasFlag(StatusFlags.Supercruise);
                dataModel.Ship.ShieldsActive = status.Flags.HasFlag(StatusFlags.ShieldsUp);
            }
        }

        public void Dispose() {
            lock (streamLock) {
                streamReader.Close();
                fileStream.Close();
                isStreamOpen = false;
            }
        }
    }
}
