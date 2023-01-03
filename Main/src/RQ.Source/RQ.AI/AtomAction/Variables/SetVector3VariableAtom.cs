using RQ.AI;
using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Input;
using RQ.Model.Enums;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    public enum Vector3VariableType
    {
        //SteeringOffset = 0,
        GetOffsetPursuitLocation = 1
    }

    [Serializable]
    public class SetVector3VariableAtom : AtomActionBase
    {
        //private EntityStatsComponent _entityStatsComponent;
        //private EntityStatsData _entityStatusData;
        //private InputComponent _inputComponent;
        public Vector3 Value;
        public ActionTarget ActionTarget = ActionTarget.Self;
        [SerializeField]
        public Vector3VariableType _variable;
        private SteeringBehaviorManager _steering;
        //public string VariableName;

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
            //_entityStatsComponent = target.Components.GetComponent<EntityStatsComponent>();            
            //_inputComponent = target.Components.GetComponent<InputComponent>();
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _steering = physicsComponent.GetSteering() as SteeringBehaviorManager;
        }

        public override AtomActionResults OnUpdate()
        {
            Value = GetValue();
            return AtomActionResults.Success;
        }

        private Vector3 GetValue()
        {
            //_entityStatusData = _entityStatsComponent?.GetEntityStats();
            //var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (_variable)
            {
                case Vector3VariableType.GetOffsetPursuitLocation:
                    return _steering.TargetAgent1.GetWorldPos2D() + (Vector2)_steering.Offset;
            }
            return Vector3.zero;
        }
    }
}
