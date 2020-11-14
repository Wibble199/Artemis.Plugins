﻿using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {
    internal class ApproachBodyEvent : IJournalEvent {

        public string StarSystem;
        public string Body;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.UpdateLocation(StarSystem, Body);
            model.Navigation.ApproachBody.Trigger();
        }
    }
}
