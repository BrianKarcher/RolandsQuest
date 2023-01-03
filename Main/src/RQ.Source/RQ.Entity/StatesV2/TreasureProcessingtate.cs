using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Treasure Processing")]
    public class TreasureProcessingState : AnimatorState
    {
        public override void Enter()
        {
            base.Enter();
            //sprite.GetSteering().TurnOff(behavior_type.wander);

            MessageDispatcher2.Instance.DispatchMsg("AcquireItem", 0f, this.UniqueId, _componentRepository.UniqueId, null);
            MessageDispatcher2.Instance.DispatchMsg("DisplayAcquireModal", 0f, this.UniqueId, _componentRepository.UniqueId, null);
        }
    }
}
