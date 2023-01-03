using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Slow Entity")]
    public class SlowEntity : ActionBase
    {
        [SerializeField]
        private float _maxSpeedMultiplier = 1f;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            // TODO Make the speed multipliers additive
            //var spriteBase = otherRigidBody.GetComponent<SpriteBaseComponent>();
            //if (spriteBase == null)
            //    return;

            //var physicsComponent = spriteBase.Components.GetComponent<PhysicsComponent>();
            //if (physicsComponent == null)
            //    return;

            if (_physicsComponent == null)
                return;

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.SetMaxSpeedMultiplier, _maxSpeedMultiplier);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);

            //var spriteBase = otherRigidBody.GetComponent<SpriteBaseComponent>();
            //if (spriteBase == null)
            //    return;

            //var physicsComponent = spriteBase.Components.GetComponent<PhysicsComponent>();
            //if (physicsComponent == null)
            //    return;

            if (_physicsComponent == null)
                return;

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.SetMaxSpeedMultiplier, 1f);
        }
    }
}
