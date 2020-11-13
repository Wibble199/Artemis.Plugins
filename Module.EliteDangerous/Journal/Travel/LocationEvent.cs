﻿using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {

    // Written at startup and resurrection
    internal class LocationEvent : IJournalEvent {

        public string StarSystem;
        public string Body;
        public BodyType BodyType;
        public bool Docked;
        public string StationName;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            model.Navigation.CurrentSystem = StarSystem;
            model.Navigation.CurrentBody = Body;
            model.Navigation.CurrentBodyType = BodyType;
            model.Navigation.CurrentStation = StationName;
        }
    }
}
