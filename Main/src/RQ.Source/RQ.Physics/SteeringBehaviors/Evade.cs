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
        protected override Vector2D CalculateForce()
        {
            return SteeringBehaviorCalculations.Evade(_steeringBehaviorManager.TargetAgent1, _steeringBehaviorManager.Entity);
        }
    }
}
