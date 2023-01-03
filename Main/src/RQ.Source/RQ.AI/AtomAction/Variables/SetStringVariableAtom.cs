using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2.Conditionals;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class GetStringVariableAtom : AtomActionBase
    {
        public string Value;
        [SerializeField]
        public StringVariableEnum _variable;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            Set();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void Set()
        {
            switch (_variable)
            {
                case StringVariableEnum.ModalText:
                    if (!String.IsNullOrEmpty(Value))
                        GameController.Instance.UIManager.SetModalText(Value);
                    break;
            }
        }
    }
}
