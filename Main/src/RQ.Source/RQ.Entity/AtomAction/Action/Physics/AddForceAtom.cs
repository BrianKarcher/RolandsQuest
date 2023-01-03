using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class AddForceAtom : AtomActionBase
    {
        public Vector2 force;
        public string _physicsComponentName;
        private PhysicsComponent _physicsComponent;
        public string PhysicsAffector;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);

            AddForce();
            //_isRunning = false;
        }

        private void AddForce()
        {
            if (string.IsNullOrEmpty(PhysicsAffector))
            {
                _physicsComponent.AddForce(force);
            }
            else
            {
                _physicsComponent.GetPhysicsAffector(PhysicsAffector).Force += force;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            AddForce();
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
