using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/EntityPaused")]
    public class EntityPausedState : StateBase
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("PauseAnimation", 0f, this.UniqueId, 
                _componentRepository.UniqueId, null);
            MessageDispatcher2.Instance.DispatchMsg("StopMovement", 0f, this.UniqueId,
                _componentRepository.UniqueId, null);
        }

        public override void Exit()
        {
            base.Exit();
            MessageDispatcher2.Instance.DispatchMsg("ResumeAnimation", 0f, this.UniqueId,
                _componentRepository.UniqueId, null);
        }
    }
}
