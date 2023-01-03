using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Stop Movement")]
    public class StopMovement : ActionBase
    {
        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.StopMovement, null);
        }
    }
}
