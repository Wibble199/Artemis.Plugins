using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.TruckSimulator.Conversions;
using System;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {

    public class Game : ChildDataModel {

        public Game(TruckSimulatorDataModel root) : base(root) { }

        public SCSGame CurrentGame => (SCSGame)Telemetry.currentGame;
        public bool Paused => Telemetry.paused != 0;
        public DateTime GameTime => Telemetry.gameTime.ToGameDateTime();

        [DataModelProperty(Description = "Determines the scale between real time and in-game time. Typical values are 3 (in cities), 15 (in UK) and 19 (everywhere else). For example, at a scale of 19, one real-life minute would be 19 minutes in game.")]
        public float Scale => Telemetry.scale;
    }

    public enum SCSGame {
        Unknown,
        ETS2,
        ATS
    }
}
