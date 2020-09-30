using Artemis.Core;
using Artemis.Core.Modules;
using SkiaSharp;
using Module.EliteDangerous.DataModels;
using System.Collections.Generic;
using Module.EliteDangerous.ViewModels;
using Module.EliteDangerous.Journal;
using System;
using System.IO;
using Module.EliteDangerous.Status;

namespace Module.EliteDangerous {

    public class EliteDangerousModule : ProfileModule<EliteDangerousDataModel> {

        private static readonly string EliteDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @"Saved Games\Frontier Developments\Elite Dangerous"
        );

        private JournalParser journalParser;
        private StatusParser statusParser;

        public override void EnablePlugin() {
            DisplayName = "Elite: Dangerous";
            DisplayIcon = "StarFourPoints";
            DisplayIconPath = "Elite-Dangerous.png";
            DefaultPriorityCategory = ModulePriorityCategory.Application;
            ActivationRequirements.Add(new ProcessActivationRequirement("EliteDangerous64"));
            UpdateDuringActivationOverride = true;
            //ModuleTabs = new List<ModuleTab> { new ModuleTab<CustomViewModel>("Settings") };

            journalParser = new JournalParser(EliteDataDirectory);
            statusParser = new StatusParser(EliteDataDirectory);
        }

        public override void DisablePlugin() {
            journalParser.Dispose();
            statusParser.Dispose();
        }

        public override void Update(double deltaTime) {
            journalParser.PerfomRead(DataModel);
            statusParser.PerfomRead(DataModel);
        }

        public override void ModuleActivated(bool isOverride) { }
        public override void ModuleDeactivated(bool isOverride) { }

        public override void Render(double deltaTime, ArtemisSurface surface, SKCanvas canvas, SKImageInfo canvasInfo) { }
    }
}