using RQ.Common;
using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Set Global Variable")]
    public class SetGlobalVariable : ActionBase
    {
        [SerializeField]
        [VariableSelector]
        //[HideInInspector]
        private string _variable;

        [SerializeField]
        //[HideInInspector]
        private string _value;

        public string Variable { get { return _variable; } set { _variable = value; } }

        public string Value { get { return _value; } set { _value = value; } }

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //GameController._instance._gameData.Variables
            GameDataController.Instance.Data.GlobalVariables[Variable].Value = Value;
        }
    }
}
