using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class AddVelocityAtom : AtomActionBase
    {
        public Vector2 velocity;
        public string _physicsComponentName;
        //[SerializeField]
        //public ShootTarget _shootTarget = ShootTarget.Random;
        private PhysicsComponent _physicsComponent;
        public string PhysicsAffector;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);
            _physicsComponent.SetVelocity(_physicsComponent.GetVelocity() + velocity);
            _isRunning = false;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
