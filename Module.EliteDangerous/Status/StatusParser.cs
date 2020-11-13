using Artemis.Plugins.Modules.EliteDangerous.DataModels;
using Newtonsoft.Json;
using System.IO;

namespace Artemis.Plugins.Modules.EliteDangerous.Status {

    internal class StatusParser : FileReaderBase {

        private readonly string dataDirectory;

        public StatusParser(string dataDirectory) : base(false) {
            this.dataDirectory = dataDirectory;
        }

        public override void Activate() => OpenFile(Path.Combine(dataDirectory, "Status.json"));

        protected override void OnContentRead(EliteDangerousDataModel dataModel, string content) {
            var status = JsonConvert.DeserializeObject<StatusJson>(content);
            if (status == null) return;

            bool Has(StatusFlags flag) => (status.Flags & flag) != 0;

            //
            // Update the datamodel based on the parsed values
            //

            // Player details
            dataModel.Player.CurrentlyPiloting =
                  Has(StatusFlags.PilotingMainShip) ? Vehicle.Ship
                : Has(StatusFlags.PilotingFighter) ? Vehicle.Fighter
                : Has(StatusFlags.PilotingSRV) ? Vehicle.SRV
                : Vehicle.Unknown;
            dataModel.Player.LegalState = status.LegalState;
            dataModel.Player.InWing = Has(StatusFlags.InWing);

            // HUD
            dataModel.HUD.ActivePanel = status.GuiFocus;
            dataModel.HUD.AnalysisMode = Has(StatusFlags.AnalysisMode);
            dataModel.HUD.NightVision = Has(StatusFlags.NightVision);

            // Ship
            dataModel.Ship.Docked = Has(StatusFlags.Docked);
            dataModel.Ship.Landed = Has(StatusFlags.Landed);
            dataModel.Ship.InSupercruise = Has(StatusFlags.Supercruise);
            dataModel.Ship.LandingGearDeployed = Has(StatusFlags.LandingGearDeployed);
            dataModel.Ship.CargoScoopDeployed = Has(StatusFlags.CargoScoopDeployed);
            dataModel.Ship.HardpointsDeployed = Has(StatusFlags.HardpointsDeployed);
            dataModel.Ship.ShieldsActive = Has(StatusFlags.ShieldsUp);
            dataModel.Ship.FlightAssistActive = !Has(StatusFlags.FlightAssistOff);
            dataModel.Ship.LightsOn = Has(StatusFlags.PilotingMainShip) && Has(StatusFlags.LightsOn);
            dataModel.Ship.SilentRunning = Has(StatusFlags.SilentRunning);
            dataModel.Ship.Overheating = Has(StatusFlags.Overheating);
            dataModel.Ship.InDanger = Has(StatusFlags.InDanger);
            dataModel.Ship.BeingInterdicted = Has(StatusFlags.BeingInterdicted);

            // Ship power
            dataModel.Ship.SystemPips = status.Pips[0] / 2f;
            dataModel.Ship.EnginePips = status.Pips[1] / 2f;
            dataModel.Ship.WeaponPips = status.Pips[2] / 2f;

            // Ship FSD
            dataModel.Ship.FSD.IsCharging = Has(StatusFlags.FSDCharging);
            dataModel.Ship.FSD.IsJumping = Has(StatusFlags.FSDJump);
            dataModel.Ship.FSD.IsCoolingDown = Has(StatusFlags.FSDCooldown);
            dataModel.Ship.FSD.IsMassLocked = Has(StatusFlags.FSDMassLocked);

            // Ship fuel
            dataModel.Ship.Fuel.FuelMain = status.Fuel.FuelMain;
            dataModel.Ship.Fuel.FuelReservoir = status.Fuel.FuelReservoir;
            dataModel.Ship.Fuel.FuelLow = Has(StatusFlags.LowFuel);
            dataModel.Ship.Fuel.FuelScoopActive = Has(StatusFlags.FuelScooping);

            // SRV
            dataModel.SRV.HandbrakeActive = Has(StatusFlags.SRVHandbrake);
            dataModel.SRV.TurretViewActive = Has(StatusFlags.SRVTurretView);
            dataModel.SRV.TurretRetracted = Has(StatusFlags.SRVTurretRetracted);
            dataModel.SRV.DriveAssistActive = Has(StatusFlags.SRVDriveAssist);
            dataModel.SRV.LightsOn = Has(StatusFlags.PilotingSRV) && Has(StatusFlags.LightsOn);
            dataModel.SRV.HighBeamActive = Has(StatusFlags.SRVHighBeam);
        }
    }
}
