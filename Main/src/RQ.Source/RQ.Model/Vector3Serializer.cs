using System;
using UnityEngine;

namespace RQ.Serialization
{
    [Serializable]
    public struct Vector3Serializer
    {
        public float x;
        public float y;
        public float z;

        //public Vector3Serializer()
        //{

        //}

        public Vector3Serializer(float nx, float ny, float nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }

        public override bool Equals (object obj)
        {
            if (!(obj is Vector3Serializer))
                return false;
            var test = (Vector3Serializer) obj;

            if (test.x == this.x && test.y == this.y && test.z == this.z)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}-{1}-{2}", x, y, z).GetHashCode();
        }

        public override string ToString()
        {
            Vector3 v = new Vector3(x, y, z);
            return v.ToString();
        }

        public string ToString(string format)
        {
            //if (this == null)
            //{
            //    x = 0;
            //    y = 0;
            //    z = 0;
            //}
            //if (ser == null)
            //    return "(null)";
            Vector3 v = new Vector3(x, y, z);
            return v.ToString(format);
        }

        //public Vector3 Deserialize()
        //{
        //    return new Vector3(x, y, z);
        //}

        /// <summary>
        /// Automatic conversion from SerializableVector3 to Vector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator Vector3(Vector3Serializer rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SerializableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator Vector3Serializer(Vector3 rValue)
        {
            return new Vector3Serializer(rValue.x, rValue.y, rValue.z);
        }

        public static Vector3Serializer operator -(Vector3Serializer lhs, Vector3Serializer rhs)
        {
            Vector3Serializer result = new Vector3Serializer(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
            //result.x -= rhs.x;
            //result.y -= rhs.y;

            return result;
        }

        public static bool operator ==(Vector3Serializer lhs, Vector3Serializer rhs)
        {
            if (System.Object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }
        public static bool operator !=(Vector3Serializer lhs, Vector3Serializer rhs)
        {
            return !(lhs == rhs);
        }
    }
}
