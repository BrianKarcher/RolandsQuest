using RQ.Common;
using RQ.Enums;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.Components
{
    public interface IComponentRepository : IEntity
    {
        void Init();
        void Reset();
        void ReAwaken();
        void Destroy();
        T GetComponent<T>();
        T GetComponentInChildren<T>();
        void SendMessageToComponents<T>(float delay, string senderId, Telegrams msg, object extraInfo)
             where T : class, IBaseObject;
        //void StartListening(string eventName, string id, Action<Telegram2> callbackMethod, bool addLocal = false);
        //void StopListening(string eventName, string id, bool removeLocal = false);
        //bool isActiveAndEnabled { get; }
        //Transform Instantiate(Transform original, Vector3 position, Quaternion rotation);
        Coroutine StartCoroutine(string name);
        void StopCoroutine(string name);
    }
}
