using UnityEngine;

namespace RQ.FSM.V2.States
{
    [AddComponentMenu("RQ/States/State/End State")]
    public class StateEndState : StateBase
    {
        public override void Enter()
        {
            base.Enter();
            var stateMachineBase = StateMachine.GetParentState();
            if (stateMachineBase != null)
                stateMachineBase.Complete();
        }
    }
}
