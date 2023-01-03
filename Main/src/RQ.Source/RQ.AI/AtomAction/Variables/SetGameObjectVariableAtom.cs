using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.Input;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.AtomAction.Variables
{
    [Serializable]
    public class SetGameObjectVariableAtom : AtomActionBase
    {
        //private EntityStatsComponent _entityStatsComponent;
        //private EntityStatsData _entityStatusData;
        //private InputComponent _inputComponent;
        public GameObject Value;
        //public ActionTarget ActionTarget = ActionTarget.Self;
        public GameObjectVariableEnum _variable;
        public string VariableName;
        private Rigidbody _rigidBody;
        private PlayerComponent _playerComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_rigidBody == null)
                _rigidBody = _entity.transform.GetComponent<Rigidbody>();
            if (_rigidBody == null)
                _rigidBody = _entity.transform.GetComponentInParent<Rigidbody>();
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();
        }

        public override AtomActionResults OnUpdate()
        {
            Value = GetValue();
            return AtomActionResults.Success;
        }

        private GameObject GetValue()
        {            
            switch (_variable)
            {
                case GameObjectVariableEnum.RigidBody:
                    return _rigidBody.gameObject;
                case GameObjectVariableEnum.LiftingObject:
                    return _playerComponent.GetLiftingObject();
            }
            return null;
        }
    }
}
