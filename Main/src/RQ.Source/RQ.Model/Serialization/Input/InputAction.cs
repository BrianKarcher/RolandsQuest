namespace RQ.Model.Serialization.Input
{
    public enum InputAction
    {
        Horizontal = 0,
        MoveRight = 1,
        MoveLeft = 2,
        Vertical = 3,
        MoveUp = 4,
        MoveDown = 5,
        Attack = 6,
        Special = 7,
        Dash = 8,
        Menu = 9,
        SkillMenu = 10
    }

    public static class Extensions
    {
        public static string ToFriendlyName(this InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.MoveDown:
                    return "Move Down";
                case InputAction.MoveLeft:
                    return "Move Left";
                case InputAction.MoveRight:
                    return "Move Right";
                case InputAction.MoveUp:
                    return "Move Up";
                case InputAction.SkillMenu:
                    return "Skill Menu";
                default:
                    return inputAction.ToString();
            }
        }
    }
}
