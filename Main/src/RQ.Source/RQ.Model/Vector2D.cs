using RQ.Serialization;
//using RQ.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public struct Vector2D
    {
        public float x;
        public float y;

        //public Vector2D()
        //{
        //    x = 0f;
        //    y = 0f;
        //}
        public Vector2D(float a, float b)
        {
            x = a;
            y = b;
        }

        //sets x and y to zero
        public void SetToZero()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3Serializer))
                return false;
            var test = (Vector3Serializer)obj;

            if (test.x == this.x && test.y == this.y)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}-{1}", x, y).GetHashCode();
        }

        public static Vector2D Zero()
        {
            return new Vector2D(0f, 0f);
        }

        //returns true if both x and y are zero
        public bool isZero()
        {
            return (x * x + y * y) < float.Epsilon;
        }

        //returns the length of the vector
        public float Length()
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        //returns the squared length of the vector (thereby avoiding the sqrt)
        public float LengthSq()
        {
            return (x * x + y * y);
        }

        public void Normalize()
        {
            float vector_length = this.Length();

            if (vector_length > float.Epsilon)
            {
                this.x /= vector_length;
                this.y /= vector_length;
            }
        }

        public static Vector2D Vec2DNormalize(Vector2D v)
        {
            Vector2D vec = v;

            float vector_length = vec.Length();

            if (vector_length > float.Epsilon)
            {
                vec.x /= vector_length;
                vec.y /= vector_length;
            }

            return vec;
        }


        public static float Vec2DDistance(Vector2D v1, Vector2D v2)
        {

            float ySeparation = v2.y - v1.y;
            float xSeparation = v2.x - v1.x;

            return Mathf.Sqrt(ySeparation * ySeparation + xSeparation * xSeparation);
        }

        public static float Vec2DDistanceSq(Vector2D v1, Vector2D v2)
        {

            float ySeparation = v2.y - v1.y;
            float xSeparation = v2.x - v1.x;

            return ySeparation * ySeparation + xSeparation * xSeparation;
        }

        public static float Vec2DLength(Vector2D v)
        {
            return Mathf.Sqrt(v.x * v.x + v.y * v.y);
        }

        public static float Vec2DLengthSq(Vector2D v)
        {
            return (v.x * v.x + v.y * v.y);
        }

        public float Dot(Vector2D v2)
        {
            return x * v2.x + y * v2.y;
        }

        //returns positive if v2 is clockwise of this vector,
        //negative if anticlockwise (assuming the Y axis is pointing down,
        //X axis to right like a Window app)
        enum IsClockwise { clockwise = 1, anticlockwise = -1 };
        public int Sign(Vector2D v2)
        {
            if (y * v2.x > x * v2.y)
            {
                return (int)IsClockwise.anticlockwise;
            }
            else
            {
                return (int)IsClockwise.clockwise;
            }
        }

        //returns the vector that is perpendicular to this one.
        public Vector2D Perp()
        {
            return new Vector2D(-y, x);
        }

        //adjusts x and y so that the length of the vector does not exceed max
        public void Truncate(float max)
        {
            if (this.Length() > max)
            {
                this.Normalize();

                this *= max;
            }
        }

        //returns the distance between this vector and th one passed as a parameter
        public float Distance(Vector2D v2)
        {
            float ySeparation = v2.y - y;
            float xSeparation = v2.x - x;

            return Mathf.Sqrt(ySeparation * ySeparation + xSeparation * xSeparation);
        }

        //squared version of above.
        public float DistanceSq(Vector2D v2)
        {
            float ySeparation = v2.y - y;
            float xSeparation = v2.x - x;

            return ySeparation * ySeparation + xSeparation * xSeparation;
        }

        public void Reflect(Vector2D norm)
        {
            this += 2.0f * this.Dot(norm) * norm.GetReverse();
        }

        //returns the vector that is the reverse of this vector
        public Vector2D GetReverse()
        {
            return new Vector2D(-this.x, -this.y);
        }

        //public static bool operator ==(Vector3Serializer lhs, Vector3Serializer rhs)
        //we need some overloaded operators
        //public static Vector2D operator +=(Vector2D rhs)
        //{
        //  x += rhs.x;
        //  y += rhs.y;

        //  return this;
        //}

        //const Vector2D operator-=(const Vector2D rhs)
        //{
        //  x -= rhs.x;
        //  y -= rhs.y;

        //  return *this;
        //}

        //const Vector2D& operator*=(const double& rhs)
        //{
        //  x *= rhs;
        //  y *= rhs;

        //  return *this;
        //}

        //const Vector2D& operator/=(const double& rhs)
        //{
        //  x /= rhs;
        //  y /= rhs;

        //  return *this;
        //}

        public static bool operator ==(Vector2D lhs, Vector2D rhs)
        {
            //if (System.Object.ReferenceEquals(lhs, rhs))
            //{
            //    return true;
            //}

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator ==(Vector2D lhs, Vector2 rhs)
        {
            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Vector2D lhs, Vector2D rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator !=(Vector2D lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        public static Vector2D operator *(Vector2D lhs, float rhs)
        {
            Vector2D result = new Vector2D(lhs.x * rhs, lhs.y * rhs);
            //result.x *= rhs;
            //result.y *= rhs;
            //result *= rhs;
            return result;
        }

        public static Vector2D operator *(float lhs, Vector2D rhs)
        {
            Vector2D result = new Vector2D(rhs.x * lhs, rhs.y * lhs);
            
            //result *= lhs;
            return result;
        }

        public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
        {
            Vector2D result = new Vector2D(lhs.x - rhs.x, lhs.y - rhs.y);
            //result.x -= rhs.x;
            //result.y -= rhs.y;

            return result;
        }

        public static Vector2D operator -(Vector2D lhs, float rhs)
        {
            Vector2D result = new Vector2D(lhs.x - rhs, lhs.y - rhs);
            //result.x -= rhs.x;
            //result.y -= rhs.y;

            return result;
        }

        public static Vector2D operator -(Vector2D lhs, Vector3 rhs)
        {
            Vector2D result = new Vector2D(lhs.x - rhs.x, lhs.y - rhs.y);
            //result.x -= rhs.x;
            //result.y -= rhs.y;

            return result;
        }

        public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
        {
            Vector2D result = new Vector2D(lhs.x + rhs.x, lhs.y + rhs.y);
            //result.x += rhs.x;
            //result.y += rhs.y;

            return result;
        }

        public static Vector2D operator +(Vector2D lhs, float rhs)
        {
            Vector2D result = new Vector2D(lhs.x + rhs, lhs.y + rhs);
            //result.x += rhs.x;
            //result.y += rhs.y;

            return result;
        }

        public static Vector2D operator /(Vector2D lhs, float val)
        {
            Vector2D result = new Vector2D(lhs.x / val, lhs.y / val);
            //result.x /= val;
            //result.y /= val;

            return result;
        }

        public Vector3 ToVector3(float z)
        {
            return new Vector3(this.x, this.y, z);
        }

        //public Vector2 ToVector2()
        //{
        //    return new Vector2(this.x, this.y);
        //}

        // Convert from Vector3 to Vector2D
        public static implicit operator Vector2D(Vector3 v)
        {
            return new Vector2D(v.x, v.y); 
        }

        public static implicit operator Vector2D(Vector3Serializer v)
        {
            return new Vector2D(v.x, v.y);
        }

        public static implicit operator Vector2D(Vector2 v)
        {
            return new Vector2D(v.x, v.y);
        }

        public static implicit operator Vector2(Vector2D v)
        {
            return new Vector2(v.x, v.y);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        // Convert from Vector3 to Vector2D
        //public static Vector2D ToVector2D(this Vector3 newV)
        //{
        //    return new Vector2D(newV.x, newV.y);
        //}

        public static T GetClosestBehaviourToPoint<T>(Vector2D point, IEnumerable<T> behaviours)
            where T : MonoBehaviour
        {
            T closestBehaviour = null;
            float closestDistance = 5000f;
            foreach (var behaviour in behaviours)
            {
                var distance = Vector2D.Vec2DDistanceSq(behaviour.transform.position,
                    point);
                if (distance < closestDistance)
                {
                    closestBehaviour = behaviour;
                    closestDistance = distance;
                }
            }
            return closestBehaviour;
        }
    }
}
