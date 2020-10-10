using Artemis.Core.DataModelExpansions;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {

    public class TruckModel : ChildDataModel {
        public TruckModel(TruckSimulatorDataModel root) : base(root) {
            Damage = new TruckDamage(root);
        }

        public TruckDamage Damage { get; }

        //public bool ParkingBrake => Telemetry.parkBrake > 0;

        [DataModelProperty(Name = "Speed (Km/h)", Affix = "Km/h")]
        public float SpeedKph => Telemetry.speed * 3.6f;

        [DataModelProperty(Name = "Speed (Mph)", Affix = "Mph")]
        public float SpeedMph => Telemetry.speed * 2.237f;
    }




    /// <summary>
    /// Damage values for the truck parts.
    /// </summary>
    public class TruckDamage : ChildDataModel {
        public TruckDamage(TruckSimulatorDataModel root) : base(root) { }

        [DataModelProperty(MinValue = 0f, MaxValue = 1f)]
        public float Engine => Telemetry.wearEngine;

        [DataModelProperty(MinValue = 0f, MaxValue = 1f)]
        public float Transmission => Telemetry.wearTransmission;

        [DataModelProperty(MinValue = 0f, MaxValue = 1f)]
        public float Cabin => Telemetry.wearCabin;

        [DataModelProperty(MinValue = 0f, MaxValue = 1f)]
        public float Chassis => Telemetry.wearChassis;

        [DataModelProperty(MinValue = 0f, MaxValue = 1f)]
        public float Wheels => Telemetry.wearWheelsAvg;
    }
}
