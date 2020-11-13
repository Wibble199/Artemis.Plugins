using Artemis.Core;
using Artemis.Core.Modules;
using SkiaSharp;
using Artemis.Plugins.Modules.TruckSimulator.DataModels;
using Artemis.Plugins.Modules.TruckSimulator.Telemetry;

namespace Artemis.Plugins.Modules.TruckSimulator {

    // Module for providing data to Euro and American Truck Simulator
    public class TruckSimulatorModule : ProfileModule<TruckSimulatorDataModel> {

        private MappedFileReader<TruckSimulatorMemoryStruct> mappedFileReader;

        public override void Enable() {
            DisplayName = "Truck Simulator (ETS2 & ATS)";
            DisplayIcon = "Truck";

            DefaultPriorityCategory = ModulePriorityCategory.Application;

            ActivationRequirementMode = ActivationRequirementType.Any;
            ActivationRequirements.Add(new ProcessActivationRequirement("eurotrucks2"));
            ActivationRequirements.Add(new ProcessActivationRequirement("amtrucks"));

            mappedFileReader = new MappedFileReader<TruckSimulatorMemoryStruct>(@"Local\SCSTelemetry");
        }

        public override void Update(double deltaTime) {
            if (!IsActivatedOverride && DataModel != null)
                DataModel.Telemetry = mappedFileReader?.Read() ?? default;
        }

        public override void Disable() {
            mappedFileReader?.Dispose();
            mappedFileReader = null;
        }


        // Unused abstract methods
        public override void ModuleActivated(bool isOverride) { }
        public override void ModuleDeactivated(bool isOverride) { }
        public override void Render(double deltaTime, ArtemisSurface surface, SKCanvas canvas, SKImageInfo canvasInfo) { }
    }
}