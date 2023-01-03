using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Smart Go Home")]
    public class SmartGoHome : SmartFollow
    {
        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("SetMaxSpeed", 0f, this.UniqueId, 
                _componentRepository.UniqueId, _aiComponent.GetRunHomeSpeed());
        }

        public override void Exit()
        {
            base.Exit();
            MessageDispatcher2.Instance.DispatchMsg("SetMaxSpeedToOriginal", 0f, this.UniqueId, _componentRepository.UniqueId, null);
        }
    }
}
