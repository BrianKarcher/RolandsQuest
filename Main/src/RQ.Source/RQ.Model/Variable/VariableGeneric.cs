using RQ.Model.Enums;
using RQ.Physics;
using RQ.Serialization;
using System;
using UnityEngine;

namespace RQ.Model
{
    [Serializable]
    public class StringVariable : Variable<string> { }
    [Serializable]
    public class BoolVariable : Variable<bool> { }
    [Serializable]
    public class FloatVariable : Variable<float> { }
    [Serializable]
    public class Vector2Variable : Variable<Vector2D> { }
    [Serializable]
    public class Vector3Variable : Variable<Vector3Serializer> { }

    [Serializable]
    public class Variable<T>
    {
        public string Name;
        public T Value;
        //public string UniqueId;
        //public StatusPersistenceLength Persistence;

        public Variable<T> Clone()
        {
            return new Variable<T>()
            {
                Name = Name,
                Value = Value
                //UniqueId = UniqueId
                //Persistence = Persistence
            };
        }
    }
}
