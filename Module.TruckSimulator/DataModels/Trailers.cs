using Artemis.Core.DataModelExpansions;
using Artemis.Plugins.Modules.TruckSimulator.Telemetry;
using System.Collections.Generic;

namespace Artemis.Plugins.Modules.TruckSimulator.DataModels {
    public class Trailers : ChildDataModel {

        private List<Trailer> trailerData;

        public Trailers(TruckSimulatorDataModel root) : base(root) { }

        [DataModelProperty(Description = "Total number of trailers. Note: this includes any that are spawned but not yet hooked up to the truck.")]
        public int TrailerCount {
            get {
                for (var i = 0; i < TruckSimulatorMemoryStruct.TrailerCount; i++)
                    if (string.IsNullOrWhiteSpace(Telemetry.trailers[i].id))
                        return i;
                return TruckSimulatorMemoryStruct.TrailerCount;
            }
        }

        [DataModelProperty(Description = "Whether any trailers are attached to the truck.")]
        public bool AnyAttached {
            get {
                foreach (var trailer in Telemetry.trailers)
                    if (trailer.attached != 0)
                        return true;
                return false;
            }
        }

        [DataModelProperty(Description = "List containing details about the state of each trailer.")]
        public List<Trailer> TrailerData {
            get {
                // When the list of trailers is fetched, check to see if the current trailer data list has the correct number of trailers
                // I.E. if there are 2 trailers spawned, ensure that there are 2 items in this list.
                if (trailerData == null || trailerData.Count != TrailerCount) {
                    trailerData = new List<Trailer>(TrailerCount);
                    for (var i = 0; i < TrailerCount; i++)
                        trailerData.Add(new Trailer(DataModelRoot, i));
                }
                return trailerData;
            }
        }
    }

    public class Trailer : ChildDataModel {
        private readonly int trailerIndex;

        public Trailer(TruckSimulatorDataModel root, int trailerIndex) : base(root) {
            this.trailerIndex = trailerIndex;
            Wheels = new TrailerWheels(root, trailerIndex);
        }

        public string BodyType => Telemetry.trailers[trailerIndex].bodyType;
        public string Brand => Telemetry.trailers[trailerIndex].brand;
        public string Model => Telemetry.trailers[trailerIndex].model;
        public TrailerWheels Wheels { get; }
    }

    public class TrailerWheels : ChildDataModel {
        private readonly int trailerIndex;
        private List<TrailerWheel> wheelData;

        public TrailerWheels(TruckSimulatorDataModel root, int trailerIndex) : base(root) {
            this.trailerIndex = trailerIndex;
        }

        public uint WheelCount => Telemetry.trailers[trailerIndex].wheelCount;

        public List<TrailerWheel> WheelData {
            get {
                // When the list of wheels is fetched, check to see if the wheel data list has the correct number of elements for this trailer.
                // I.E. if this trailer has 8 wheels, ensure that there are 8 items in this list.
                if (wheelData == null || wheelData.Count != WheelCount) {
                    wheelData = new List<TrailerWheel>((int)WheelCount);
                    for (var i = 0; i < WheelCount; i++)
                        wheelData.Add(new TrailerWheel(DataModelRoot, trailerIndex, i));
                }
                return wheelData;
            }
        }
    }

    public class TrailerWheel : ChildDataModel {
        private readonly int trailerIndex;
        private readonly int wheelIndex;

        public TrailerWheel(TruckSimulatorDataModel root, int trailerIndex, int wheelIndex) : base(root) {
            this.trailerIndex = trailerIndex;
            this.wheelIndex = wheelIndex;
        }

        private Telemetry.Trailer ThisTrailer => Telemetry.trailers[trailerIndex];

        public bool OnGround => ThisTrailer.wheelsOnGround[wheelIndex] != 0;
        public bool Steerable => ThisTrailer.wheelsSteerable[wheelIndex] != 0;
        public bool Liftable => ThisTrailer.wheelsLiftable[wheelIndex] != 0;
        public bool Powered => ThisTrailer.wheelsPowered[wheelIndex] != 0;
        public float Steering => ThisTrailer.wheelsSteering[wheelIndex];
        public float Velocity => ThisTrailer.wheelVelocities[wheelIndex];
        public float SuspensionDeflection => ThisTrailer.wheelSuspDeflections[wheelIndex];
        public string Surface => Telemetry.substances[ThisTrailer.wheelSubstances[wheelIndex]].name;
    }
}
