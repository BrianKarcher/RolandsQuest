using RQ.Common;
using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Conditional/Global Variable")]
    public class GlobalVariableConditional : ConditionalBase
    {
        [SerializeField]
        [VariableSelector]
        [HideInInspector]
        private string _variable;

        [SerializeField]
        [HideInInspector]
        private string _value;

        [HideInInspector]
        public string Variable { get { return _variable; } set { _variable = value; } }

        public string Value { get { return _value; } set { _value = value; } }

        public override bool Check()
        {
            var currentValue = GameDataController.Instance.Data.GlobalVariables[Variable].Value;
            switch (Operator)
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
