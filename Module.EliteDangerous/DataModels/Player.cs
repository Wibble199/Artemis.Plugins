﻿using Artemis.Plugins.Modules.EliteDangerous.Status;

namespace Artemis.Plugins.Modules.EliteDangerous.DataModels {
    public class Player {
        public Vehicle CurrentlyPiloting { get; internal set; }
        public Ranks Ranks { get; } = new Ranks();
        public LegalState LegalState { get; internal set; }
        public bool InWing { get; internal set; }
    }
}
