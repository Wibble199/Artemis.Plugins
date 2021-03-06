﻿using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Plugins.Modules.EliteDangerous.DataModels;
using Artemis.Plugins.Modules.EliteDangerous.Journal;
using Artemis.Plugins.Modules.EliteDangerous.Status;
using System;
using System.IO;

namespace Artemis.Plugins.Modules.EliteDangerous {

    [PluginFeature(AlwaysEnabled = true)]
    public class EliteDangerousModule : Module<EliteDangerousDataModel> {

        private static readonly string EliteDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @"Saved Games\Frontier Developments\Elite Dangerous"
        );

        private JournalParser journalParser;
        private StatusParser statusParser;

        public override void Enable() {
            DisplayName = "Elite: Dangerous";
            DisplayIcon = "Elite-Dangerous.svg";
            ActivationRequirements.Add(new ProcessActivationRequirement("EliteDangerous64"));

            journalParser = new JournalParser(EliteDataDirectory);
            statusParser = new StatusParser(EliteDataDirectory);
        }

        public override void Disable() {
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
    }
}