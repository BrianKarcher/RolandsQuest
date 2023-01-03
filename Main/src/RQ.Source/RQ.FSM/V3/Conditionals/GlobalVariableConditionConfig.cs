using RQ.Common;
using RQ.Common.Controllers;
using RQ.Controller.Actions.Conditionals;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Global Variable")]
    public class GlobalVariableConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        [VariableSelector]
        //[HideInInspector]
        private string _variable;

        [SerializeField]
        //[HideInInspector]
        private OperatorEnum _operator = OperatorEnum.Equal;

        [SerializeField]
        //[HideInInspector]
        private string _value;

        public string Variable { get { return _variable; } set { _variable = value; } }

        public string Value { get { return _value; } set { _value = value; } }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var currentValue = GameDataController.Instance.Data.GlobalVariables[Variable].Value;
            switch (_operator)
            {
                case OperatorEnum.Equal:
                    return Value == currentValue;
                case OperatorEnum.NotEqual:
                    return Value != currentValue;
            }
            return false;
        }
    }
}
