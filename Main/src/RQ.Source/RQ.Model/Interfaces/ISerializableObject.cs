using RQ.Serialization;
using System;
using UnityEngine;
namespace RQ.Common
{
    public interface ISerializableObject
    {
        void Serialize(EntitySerializedData entitySerializedData);
        void Deserialize(EntitySerializedData entitySerializedData);
        // TODO See if I can remove this and make the UniqueId's populate automatically in Awake
        // prior to it setting the listeners
        void DeserializeUniqueIds(EntitySerializedData entitySerializedData);
        GameObject gameObject { get; }
    }
}
