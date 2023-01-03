using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Conditional/Scene Variable")]
    public class SceneVariableConditional : ConditionalBase
    {
        [SerializeField]
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
            var currentValue = GameDataController.Instance.GetVariable(Model.Enums.VariableType.Scene, 
                Variable);
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
