using Artemis.Core.DataModelExpansions;

namespace Module.EliteDangerous.DataModels {
    public class Ship {

        [DataModelProperty(Description = "Whether the ship is currently flying in supercruise.")]
        public bool InSupercruise { get; internal set; }
        
        [DataModelProperty(Description = "Whether the shields are currently disabled (e.g. being shot in combat) or recharged.")]
        public bool ShieldsActive { get; internal set; }

        [DataModelProperty(Name = "Fuel", Description = "Amount of fuel in the main fuel tank.")]
        public float FuelMain { get; internal set; }
    }
}
