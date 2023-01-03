using RQ.Common;
using RQ.Entity.Components;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IComponentBase : IBaseObject
    {        
        IComponentRepository GetComponentRepository();
        void Reset();
        void ReAwaken();
        void Init();
        void Destroy();
        Transform transform { get; }
        GameObject gameObject { get; }
        //void DeserializeUniqueIds(EntitySerializedData entitySerializedData);
        //void StartListening();
        //void StopListening();
    }
}
