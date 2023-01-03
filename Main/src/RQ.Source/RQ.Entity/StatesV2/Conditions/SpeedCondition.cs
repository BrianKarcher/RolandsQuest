using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Speed")]
    public class SpeedCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        [SerializeField]
        private Operator Operator = Operator.Equal;
        [SerializeField]
        private float _valueSquared = 0f;
        [SerializeField]
        private float _value2Squared = 0f;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            //_entity = entity as ISprite;
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            float speedsq = _physicsComponent.GetVelocity().sqrMagnitude;
            switch (Operator)
            {
                case Operator.Equal:
                    if (speedsq != 0f)
                    {
                        int i = 1;
                    }
                    return speedsq == _valueSquared;
                case Operator.GreaterThen:
                    return speedsq > _valueSquared;
                case Operator.GreaterThenOrEqualTo:
                    return speedsq >= _valueSquared;
                case Operator.LessThanOrEqualTo:
                    return speedsq <= _valueSquared;
                case Operator.LessThen:
                    return speedsq < _valueSquared;
                case Operator.NotEqual:
                    return speedsq != _valueSquared;
                case Operator.Between:
                    return speedsq >= _valueSquared && speedsq <= _value2Squared;
            }
            return false;
        }
    }
}
