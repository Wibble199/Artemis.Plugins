
// http://edcodex.info/?m=doc

namespace Module.EliteDangerous.Journal {

    public enum StarClass {
        O, B, A, F, G, K, M, L, T, Y, // Main sequence
        TTS, AeBe, // Proto
        W, WN, WNC, WC, WO, // Wolf-Rayet
        CS, C, CN, CJ, CH, CHd, // Carbon
        MS, S,
        D, DA, DAB, DAO, DAZ, DAV, DB, DBZ, DBV, DO, DOV, DQ, DC, DCV, DX, // White dwarfs
        N, // Neutron
        H, // Black hole
        X, // Exotic
        SupermassiveBlackHole,
        A_BlueWhiteSuperGiant,
        F_WhiteSuperGiant,
        M_RedSuperGiant,
        M_RedGiant,
        K_OrangeGiant,
        RoguePlanet,
        Nebula,
        StellarRemnantNebula
    }

    public enum JumpType {
        Supercruise,
        Hyperspace
    }

    public enum CombatRank {
        Harmless = 0,
        Mostly_Harmless = 1,
        Novice = 2,
        Competent = 3,
        Expert = 4,
        Master = 5,
        Dangerous = 6,
        Deadly = 7,
        Elite = 8
    }

    public enum TradeRank {
        Penniless = 0,
        Mostly_Penniless = 1,
        Peddler = 2,
        Dealer = 3,
        Merchant = 4,
        Broker = 5,
        Entrepreneur = 6,
        Tycoon = 7,
        Elite = 8
    }

    public enum ExplorerRank {
        Aimless = 0,
        Mostly_Aimless = 1,
        Scout = 2,
        Surveyor = 3,
        Trailblazer = 4,
        Pathfinder = 5,
        Ranger = 6,
        Pioneer = 7,
        Elite = 8
    }

    public enum CQCRank {
        Helpless = 0,
        Mostly_Helpless = 1,
        Amateur = 2,
        Semi_Professional = 3,
        Professional = 4,
        Champion = 5,
        Hero = 6,
        Legend = 7,
        Elite = 8
    }

    public enum EmpireRank {
        None = 0,
        Outsider = 1,
        Serf = 2,
        Master = 3,
        Squire = 4,
        Knight = 5,
        Lord = 6,
        Baron = 7,
        Viscount = 8,
        Count = 9,
        Earl = 10,
        Marquis = 11,
        Duke = 12,
        Prince = 13,
        King = 14
    }

    public enum FederationRank {
        None = 0,
        Recruit = 1,
        Cadet = 2,
        Midshipman = 3,
        Petty_Officer = 4,
        Chief_Petty_Officer = 5,
        Warrant_Officer = 6,
        Ensign = 7,
        Lieutenant = 8,
        Lieutenant_Commander = 9,
        Post_Commander = 10,
        Post_Captain = 11,
        Rear_Admiral = 12,
        Vice_Admiral = 13,
        Admiral = 14
    }
}
