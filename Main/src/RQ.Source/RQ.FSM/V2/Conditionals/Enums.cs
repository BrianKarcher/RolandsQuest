namespace RQ.FSM.V2.Conditionals
{
    public enum Operator
    {
        Equal = 0,
        NotEqual = 1,
        GreaterThen = 2,
        LessThen = 3,
        GreaterThenOrEqualTo = 4,
        LessThanOrEqualTo = 5,
        Between = 6
    }

    public enum FloatVariableEnum
    {
        Altitude = 0,
        HP = 1,
        InputVelocity = 2,
        DistanceSqToTarget = 3,
        DistanceSqToTargetOffset = 4,
        DistanceSqToSteeringTarget = 5,
        FloatVariable = 6,
        DistanceSqToSteeringVector = 7,
        AirVelocity = 8,
        Gravity = 9,
        Velocity = 10,
        PreviousVelocity = 11
    }

    public enum BoolVariableEnum
    {
        IsHiding = 0,
        Invincible = 1,
        //MoveOnInput = 2,
        IsInputEnabled = 3,
        IsAutoStart = 4,
        GlobalBoolVariable = 5,
        SetFacingDirectionOnInput = 6,
        //IsPushing = 7
        IsKinematic = 8,
        Toggling = 9,
        IsCrafted = 10
    }

    public enum VectorVariableEnum
    {
        FacingDirectionVector = 0,
        TargetFacingDirectionVector = 1,
        VectorToTarget = 2,
        Position = 3,
        DirectionToDamageDealer = 4,
        Velocity = 5,
        Force = 6,
        FootPosition = 7,
        InputVelocity = 8
    }

    public enum StringVariableEnum
    {
        ModalText = 0
    }

    public enum SpriteBoolVariable
    {
        UsableInRange = 0
    }
}
