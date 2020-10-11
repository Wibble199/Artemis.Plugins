using System.Collections.Generic;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {
    public class TruckWheels : ChildDataModel {

        private List<TruckWheel> wheelData;

        public TruckWheels(TruckSimulatorDataModel root) : base(root) { }

        public uint WheelCount => Telemetry.wheelCount;

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

        public bool OnGround => Telemetry.wheelsOnGround[wheelIndex] != 0;
        public bool Steerable => Telemetry.wheelsSteerable[wheelIndex] != 0;
        public bool Liftable => Telemetry.wheelsLiftable[wheelIndex] != 0;
        public bool Powered => Telemetry.wheelsPowered[wheelIndex] != 0;
        public float Steering => Telemetry.wheelSteerings[wheelIndex];
        public float Velocity => Telemetry.wheelVelocities[wheelIndex];
        public float SuspensionDeflection => Telemetry.wheelSuspDeflections[wheelIndex];        
        public string Surface => Telemetry.substances[Telemetry.wheelSubstances[wheelIndex]].name;
    }
}
