namespace RQ.Physics.SteeringBehaviors
{
    public class Flee : SteeringBehaviorBase, ISteeringBehavior
    {
        public Flee(SteeringBehaviorManager manager)
            : base(manager)
        {
            _constantWeight = Constants.FleeWeight;
        }
        //this behavior returns a vector that moves the agent away
        //from a target position
        protected override Vector2D CalculateForce()
        {
            return SteeringBehaviorCalculations.Flee(_steeringBehaviorManager.Target, _steeringBehaviorManager.Entity);
        }
    }
}
