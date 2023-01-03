using System;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IEventTrigger
    {
        event EventHandler EventEnded;
        void Trigger(Transform requester);
    }
}
