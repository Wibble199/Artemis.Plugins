﻿using Artemis.Plugins.Modules.EliteDangerous.DataModels;

namespace Artemis.Plugins.Modules.EliteDangerous.Journal.Travel {
    internal class SupercruiseEntryEvent : IJournalEvent {

        public string StarSystem;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update InSupercruise, this is done via Status.json
            model.Navigation.EnterSupercruise.Trigger();
        }
    }

    internal class SupercruiseExitEvent : IJournalEvent {

        public string StarSystem;
        public string Body;
        public BodyType BodyType;

        public void ApplyUpdate(EliteDangerousDataModel model) {
            // Does not need to update InSupercruise, this is done via Status.json
            model.Navigation.CurrentSystem = StarSystem;
            model.Navigation.CurrentBody = Body;
            model.Navigation.CurrentBodyType = BodyType;
            model.Navigation.ExitSupercruise.Trigger();
        }
    }
}
