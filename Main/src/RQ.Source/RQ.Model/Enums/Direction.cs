using UnityEngine;

namespace RQ
{
	public enum Direction {
		Left = 0,
		Right = 1,
		Up = 2,
		Down = 3,
		UpLeft = 4,
		UpRight = 5,
		DownLeft = 6,
		DownRight = 7,
		None = 9,
        Any = 10
	}

    public static class DirectionExtensions
    {
        public static float GetDirectionAngle(this Direction direction)
        {
            switch (direction)
            {
                case RQ.Direction.Right:
                    return 0f;
                case RQ.Direction.Up:
                    return 90f;
                case RQ.Direction.Left:
                    return 180f;
                default:
                    return 270f;
            }
        }

        public static Vector2 GetSideVector(this Direction direction)
        {
            switch (direction)
            {
                case RQ.Direction.Right:
                    return Vector2.down;
                case RQ.Direction.Up:
                    return Vector2.right;
                case RQ.Direction.Left:
                    return Vector2.up;
                default:
                    return Vector2.left;
            }
        }
    }


    //public enum FacingDirection
    //{
    //    Left = 0,
    //    Right = 1,
    //    Up = 2,
    //    Down = 3,
    //    GameControlled = 4
    //}

    public enum DirectionMode
    {
        Automatic = 0,
        Manual = 1,
        FacePlayer = 2
    }
}