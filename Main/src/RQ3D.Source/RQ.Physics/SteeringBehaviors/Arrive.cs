using RQ.Physics.SteeringBehaviors.RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Arrive : SteeringBehaviorBase, ISteeringBehavior
    {
        private Deceleration _deceleration;
        protected float _weight;

        public Arrive(SteeringBehaviorManager manager) : base(manager)
        {
            _weight = Constants.ArriveWeight;
            _constantWeight = Constants.ArriveWeight;
            _deceleration = Deceleration.normal;
        }

        //--------------------------- CalculateForce -----------------------------
        //
        //  This behavior is similar to seek but it attempts to arrive at the
        //  target with a zero velocity
        //------------------------------------------------------------------------
        protected override Vector2 CalculateForce()
        {
            return SteeringBehaviorCalculations.Arrive(_steeringBehaviorManager.Target,
                _steeringBehaviorManager.Entity, _deceleration);
        }

        public void SetDeceleration(Deceleration deceleration)
        {
            _deceleration = deceleration;
        }

        //public override void Serialize(SteeringBehaviorData data)
        //{
        //    data.Deceleration = _deceleration;
        //    data.Weight = _weight;
        //}

        //public override void Deserialize(SteeringBehaviorData data)
        //{
        //    _deceleration = data.Deceleration;
        //    _weight = data.Weight;
        //}
    }
}
