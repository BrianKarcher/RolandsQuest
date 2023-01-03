using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Interfaces;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Sprite Float Variable")]
    public class SpriteFloatVariableConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private Operator _operator = Operator.Equal;
        [SerializeField]
        private FloatVariableEnum _variable = FloatVariableEnum.Altitude;
        [SerializeField]
        private float _value = 0f;
        [SerializeField]
        private bool _isValuePercent = false;
        //[SerializeField]
        //private float _value2Squared;

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    //_entity = entity as ISprite;
        //    _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
        //    _entityStatsComponent = entity.Components.GetComponent<EntityStatsComponent>();
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var value = GetValue(stateMachine);
            var testResult = Test(value);

            //if (testResult)
            //{
            //    string hi = "hi";
            //}

            return testResult;
        }

        private float GetValue(IStateMachine stateMachine)
        {
            IBasicPhysicsComponent physicsComponent = stateMachine.GetComponentRepository().Components.GetComponent<IBasicPhysicsComponent>();
            EntityStatsComponent entityStatsComponent = stateMachine.GetComponentRepository().Components.GetComponent<EntityStatsComponent>();
            switch(_variable)
            {
                case FloatVariableEnum.Altitude:
                    //return (physicsComponent.GetPhysicsData() as PhysicsData).Altitude.y;
                    return 0f;
                case FloatVariableEnum.HP:
                    if (_isValuePercent)
                    {
                        return entityStatsComponent.GetEntityStats().CurrentHP / entityStatsComponent.GetEntityStats().MaxHP;
                    }
                    else
                    {
                        return entityStatsComponent.GetEntityStats().CurrentHP;
                    }
                    
                case FloatVariableEnum.InputVelocity:
                    return physicsComponent.GetPhysicsData().InputVelocity.magnitude;
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
