using Artemis.Core.DataModelExpansions;

namespace Module.EliteDangerous.DataModels {
    public class Ship {

        public bool ShieldsActive { get; internal set; }

        public bool InSupercruise { get; internal set; }

        [DataModelProperty(Description = "Whether the ship is currently docked (at a station).")]
        public bool Docked { get; internal set; }

        [DataModelProperty(Description = "Whether the ship is currently landed (on a planet).")]
        public bool Landed { get; internal set; }

        public bool HardpointsDeployed { get; internal set; }
        public bool LandingGearDeployed { get; internal set; }
        public bool CargoScoopDeployed { get; internal set; }

        public bool InDanger { get; internal set; }
        public bool BeingInterdicted { get; internal set; }

        [DataModelProperty(Description = "When the ship's heat level is above 100%.")]
        public bool Overheating { get; internal set; }

        public bool LightsOn { get; internal set; }
        public bool SilentRunning { get; internal set; }
        public bool FlightAssistActive { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to systems. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float SystemPips { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to engines. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float EnginePips { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to weapons. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float WeaponPips { get; internal set; }

        public Fuel Fuel { get; } = new Fuel();
        public FSD FSD { get; } = new FSD();
    }
}
