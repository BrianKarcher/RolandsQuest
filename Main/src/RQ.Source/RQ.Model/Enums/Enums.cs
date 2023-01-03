namespace RQ.Model.Enums
{
    public enum ActionTarget
    {
        Target = 0,
        Self = 1,
        MainCharacter = 2,
        Companion = 3,
        Waypoint1 = 4,
        Waypoint2 = 5,
        MCJointUnit = 6,
        Waypoint3 = 7,
        ManualVector = 8,
        Parent = 9
    }

    public enum CollisionActionType
    {
        None = 0,
        Normal = 1,
        LiftIntoAir = 2
    }

    public enum DirectionToCheck
    {
        Heading = 0,
        FacingDirection = 1
    }
}
