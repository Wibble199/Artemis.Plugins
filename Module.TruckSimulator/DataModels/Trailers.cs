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
                // Guard to check telemetry data is valid
                if (Telemetry.trailers == null)
                    return 0;

                // To count the trailers, return the index of the first one without a valid ID.
                // E.G. if there were 2 trailers, the trailers at indicies [0] and [1] would have ID, but trailers[2] would not.
                // If we leave the loop, there must be the maximum number of supported trailers
                for (var i = 0; i < TruckSimulatorMemoryStruct.TrailerCount; i++)
                    if (string.IsNullOrWhiteSpace(Telemetry.trailers[i].id))
                        return i;
                return TruckSimulatorMemoryStruct.TrailerCount;
            }
        }

        [DataModelProperty(Description = "Whether the first trailer is attached to the truck.")]
        public bool Attached => Telemetry.trailers != null && Telemetry.trailers[0].attached != 0;

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

        private Telemetry.Trailer ThisTrailer => Telemetry.trailers[trailerIndex];

        [DataModelProperty(Description = "Whether this trailer is attached to an object. Does NOT represent whether the trailer is attached to the player's truck - in the case of multiple trailers (e.g. B-Doubles) this will be true because the second trailer will be attached to the first regardless of whether it is connected to the truck or not.")]
        public bool Attached => ThisTrailer.attached != 0;
        public string ChainType => ThisTrailer.chainType;

        public float CargoDamage => ThisTrailer.damageCargo;
        public float ChassisDamage => ThisTrailer.damageChassis;
        public float WheelDamage => ThisTrailer.damageWheels;

        public string BodyType => ThisTrailer.bodyType;
        public string Brand => ThisTrailer.brand;
        public string Model => ThisTrailer.model;

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

        [DataModelProperty(Description = "Whether this wheel is in contact with the ground.")]
        public bool OnGround => ThisTrailer.wheelsOnGround[wheelIndex] != 0;
        public bool Powered => ThisTrailer.wheelsPowered[wheelIndex] != 0;

        [DataModelProperty(Description = "Whether this wheel can be lifted.")]
        public bool Liftable => ThisTrailer.wheelsLiftable[wheelIndex] != 0;
        [DataModelProperty(Description = "Whether this wheel has been lifted.")]
        public bool Lifted => Liftable && LiftOffset > 0;
        [DataModelProperty(Description = "The vertical displacement of the wheel axle from its normal position due to lifting the axle.", Affix = "m")]
        public float LiftOffset => ThisTrailer.wheelLiftOffsets[wheelIndex];

        [DataModelProperty(Description = "Whether this wheel may turn in the opposite direction of the truck to help the trailer steer round corners.")]
        public bool Steerable => ThisTrailer.wheelsSteerable[wheelIndex] != 0;
        [DataModelProperty(Description = "Direction the wheel is facing relative to the trailer (0° = straight, <0° = turning left, >0° = turning right)", Affix = "°")]
        public float Steering => ThisTrailer.wheelsSteering[wheelIndex] * -360f;

        [DataModelProperty(Description = "Current rotational speed of the wheel about the axle in rotations per second.", Affix = "RPS")]
        public float Velocity => ThisTrailer.wheelVelocities[wheelIndex];
        [DataModelProperty(Description = "Current rotation of the wheel about the axle in degrees.", Affix = "°")]
        public float Rotation => ThisTrailer.wheelsRotation[wheelIndex] * 360f;

        [DataModelProperty(Description = "The vertical displacement of the wheel due to the suspension.", Affix = "m")]
        public float SuspensionDeflection => ThisTrailer.wheelSuspDeflections[wheelIndex];

        [DataModelProperty(Description = "Name of the substance underneath this wheel. E.G. 'road' or 'dirt'.")]
        public string Surface => Telemetry.substances[ThisTrailer.wheelSubstances[wheelIndex]].name;
    }
}
