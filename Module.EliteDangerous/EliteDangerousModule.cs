using Artemis.Core;
using Artemis.Core.Modules;
using SkiaSharp;
using Module.EliteDangerous.DataModels;
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

            journalParser = new JournalParser(EliteDataDirectory);
            statusParser = new StatusParser(EliteDataDirectory);
        }

        public override void DisablePlugin() {
            journalParser.Dispose();
            statusParser.Dispose();
        }

        public override void Update(double deltaTime) {
            journalParser.PerformUpdate(DataModel);
            statusParser.PerformUpdate(DataModel);
        }

        public override void ModuleActivated(bool isOverride) {
            journalParser.Activate();
            statusParser.Activate();
        }

        public override void ModuleDeactivated(bool isOverride) {
            journalParser.Deactivate();
            statusParser.Deactivate();
        }

        public override void Render(double deltaTime, ArtemisSurface surface, SKCanvas canvas, SKImageInfo canvasInfo) { }
    }
}