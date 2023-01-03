using RQ.Messaging;
using UnityEngine;

namespace RQ.Common.Container
{
    public interface IComponentRepository : IMessagingObject
    {
        void Init();
        void Reset();
        void ReAwaken();
        void Destroy();
        T GetComponent<T>();
        T GetComponentInChildren<T>();
        Coroutine StartCoroutine(string name);
        void StopCoroutine(string name);
    }
}
