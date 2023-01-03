using RQ.Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V2.States
{
    [AddComponentMenu("RQ/States/State/Change State")]
    public class ChangeState : PlaySoundState
    {
        [SerializeField]
        private List<ChangeStateData> _changeStates = null;

        public override void Enter()
        {
            base.Enter();
            if (_changeStates == null)
                return;

            foreach (var stateChange in _changeStates)
            {
                if (stateChange.State == null)
                    continue;
                Debug.LogError(stateChange.State.GetName() + " found in ChangeState, please remove.");
                // Only change states if the target is not already in that state
                // TODO: Add a Force State Change option
                var currentState = stateChange.StateMachine.GetCurrentState();
                if (currentState == null || currentState.UniqueId != stateChange.State.UniqueId)
                {
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                        stateChange.StateMachine.UniqueId, Enums.Telegrams.ChangeState,
                        stateChange.State.UniqueId);
                }
            }
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId, 
            //    Enums.Telegrams.StopMovement, null);
            //_physicsComponent.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        public override void Exit()
        {
            base.Exit();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
