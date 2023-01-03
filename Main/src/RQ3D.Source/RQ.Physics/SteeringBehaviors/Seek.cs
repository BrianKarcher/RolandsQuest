using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Seek : SteeringBehaviorBase, ISteeringBehavior
    {
        public Seek(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.SeekWeight;
        }
        //this behavior moves the agent towards a target position
        protected override Vector2 CalculateForce()
        {
            return SteeringBehaviorCalculations.Seek(_steeringBehaviorManager.Target, _steeringBehaviorManager.Entity);
        }

        //public override void Serialize(SteeringBehaviorData data)
        //{
        //}

        //public override void Deserialize(SteeringBehaviorData data)
        //{
        //}

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_steeringBehaviorManager.Entity.transform.position, 
                _steeringBehaviorManager.Target.ToVector3(_steeringBehaviorManager.Entity.transform.position.z));
        }
    }
}
