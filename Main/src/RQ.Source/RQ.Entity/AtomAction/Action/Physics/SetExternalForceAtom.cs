using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class SetExternalForceAtom : AtomActionBase
    {
        public Vector2 force;
        public string _physicsComponentName;
        private PhysicsComponent _physicsComponent;
        public string AffectorName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);
            if (string.IsNullOrEmpty(AffectorName))
                Debug.LogError($"Entity {entity.name} has SetExternalForce with no Affector Name");
            _physicsComponent.GetPhysicsAffector(AffectorName).Force = force;
            _isRunning = false;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
