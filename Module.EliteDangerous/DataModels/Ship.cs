using Artemis.Core.DataModelExpansions;

namespace Module.EliteDangerous.DataModels {
    public class Ship {

        [DataModelProperty(Description = "Whether the ship is currently flying in supercruise.")]
        public bool InSupercruise { get; internal set; }
        
        [DataModelProperty(Description = "Whether the shields are currently disabled (e.g. being shot in combat) or recharged.")]
        public bool ShieldsActive { get; internal set; }

        [DataModelProperty(Name = "Fuel (Main)", Description = "Amount of fuel in the main fuel tank (thick bar on the HUD).")]
        public float FuelMain { get; internal set; }

        [DataModelProperty(Name = "Fuel (Reservoir)", Description = "Amount of fuel in the reservoir fuel tank (small bar on HUD).")]
        public float FuelReservoir { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to systems. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float SystemPips { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to engines. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float EnginePips { get; internal set; }

        [DataModelProperty(Description = "The number of power pips diverted to weapons. Value between 0 and 4. May be a half value.", MinValue = 0f, MaxValue = 4f)]
        public float WeaponPips { get; internal set; }
    }
}
