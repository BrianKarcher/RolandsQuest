using RQ.Physics.SteeringBehaviors.RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Interpose : SteeringBehaviorBase, ISteeringBehavior
    {
        public Interpose(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.InterposeWeight;
        }
        //this results in a steering force that attempts to steer the vehicle
        //to the center of the vector connecting two moving agents.
        protected override Vector2 CalculateForce()
        {
            return SteeringBehaviorCalculations.Interpose(_steeringBehaviorManager.TargetAgent1, 
                _steeringBehaviorManager.TargetAgent2, _steeringBehaviorManager.Entity);
        }
    }
}
