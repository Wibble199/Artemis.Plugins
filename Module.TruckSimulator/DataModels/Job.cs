using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.TruckSimulator.Conversions;
using System;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {
    public class Job : ChildDataModel {
        public Job(TruckSimulatorDataModel root) : base(root) { }

        public DateTime DeliveryTime => Telemetry.deliveryTime.ToGameDateTime();

        [DataModelProperty(Description = "Time until the current delivery is due at the destination in minutes.", Affix = "min")]
        public int RemainingDeliveryTime => Telemetry.gameTime < 4_000_000_000 && Telemetry.deliveryTime > 0
            ? (int)(Telemetry.deliveryTime - Telemetry.gameTime)
            : 0;

        public bool CargoLoaded => Telemetry.cargoLoaded != 0;
        public bool SpecialCargo => Telemetry.specialJob != 0;

        [DataModelProperty(Description = "Name of the cargo type being transported.")]
        public string CargoType => Telemetry.cargo;
        [DataModelProperty(Description = "Total number of units of cargo being transported.")]
        public uint CargoUnitCount => Telemetry.jobCargoUnitCount;
        [DataModelProperty(Description = "Mass of a single unit of cargo in kilograms.", Affix = "kg")]
        public float CargoUnitMass => Telemetry.jobCargoUnitMass;
        [DataModelProperty(Description = "Total mass of all cargo in kilograms.", Affix = "kg")]
        public float CargoTotalMass => Telemetry.jobCargoMass;

        public string DestinationCity => Telemetry.destinationCity;
        public string DestinationCompany => Telemetry.destinationCompany;

        public string SourceCity => Telemetry.sourceCity;
        public string SourceCompany => Telemetry.sourceCompany;
    }
}
