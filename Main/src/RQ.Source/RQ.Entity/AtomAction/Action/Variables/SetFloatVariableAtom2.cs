using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetFloatVariableAtom2 : AtomActionBase
    {
        private PhysicsComponent _physicsComponent;
        private EntityStatsComponent _entityStatsComponent;
        public float Value;
        private BasicPhysicsData _physicsData;
        private AltitudePhysicsComponent _altitudePhysicsComponent;
        private AIComponent _aiComponent;
        private AltitudeData altitudeData;
        [SerializeField]
        public FloatVariableEnum _variable;
        public string VariableName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_altitudePhysicsComponent == null)
                _altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _physicsData = _physicsComponent.GetPhysicsData();
            if (_entityStatsComponent == null)
                _entityStatsComponent = entity.Components.GetComponent<EntityStatsComponent>();
            if (_aiComponent == null)
                _aiComponent = entity.Components.GetComponent<AIComponent>();
            altitudeData = _altitudePhysicsComponent?.GetAltitudeData();            
        }

        public override AtomActionResults OnUpdate()
        {
            Value = GetValue();
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        private float GetValue()
        {
            var variableComponent = _entity.Components.GetComponent<VariablesComponent>();
            switch (_variable)
            {
                case FloatVariableEnum.Altitude:
                    return altitudeData.Altitude.y;
                case FloatVariableEnum.HP:
                    return _entityStatsComponent.GetEntityStats().CurrentHP;
                case FloatVariableEnum.InputVelocity:
                    return _physicsComponent.GetPhysicsData().InputVelocity.magnitude;
                    //return _physicsComponent.GetPhysicsAffector("Input").Force.magnitude;
                case FloatVariableEnum.DistanceSqToTarget:
                    if (_aiComponent.Target == null)
                        return 0f;
                    var target = _aiComponent.Target;
                    return (target.position - (Vector3)_physicsComponent.GetFeetWorldPosition()).sqrMagnitude;
                case FloatVariableEnum.DistanceSqToSteeringTarget:
                    var steering = _physicsComponent.GetSteering() as SteeringBehaviorManager;
                    if (steering.TargetAgent1 == null)
                        throw new Exception($"No Steering.TargetAgent1 in {_entity.name}");
                    return ((Vector2)steering.TargetAgent1.GetFeetWorldPosition3() - _physicsComponent.GetFeetWorldPosition()).sqrMagnitude;
                case FloatVariableEnum.DistanceSqToTargetOffset:
                    if (_aiComponent.Target == null)
                        return 0f;
                    var steeringManager = _physicsComponent.GetSteering() as SteeringBehaviorManager;
                    var targetPos = _aiComponent.Target.position + steeringManager.Offset;
                    return (targetPos - (Vector3)_physicsComponent.GetFeetWorldPosition()).sqrMagnitude;
                case FloatVariableEnum.FloatVariable:
                    return variableComponent.Variables.GetFloat(VariableName);
                case FloatVariableEnum.DistanceSqToSteeringVector:
                    //if (_aiComponent.Target == null)
                    //    return 0f;
                    //var target = _aiComponent.Target;
                    return (_physicsComponent.GetSteering().GetTarget3() - (Vector3)_physicsComponent.GetFeetWorldPosition()).sqrMagnitude;
                case FloatVariableEnum.AirVelocity:
                    return altitudeData.AirVelocity.y;
                case FloatVariableEnum.Gravity:
                    return altitudeData.Gravity.y;
                case FloatVariableEnum.Velocity:
                    return _physicsData.Velocity.magnitude;
                //return _physicsComponent.GetPreviousVelocity().magnitude;
                case FloatVariableEnum.PreviousVelocity:
                    return _physicsComponent.GetPreviousVelocity().magnitude;
            }
            return 0f;
        }
    }
}
