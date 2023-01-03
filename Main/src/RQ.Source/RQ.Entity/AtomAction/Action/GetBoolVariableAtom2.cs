using RQ.AI;
using RQ.Common.Controllers;
using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using RQ.Model;
using RQ.Model.UI;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class GetBoolVariableAtom2 : AtomActionBase
    {
        private EntityStatsComponent _entityStatsComponent;
        private EntityStatsData _entityStatusData;
        private PhysicsComponent _physicsComponent;
        public bool Value;
        public bool RevertOnExit = false;
        public ActionTarget ActionTarget = ActionTarget.Self;
        [SerializeField]
        public BoolVariableEnum VariableToSet;
        private IComponentRepository target;
        private string _variableName;
        private bool _originalValue;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);

            target = base.GetTarget(ActionTarget);
            if (target == null)
            {
                Debug.LogError($"(GetBoolVariableAtom) - Could not locate target in {ActionTarget} for {VariableToSet}.");
                return;
            }
            if (_entityStatsComponent == null)
                _entityStatsComponent = target.Components.GetComponent<EntityStatsComponent>();
            _entityStatusData = _entityStatsComponent?.GetEntityStats();
            if (_physicsComponent == null)
                _physicsComponent = target.Components.GetComponent<PhysicsComponent>();
            SetValue(Value);
        }

        public override void End()
        {
            base.End();
            if (RevertOnExit)
                SetValue(_originalValue);
        }

        public override AtomActionResults OnUpdate()
        {
            //Value = GetValue();
            //if (InvertValue)
            //    Value = !Value;
            return AtomActionResults.Success;
        }

        private void SetValue(bool value)
        {
            var playerComponent = target.Components.GetComponent<PlayerComponent>();
            //var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (VariableToSet)
            {
                case BoolVariableEnum.IsHiding:
                    _entityStatusData.IsHiding = value;
                    break;
                case BoolVariableEnum.Invincible:
                    var damageComponent = target.Components.GetComponent<DamageComponent>();
                    var damageData = damageComponent.GetDamageData();
                    damageData.Vulnerable = !value;
                    break;
                //case BoolVariableEnum.MoveOnInput:                    
                //    if (playerComponent == null)
                //        return;
                //    playerComponent.SetMoveOnInput(Value);
                //    break;
                case BoolVariableEnum.GlobalBoolVariable:
                    if (String.IsNullOrEmpty(_variableName))
                        return;
                    if (!GameDataController.Instance.Data.GlobalVariables.TryGetValue(_variableName, out var variable))
                    {
                        variable = new Variable();
                        variable.Name = _variableName;
                        GameDataController.Instance.Data.GlobalVariables.Add(_variableName, variable);
                    }
                    variable.Value = value ? "1" : "0";
                    //if (variableComponent == null)
                    //    return;
                    //variableComponent.Variables.SetBool(VariableName, Value);
                    break;
                case BoolVariableEnum.SetFacingDirectionOnInput:
                    if (playerComponent == null)
                        return;
                    playerComponent.SetFacingDirectionOnInput(value);
                    break;
                case BoolVariableEnum.IsKinematic:
                    _physicsComponent.SetKinematic(value);
                    break;
            }
        }

        private bool GetValue()
        {
            var playerComponent = target.Components.GetComponent<PlayerComponent>();
            //var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (VariableToSet)
            {
                case BoolVariableEnum.IsHiding:
                    return _entityStatusData.IsHiding;
                case BoolVariableEnum.Invincible:
                    var damageComponent = target.Components.GetComponent<DamageComponent>();
                    var damageData = damageComponent.GetDamageData();
                    return !damageData.Vulnerable;
                //case BoolVariableEnum.MoveOnInput:                    
                //    if (playerComponent == null)
                //        return;
                //    playerComponent.SetMoveOnInput(Value);
                //    break;
                case BoolVariableEnum.GlobalBoolVariable:
                    if (String.IsNullOrEmpty(_variableName))
                        return false;
                    if (!GameDataController.Instance.Data.GlobalVariables.TryGetValue(_variableName, out var variable))
                    {
                        variable = new Variable();
                        variable.Name = _variableName;
                        GameDataController.Instance.Data.GlobalVariables.Add(_variableName, variable);
                    }
                    return variable.Value == "1";
                    //if (variableComponent == null)
                    //    return;
                    //variableComponent.Variables.SetBool(VariableName, Value);
                case BoolVariableEnum.SetFacingDirectionOnInput:
                    if (playerComponent == null)
                        return false;
                    return playerComponent.GetSetFacingDirectionOnInput();
                case BoolVariableEnum.IsKinematic:
                    return _physicsComponent.GetKinematic();
            }
            return false;
        }

        public void SetVariableName(string variableName)
        {
            _variableName = variableName;
        }
    }
}
