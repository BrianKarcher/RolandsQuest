using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Set Scene Variable")]
    public class SetSceneVariable : ActionBase
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

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            GameDataController.Instance.SetVariable(Model.Enums.VariableType.Scene, Variable, Value);
        }
    }
}
