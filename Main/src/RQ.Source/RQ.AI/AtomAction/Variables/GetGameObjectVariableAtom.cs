using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.FSM.V2;
using UnityEngine;
using RQ.Entity;

namespace RQ.AI.AtomAction.Variables
{
    [Serializable]
    public class GetGameObjectVariableAtom : AtomActionBase
    {
        //private EntityStatsComponent _entityStatsComponent;
        //private EntityStatsData _entityStatusData;
        //private InputComponent _inputComponent;
        private GameObject Value;
        //public ActionTarget ActionTarget = ActionTarget.Self;
        public GameObjectVariableEnum _variable;
        public string VariableName;

        private AIComponent _aiComponent;
        private PlayerComponent _playerComponent;
        //private Rigidbody _rigidBody;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_aiComponent == null)
                _aiComponent = _entity.Components.GetComponent<AIComponent>();
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();
            //_rigidBody = _entity.transform.GetComponent<Rigidbody>();
            //if (_rigidBody == null)
            //    _rigidBody = _entity.transform.GetComponentInParent<Rigidbody>();
        }

        public override AtomActionResults OnUpdate()
        {
            SetValue(Value);
            return AtomActionResults.Success;
        }

        private void SetValue(GameObject gameObject)
        {
            //var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (_variable)
            {
                case GameObjectVariableEnum.Parent:
                    _aiComponent.Parent = gameObject.transform;
                    break;
                case GameObjectVariableEnum.Target:
                    _aiComponent.Target = gameObject.transform;
                    break;
                case GameObjectVariableEnum.LiftingObject:
                    _playerComponent.SetLiftingObject(gameObject);
                    break;
            }
        }

        public void SetVariable(GameObject value)
        {
            Value = value;
        }
    }
}
