using RQ.Physics;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static Vector3 ToVector(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Right:
                    return Vector3.right;
            }
            return Vector3.zero;
        }

        public static float GetAngleFromAxis(this Vector3 vector, Vector3 axis)
        {
            float angle = Vector2.Angle(axis, vector);

            // Cross product helps us determine which direction vector is facing, since Dot product does not go
            // past 180
            Vector3 cross = Vector3.Cross(axis, vector);

            if (cross.z < 0)
                angle = 360 - angle;
            return angle;
        }

        public static Vector3 RotateAroundAxis(this Vector3 v, float angle, Vector3 axis)
        {
            //if (bUseRadians) a *= MathUtil.RAD_TO_DEG;
            var q = Quaternion.AngleAxis(angle, axis);
            return q * v;
        }

        public static float DetermineWhichSidePointIsOn(this Vector2 v1, Vector2 direction, Vector2 point)
        {
            // https://www.gamedev.net/forums/topic/542870-determine-which-side-of-a-line-a-point-is/
            //Assuming the points are(Ax, Ay) (Bx, By) and(Cx, Cy), you need to compute:

            //(Bx - Ax) * (Cy - Ay) - (By - Ay) * (Cx - Ax)
            Vector2 v2 = v1 + direction;

            float d = (v2.x - v1.x) * (point.y - v1.y) - (v2.y - v1.y) * (point.x - v1.x);
            return d;
        }

        public static Direction GetDirection(this Vector3 velocity)
        {
            Vector2 from = Vector2.right;
            Vector2 to = velocity;

            float angle = Vector2.Angle(from, to);

            // Cross product helps us determine which direction vector is facing, since Dot product does not go
            // past 180
            Vector3 cross = Vector3.Cross(from, to);

            if (cross.z < 0)
                angle = 360 - angle;

            Direction newDirection = Direction.None;
            if (angle >= 315 || angle < 45)
            {
                //_mode = Mode.rRun;
                newDirection = Direction.Right;
            }
            else if (angle >= 45 && angle < 135)
            {
                //_mode = Mode.lRun;
                newDirection = Direction.Up;
            }
            else if (angle >= 135 && angle < 225)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.Left;
            }
            else
            {
                //_mode = Mode.dRun;
                newDirection = Direction.Down;
            }
            return newDirection;
        }

        /// <summary>
        /// Up, down, left, right, up-left, up-right, down-left, down-right
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static Direction GetDirection8(this Vector3 velocity)
        {
            Vector2 from = Vector2.right;
            Vector2 to = velocity;

            float angle = Vector2.Angle(from, to);

            // Cross product helps us determine which direction vector is facing, since Dot product does not go
            // past 180
            Vector3 cross = Vector3.Cross(from, to);

            if (cross.z < 0)
                angle = 360 - angle;

            Direction newDirection = Direction.None;
            if (angle >= 337.5 || angle < 22.5)
            {
                //_mode = Mode.rRun;
                newDirection = Direction.Right;
            }
            else if (angle >= 22.5 && angle < 67.5)
            {
                //_mode = Mode.lRun;
                newDirection = Direction.UpRight;
            }
            else if (angle >= 67.5 && angle < 112.5)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.Up;
            }
            else if (angle >= 112.5 && angle < 157.5)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.UpLeft;
            }
            else if (angle >= 157.5 && angle < 202.5)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.Left;
            }
            else if (angle >= 202.5 && angle < 247.5)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.DownLeft;
            }
            else if (angle >= 247.5 && angle < 292.5)
            {
                //_mode = Mode.uRun;
                newDirection = Direction.Down;
            }
            //else if (angle >= 292.5 && angle < 337.5)
            //{
            //    //_mode = Mode.uRun;
            //    newDirection = Direction.DownRight;
            //}
            else
            {
                //_mode = Mode.dRun;
                newDirection = Direction.DownRight;
            }
            return newDirection;
        }

        public static Direction GetDirection(this Vector2 velocity)
        {
            // Call the function above
            Vector3 v = velocity;
            return v.GetDirection();
        }

        public static Direction GetDirection(this Vector2D velocity)
        {
            // Call the function above
            Vector3 v = velocity.ToVector3(0);
            return v.GetDirection();
        }

        public static Direction GetDirection8(this Vector2D velocity)
        {
            // Call the function above
            Vector3 v = velocity.ToVector3(0);
            return v.GetDirection8();
        }

        public static Vector3Serializer Serialize(this Vector3 vector)
        {
            return new Vector3Serializer(vector.x, vector.y, vector.z);
        }

        //public static void SetX(this Vector3 vector, float x)
        //{
        //    vector.x = x;
        //}

        //public static void SetY(this Vector3 vector, float y)
        //{
        //    vector.y = y;
        //}

        //public static void SetZ(this Vector3 vector, float z)
        //{
        //    vector.z = z;
        //}

        //public static Vector2 SnapToGrid(this Vector3 pos, Vector3 offset)
        //{
        //    // Place the position in the middle of the tile
        //    // so the wrong tile is not chosen accidently due to innacuracies
        //    // in floating point math
        //    //pos += new Vector3(.08f, .08f, 0f);
        //    //pos += offset;
        //    Vector2 newPos = new Vector2(Mathf.FloorToInt(pos.x * 100f / 16f),
        //    Mathf.FloorToInt(pos.y * 100f / 16f));

        //    newPos *= (16f / 100f);

        //    //newPos += offset;
        //    newPos = new Vector2(newPos.x - offset.x, newPos.y - offset.y);
        //    //newPos -= new Vector2(.08f, .08f);

        //    //newPos += new Vector2(.08f, .08f); // Place in center of tile
        //    return newPos;
        //}

        //public static Vector2 SnapToGrid(this Vector3 pos, tk2dTileMap tileMap, Vector3 offset)
        //{
        //    int x, y;
        //    tileMap.GetTileAtPosition(pos, out x, out y);
        //    var newPos = tileMap.GetTilePosition(x, y);
        //    // Place the position in the middle of the tile
        //    // so the wrong tile is not chosen accidently due to innacuracies
        //    // in floating point math
        //    //pos += new Vector3(.08f, .08f, 0f);
        //    //pos += offset;
        //    //Vector2 newPos = new Vector2(Mathf.FloorToInt(pos.x * 100f / 16f),
        //    //Mathf.FloorToInt(pos.y * 100f / 16f));

        //    //newPos *= (16f / 100f);
        //    newPos += offset;

        //    //newPos += offset;
        //    //newPos = new Vector2(newPos.x - offset.x, newPos.y - offset.y);
        //    //newPos -= new Vector2(.08f, .08f);

        //    //newPos += new Vector2(.08f, .08f); // Place in center of tile
        //    return newPos;
        //}

        public static void SetPosX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        public static void SetPosY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        public static void SetPosZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static void SetLocalPosX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }
        public static void SetLocalPosY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }
        public static void SetLocalPosZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }
    }
}
