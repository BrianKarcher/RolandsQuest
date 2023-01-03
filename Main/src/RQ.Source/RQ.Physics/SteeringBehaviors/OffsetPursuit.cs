using RQ.Physics.Components;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class OffsetPursuit : SteeringBehaviorBase, ISteeringBehavior
    {
        private CollisionComponent collisionComponent;

        public OffsetPursuit(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.OffsetPursuitWeight;
        }
        //this behavior maintains a position, in the direction of offset
        //from the target vehicle
        protected override Vector2D CalculateForce()
        {
            return SteeringBehaviorCalculations.OffsetPursuit(_steeringBehaviorManager.TargetAgent1, 
               _steeringBehaviorManager.Offset, _steeringBehaviorManager.Entity);
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            var targetPos = _steeringBehaviorManager.TargetAgent1.GetWorldPos() + _steeringBehaviorManager.Offset;
            bool hasLineOfSight = HasLineOfSight(targetPos);
            Gizmos.color = hasLineOfSight ? Color.blue : Color.red;
            Gizmos.DrawLine(_steeringBehaviorManager.Entity.transform.position, targetPos);
            Gizmos.DrawSphere(targetPos, 0.16f);
        }

        public bool HasLineOfSight(Vector3 target)
        {
            if (collisionComponent == null)
                collisionComponent = _steeringBehaviorManager.Entity.GetComponentRepository().Components.GetComponent<CollisionComponent>();
            //var obstacleLayerMask = collisionComponent.GetEnvironmentLayerMask();
            var obstacleLayerMask = LayerMask.NameToLayer("Environment");
            var currentPos = (Vector3)_steeringBehaviorManager.Entity.transform.position;
            //var ray = new Ray(currentPos, target - currentPos);

            //var rayCasts = UnityEngine.Physics.RaycastAll(currentPos, target - currentPos, (target - currentPos).magnitude, obstacleLayerMask);
            //var layerMask = 1 << _obstacleLayerMask;
            return (!UnityEngine.Physics.Raycast(currentPos, target - currentPos, (target - currentPos).magnitude, obstacleLayerMask));
        }
    }
}
