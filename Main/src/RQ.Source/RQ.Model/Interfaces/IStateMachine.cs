using RQ.Common;
using RQ.Entity.Components;
using UnityEngine;

namespace RQ.FSM.V2
{
    public interface IStateMachine : IBaseObject
    {
        //float StateStartTime { get; }
        Transform GetEntity();
        StateInfo GetStateInfo();
        //string UniqueId { get; }
        IComponentRepository GetComponentRepository();
        T GetComponent<T>();
        IState GetParentState();
        void CalculateNextState();
    }
}
