using RQ.Physics.SteeringBehaviors.RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Evade : SteeringBehaviorBase, ISteeringBehavior
    {
        public Evade(SteeringBehaviorManager manager) : base(manager)
        {
            _constantWeight = Constants.EvadeWeight;
        }

        //----------------------------- Evade ------------------------------------
        //
        //  similar to pursuit except the agent Flees from the estimated future
        //  position of the pursuer
        //------------------------------------------------------------------------
        protected override Vector2 CalculateForce()
        {
            return SteeringBehaviorCalculations.Evade(_steeringBehaviorManager.TargetAgent1, _steeringBehaviorManager.Entity);
        }
    }
}
