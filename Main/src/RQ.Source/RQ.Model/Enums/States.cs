namespace RQ.Enum
{
	public enum GameState
	{
		TitleScreen = 1,
		LoadScreen = 2,
		SaveScreen = 3,
		EscapeMenu = 4,
		Dialog = 5,
		Play = 6,
		Cutscene = 7,
        Conversation = 8,
        MainMenu = 9
	}

    public enum Bomb
    {
        Flying = 0
    }

    public enum BombExplosion
    {
        Explode = 0
    }

    public enum PlantBombthrower
    {
        Attack = 0,
        Idle = 1,
        Regrow = 2,
        Walk = 3
    }

    public enum GolemSentry_MainStates
    {
        Melee = 0,
        Ranged = 1,
        Spin = 2
    }

    public enum GolemSentry_Melee
    {
        WalkToPlayer = 0,
        PunchRight = 1,
        PunchLeft = 2,
        FloorSmash = 3
    }

    public enum GolemSentry_Ranged
    {
        GoToAttackLocation = 0,
        SweepLaserFromLeft = 1,
        SweepLaserFromRight = 2,
        FanningLasers = 3
    }

    public enum GolemSentry_Spin
    {
        GoToAttackLocation = 0,
        Spin = 1
    }

    public enum RolanV2
    {
        Active = 0,
        Attack = 1,
        Stunned = 2
    }

    public enum Door
    {
        Closed = 0,
        Open = 1
    }

}

