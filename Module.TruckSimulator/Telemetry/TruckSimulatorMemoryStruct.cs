using System;
using System.Runtime.InteropServices;

namespace Artemis.Plugins.Modules.TruckSimulator.Telemetry {

    // Adapted from the code at https://github.com/RenCloud/scs-sdk-plugin/blob/master/scs-client/C%23/SCSSdkClient/SCSSdkConvert.cs

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal readonly struct TruckSimulatorMemoryStruct {

        private const int StringSize = 64;
        private const int WheelSize = 16;
        private const int SlotSize = 32;
        private const int Substances = 25;


        // ----------------------------------------------
        // First zone (offset 0)
        // ----------------------------------------------
        public readonly bool sdkActive;
        public readonly bool paused;
        public readonly ulong timestamp;
        public readonly ulong simulationTimestamp;
        public readonly ulong renderTimestamp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] readonly int[] _g1; // Gap of 8 bytes


        // ----------------------------------------------
        // Second zone (offset 40)
        // ----------------------------------------------
        public readonly uint dllVersion;
        public readonly uint gameVersionMajor;
        public readonly uint gameVersionMinor;
        public readonly uint currentGame;
        public readonly uint telemetryVersionMajor;
        public readonly uint telemetryVersionMinor;

        public readonly uint gameTime;
        public readonly uint forwardGearCount;
        public readonly uint reverseGearCount;
        public readonly uint retarderStepCount;
        public readonly uint wheelCount;
        public readonly uint selectorCount;
        public readonly uint deliveryTime;
        public readonly uint maxTrailerCount;
        public readonly uint jobCargoUnitCount;
        public readonly uint plannedJobDistanceKm;

        public readonly uint hShifterSlot;
        public readonly uint retarderLevel;
        public readonly uint lightsAuxFront;
        public readonly uint lightsAuxRoof;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = WheelSize)] public readonly uint[] wheelSubstances;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SlotSize)] public readonly uint[] slotHandlePosition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SlotSize)] public readonly uint[] slotSelectors;

        public readonly uint jobDeliveryTime;
        public readonly uint jobStartTime;
        public readonly uint jobFinishingTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)] readonly int[] _g2; // Gap of 48 bytes


        // ----------------------------------------------
        // Third zone (offset 500)
        // ----------------------------------------------
        public readonly int nextRestStop;
        public readonly int selectedGear;
        public readonly int dashboardGear;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SlotSize)] public readonly int[] slotGear;
        public readonly int earnedXp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)] readonly int[] _g3; // Gap of 56 bytes


        // ----------------------------------------------
        // Fourth zone (offset 700)
        // ----------------------------------------------
        public readonly float scale;

        public readonly float fuelCapacity;
        public readonly float fuelWarningLevel;
        public readonly float adblueCapacity;
        public readonly float adblueWarningLevel;
        public readonly float airPressureWarningLevel;
        public readonly float airPressureEmergencyLevel;
        public readonly float oilPressureWarningLevel;
        public readonly float waterTemperatureWarningLevel;
        public readonly float batteryVoltageWarningLevel;
        public readonly float engineRpmMax;
        public readonly float differentialRatio;
        public readonly float jobCargoMass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = WheelSize)] public readonly float[] wheelRadii;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)] public readonly float[] gearRatiosForward;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public readonly float[] gearRatiosReverse;
        public readonly float jobCargoUnitMass;

        public readonly float speed; // meters per second
        public readonly float rpm;
        public readonly float steeringInput;
        public readonly float throttleInput;
        public readonly float brakeInput;
        public readonly float clutchInput;
        public readonly float steeringGame;
        public readonly float throttleGame;
        public readonly float brakeGame;
        public readonly float clutchGame;


        // ----------------------------------------------
        // Fifth zone (offset 1500)
        // ----------------------------------------------

        // ----------------------------------------------
        // Sixth zone (offset 1640)
        // ----------------------------------------------

        // ----------------------------------------------
        // Seventh zone (offset 2000)
        // ----------------------------------------------

        // ----------------------------------------------
        // Eigth zone (offset 2200)
        // ----------------------------------------------

        // ----------------------------------------------
        // Nineth zone (offset 2300)
        // ----------------------------------------------

        // ----------------------------------------------
        // Tenth zone (offset 4000)
        // ----------------------------------------------

        // ----------------------------------------------
        // Eleventh zone (offset 4200)
        // ----------------------------------------------

        // ----------------------------------------------
        // Twelveth zone (offset 4300)
        // ----------------------------------------------

        // ----------------------------------------------
        // Thirteenth zone (offset 4400)
        // ----------------------------------------------

        // ----------------------------------------------
        // Fourteenth zone (offset 6000)
        // ----------------------------------------------
    }
}
