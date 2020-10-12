using Artemis.Core.DataModelExpansions;
using System.Collections.Generic;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {
    public class TruckWheels : ChildDataModel {

        private List<TruckWheel> wheelData;

        public TruckWheels(TruckSimulatorDataModel root) : base(root) { }

        public uint WheelCount => Telemetry.wheelCount;

        [DataModelProperty(Description = "Gets whether the wheels are currently in a lifted state. For trucks without liftable wheels, this is always false.")]
        public bool Lifted {
            get {
                // Could be done using a List condition, but it is easier for the user to just have a boolean property.
                for (var i = 0; i < WheelCount; i++)
                    if (Telemetry.wheelsLiftable[i] != 0 && Telemetry.wheelLiftOffsets[i] > 0)
                        return true;
                return false;
            }
        }

        public List<TruckWheel> WheelData {
            get {
                // When the list of wheels is fetched, check to see if the current wheel list has the correct number of elements
                // I.E. if the truck has 6 wheels, ensure that there are 6 items in this list.
                if (wheelData == null || wheelData.Count != WheelCount) {
                    wheelData = new List<TruckWheel>((int)WheelCount);
                    for (var i = 0; i < WheelCount; i++)
                        wheelData.Add(new TruckWheel(DataModelRoot, i));
                }
                return wheelData;
            }
        }
    }

    public class TruckWheel : ChildDataModel {
        private readonly int wheelIndex;

        public TruckWheel(TruckSimulatorDataModel root, int wheelIndex) : base(root) {
            this.wheelIndex = wheelIndex;
        }

        [DataModelProperty(Description = "Whether this wheel is in contact with the ground.")]
        public bool OnGround => Telemetry.wheelsOnGround[wheelIndex] != 0;
        public bool Powered => Telemetry.wheelsPowered[wheelIndex] != 0;

        [DataModelProperty(Description = "Whether this wheel can be lifted.")]
        public bool Liftable => Telemetry.wheelsLiftable[wheelIndex] != 0;
        [DataModelProperty(Description = "Whether this wheel has been lifted.")]
        public bool Lifted => Liftable && LiftOffset > 0;
        [DataModelProperty(Description = "The vertical displacement of the wheel axle from its normal position due to lifting the axle.", Affix = "m")]
        public float LiftOffset => Telemetry.wheelLiftOffsets[wheelIndex];

        public bool Steerable => Telemetry.wheelsSteerable[wheelIndex] != 0;
        [DataModelProperty(Description = "Direction the wheel is facing relative to the trailer (0° = straight)", Affix = "°")]
        public float Steering => Telemetry.wheelSteerings[wheelIndex] * -360f; // Multiply by negative because negative turning is to the right which is counter-intuitive IMO

        [DataModelProperty(Description = "Current rotational speed of the wheel about the axle in rotations per second.", Affix = "RPS")]
        public float Velocity => Telemetry.wheelVelocities[wheelIndex];
        [DataModelProperty(Description = "Current rotation of the wheel about the axle in degrees.", Affix = "°")]
        public float Rotation => Telemetry.wheelRotations[wheelIndex] * 360f;

        [DataModelProperty(Description = "The vertical displacement of the wheel due to the suspension.", Affix = "m")]
        public float SuspensionDeflection => Telemetry.wheelSuspDeflections[wheelIndex];

        [DataModelProperty(Description = "Name of the substance underneath this wheel. E.G. 'road' or 'dirt'.")]
        public string Surface => Telemetry.substances[Telemetry.wheelSubstances[wheelIndex]].name;
    }
}
