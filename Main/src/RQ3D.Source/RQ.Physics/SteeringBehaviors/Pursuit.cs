using RQ.Physics.SteeringBehaviors.RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Pursuit : SteeringBehaviorBase, ISteeringBehavior
    {
        public Pursuit(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.PursuitWeight;
        }

        //this behavior predicts where an agent will be in time T and seeks
        //towards that point to intercept it.
        protected override Vector2 CalculateForce()
        {
            return SteeringBehaviorCalculations.Pursuit(_steeringBehaviorManager.TargetAgent1, _steeringBehaviorManager.Entity);
        }
    }
}
