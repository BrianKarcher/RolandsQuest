using System;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public class Point : IPoint
    {
        //public Vector2D Point { get; set; }
        //[SerializeField]
        //private int _id;
        public float X;
        public float Y;
        public Transform transform;

        //public int Id
        //{
        //    get
        //    {
        //        return _id;
        //    }
        //    set
        //    {
        //        _id = value;
        //    }
        //}

        public int Id { get; set; }

        public Point(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
        }

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D Get()
        {
            return new Vector2D(X, Y);
        }

        public Point()
        {

        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public Vector3 ToVector3(float z)
        {
            return new Vector3(X, Y, z);
        }
    }
}
