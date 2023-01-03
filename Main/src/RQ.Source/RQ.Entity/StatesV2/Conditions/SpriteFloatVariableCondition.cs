using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Sprite Float Variable")]
    public class SpriteFloatVariableCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        private AltitudePhysicsComponent _altitudePhysicsComponent;
        protected EntityStatsComponent _entityStatsComponent;
        [SerializeField]
        private Operator _operator = Operator.Equal;
        [SerializeField]
        private FloatVariableEnum _variable = FloatVariableEnum.Altitude;
        [SerializeField]
        private float _value = 0f;
        //[SerializeField]
        //private float _value2Squared;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            //_entity = entity as ISprite;
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            _entityStatsComponent = entity.Components.GetComponent<EntityStatsComponent>();
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            base.TestCondition(stateMachine);
            var value = GetValue();

            //float speed = _entity.GetVelocity().LengthSq();

            var testResult = Test(value);

            //if (testResult)
            //{
            //    string hi = "hi";
            //}

            return testResult;
        }

        private float GetValue()
        {
            switch(_variable)
            {
                case FloatVariableEnum.Altitude:
                    return (_altitudePhysicsComponent.GetAltitudeData()).Altitude.y;
                case FloatVariableEnum.HP:
                    return _entityStatsComponent.GetEntityStats().CurrentHP;
                case FloatVariableEnum.InputVelocity:
                    return _physicsComponent.GetPhysicsData().InputVelocity.magnitude;
            }
            return 0f;
        }

        private bool Test(float value)
        {
            switch (_operator)
            {
                case Operator.Equal:
                    return value == _value;
                case Operator.GreaterThen:
                    return value > _value;
                case Operator.GreaterThenOrEqualTo:
                    return value >= _value;
                case Operator.LessThanOrEqualTo:
                    return value <= _value;
                case Operator.LessThen:
                    return value < _value;
                case Operator.NotEqual:
                    return value != _value;
                case Operator.Between:
                    return value >= _value && value <= _value;
            }
            return false;
        }
    }
}
