using RQ.Serialization;
using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class TransformData
    {
        public Vector3Serializer Position { get; set; }
        public Vector3Serializer Scale { get; set; }
        public Vector3Serializer Rotation { get; set; }
    }
}
