using RQ.AI;
using RQ.Common.Controllers;
using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Input;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetBoolVariableAtom2 : AtomActionBase
    {
        private EntityStatsComponent _entityStatsComponent;
        private EntityStatsData _entityStatusData;
        private InputComponent _inputComponent;
        private PlayerComponent _playerComponent;
        public bool Value;
        public bool DefaultValue = false;
        public bool InvertValue;
        public ActionTarget ActionTarget = ActionTarget.Self;
        [SerializeField]
        public BoolVariableEnum _variable;
        private string _variableName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            IComponentRepository target;
            switch (ActionTarget)
            {
                case ActionTarget.Self:
                    target = entity;
                    break;
                case ActionTarget.Target:
                    var targetingData = entity.Components.GetComponent<AIComponent>()?.Target;
                    target = targetingData.GetComponent<IComponentRepository>();
                    break;
                default:
                    target = null;
                    break;
            }
            if (target == null)
            {
                Debug.LogError("(GetBoolVariableAtom) - Could not locate target.");
                return;
            }
            if (_entityStatsComponent == null)
                _entityStatsComponent = target.Components.GetComponent<EntityStatsComponent>();     
            if (_inputComponent == null)
                _inputComponent = target.Components.GetComponent<InputComponent>();
            if (_playerComponent == null)
                _playerComponent = target.Components.GetComponent<PlayerComponent>();
        }

        public override AtomActionResults OnUpdate()
        {
            Value = GetValue();
            if (InvertValue)
                Value = !Value;
            return AtomActionResults.Success;
        }

        private bool GetValue()
        {
            _entityStatusData = _entityStatsComponent?.GetEntityStats();
            //var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (_variable)
            {
                case BoolVariableEnum.IsHiding:
                    return _entityStatusData.IsHiding;
                case BoolVariableEnum.IsInputEnabled:
                    return _inputComponent.IsInputEnabled();
                case BoolVariableEnum.IsAutoStart:
                    return GameStateController.Instance.AutoStart;
                case BoolVariableEnum.GlobalBoolVariable:
                    if (String.IsNullOrEmpty(_variableName))
                        return DefaultValue;
                    if (GameDataController.Instance.Data.GlobalVariables.TryGetValue(_variableName, out var variable))
                        return variable.Value == "0" ? false : true;
                    else
                        return DefaultValue;
                //return variableComponent.Variables.GetBool(VariableName);
                //case BoolVariableEnum.IsPushing:
                //    return _playerComponent.GetIsPushing();
                case BoolVariableEnum.IsCrafted:
                    return GameStateController.Instance.IsCrafted;
            }
            return DefaultValue;
        }

        public void SetVariableName(string variableName)
        {
            _variableName = variableName;
        }
    }
}
