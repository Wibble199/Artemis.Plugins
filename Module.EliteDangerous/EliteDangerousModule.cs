using Artemis.Core;
using Artemis.Core.Modules;
using SkiaSharp;
using Module.EliteDangerous.DataModels;
using System.Collections.Generic;
using Module.EliteDangerous.ViewModels;
using Module.EliteDangerous.Journal;

namespace Module.EliteDangerous {

    public class EliteDangerousModule : ProfileModule<EliteDangerousDataModel> {

        private JournalParser parser;

        public override void EnablePlugin() {
            DisplayName = "Elite: Dangerous";
            DisplayIcon = "StarFourPoints";
            DisplayIconPath = "Elite-Dangerous.png";
            DefaultPriorityCategory = ModulePriorityCategory.Application;
            ActivationRequirements.Add(new ProcessActivationRequirement("EliteDangerous64"));
            UpdateDuringActivationOverride = true;
            //ModuleTabs = new List<ModuleTab> { new ModuleTab<CustomViewModel>("Settings") };

            parser = new JournalParser();
        }

        public override void DisablePlugin() {
            parser.Dispose();
        }

        public override void Update(double deltaTime) {
            parser.PerfomRead(DataModel);
        }

        public override void ModuleActivated(bool isOverride) { }
        public override void ModuleDeactivated(bool isOverride) { }

        public override void Render(double deltaTime, ArtemisSurface surface, SKCanvas canvas, SKImageInfo canvasInfo) { }
    }
}