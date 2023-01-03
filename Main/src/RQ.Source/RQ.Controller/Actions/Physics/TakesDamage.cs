using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Takes Damage")]
    public class TakesDamage : ActionBase
    {
        [SerializeField]
        private bool _takesDamage = false;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
                Enums.Telegrams.SetTakesDamage, _takesDamage);
        }
    }
}
