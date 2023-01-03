using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Treasure Open")]
    public class TreasureOpenState : AnimatorState
    {
        public override void Enter()
        {
            base.Enter();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
            var usableObject = _spriteBase.Components.GetComponent<UsableComponent>();
            if (usableObject != null)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, usableObject.UniqueId,
                    Enums.Telegrams.SetEnabled, false);
            }
        }
    }
}
