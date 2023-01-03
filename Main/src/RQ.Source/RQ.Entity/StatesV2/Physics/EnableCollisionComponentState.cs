using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/Enable Collision Component")]
    public class EnableCollisionComponentState : StateBase
    {
        [SerializeField]
        private bool _enabled = true;

        public override void Enter()
        {
            base.Enter();
            var collisionComponent = _componentRepository.Components.GetComponent<CollisionComponent>();
            MessageDispatcher2.Instance.DispatchMsg("EnableCollisionComponent", 0f, this.UniqueId,
                collisionComponent.UniqueId, _enabled);
            Complete();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
