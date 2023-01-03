using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class ApplyFrictionAtom : AtomActionBase
    {
        public float magnitude;
        public string _physicsComponentName;
        //[SerializeField]
        //public ShootTarget _shootTarget = ShootTarget.Random;
        private PhysicsComponent _physicsComponent;
        public string PhysicsAffector;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);

            //_isRunning = false;

            //if (_physicsData.DragForce != 0f)
            //{
            //    // Calculate the drag force and add it to Velocity.
            //    var normalizedVelocity = ((Vector2)_physicsData.Velocity).normalized;
            //    var currentDragForce = normalizedVelocity * -1f * _physicsData.DragForce / 50f;
            //    //if (currentDragForce )
            //    _physicsData.Velocity += (Vector2D)currentDragForce;
            //}
        }

        public override AtomActionResults OnUpdate()
        {
            var physicsData = _physicsComponent.GetPhysicsData();
            //var affector = _physicsComponent.GetPhysicsAffector(PhysicsAffector);
            // No friction if we are not moving
            var velocity = _physicsComponent.GetVelocity();
            if (velocity == Vector2.zero)
                return AtomActionResults.Running;
            //    // Calculate the friction force.
            var normalizedVelocity = velocity.normalized;
            var currentFrictionForce = normalizedVelocity * -1f * magnitude;

            // If the friction is so strong as to reverse the velocity, set the friction to the -velocity which will make it zero
            // next update.
            var vectorAfterForceApplied = velocity + (currentFrictionForce / 50f);
            if (Vector2.Dot(velocity, vectorAfterForceApplied) < 0)
            {
                //currentFrictionForce = -(Vector2)affector.Velocity * 50f;
                currentFrictionForce = Vector2.zero;
                //affector.Velocity = Vector2.zero;
                _physicsComponent.SetVelocity(Vector2.zero);
            }

            //affector.Force = currentFrictionForce;
            _physicsComponent.AddForce(currentFrictionForce);
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
