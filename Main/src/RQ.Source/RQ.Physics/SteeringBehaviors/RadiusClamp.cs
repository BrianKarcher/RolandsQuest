using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class RadiusClamp : SteeringBehaviorBase, ISteeringBehavior
    {
        private Vector2D _clampVector;

        public RadiusClamp(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.InterposeWeight;
        }
        //this results in a steering force that attempts to steer the vehicle
        //to the center of the vector connecting two moving agents.
        protected override Vector2D CalculateForce()
        {
            _clampVector = SteeringBehaviorCalculations.RadiusClamp(_steeringBehaviorManager.Entity);
            return _clampVector;
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_steeringBehaviorManager.Entity.transform.position,
               _steeringBehaviorManager.Entity.transform.position + 
               _clampVector.ToVector3(_steeringBehaviorManager.Entity.transform.position.z));
            Gizmos.color = Color.green;
            var targetVector = _steeringBehaviorManager.Entity.GetSteering().GetTarget3();
            Gizmos.DrawSphere(targetVector, 0.04f);
            var physicsData = _steeringBehaviorManager.Entity.GetPhysicsData();
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetVector, physicsData.MaxDistanceToTarget);
            //Gizmos.DrawCube(targetVector.ToVector3(0f), new Vector3(physicsData.MaxDistanceToTarget, physicsData.MaxDistanceToTarget, 0f));
        }
    }
}
