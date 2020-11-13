using Artemis.Core.DataModelExpansions;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Fuel {
        [DataModelProperty(Name = "Fuel (Main)", Description = "Amount of fuel in the main fuel tank (thick bar on the HUD).")]
        public float FuelMain { get; internal set; }

        [DataModelProperty(Name = "Fuel (Reservoir)", Description = "Amount of fuel in the reservoir fuel tank (small bar on HUD).")]
        public float FuelReservoir { get; internal set; }

        [DataModelProperty(Description = "If the ship currently has less than 25% fuel.")]
        public bool FuelLow { get; internal set; }

        public bool FuelScoopActive { get; internal set; }
    }
}
