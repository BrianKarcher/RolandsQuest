using RQ.AI;
using RQ.Animation;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Entity.AtomAction
{
    [Serializable]
    public class SetVectorVariableAtom2 : AtomActionBase
    {
        //private PhysicsComponent _physicsComponent;
        //private EntityStatsComponent _entityStatsComponent;
        public Vector2 Value;
        //private BasicPhysicsData _physicsData;
        //private AltitudePhysicsComponent altitudePhysicsComponent;
        //private AltitudeData altitudeData;
        private AnimationComponent _animComponent;
        private AnimationComponent _targetAnimComponent;
        private PhysicsComponent _physicsComponent;
        private AIComponent _aiComponent;
        [SerializeField]
        public VectorVariableEnum _variable;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();

            var animationComponents = entity.Components.GetComponents<AnimationComponent>();

            if (animationComponents != null)
            {
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animComponent = aniamtionComponent;
                        break;
                    }
                }
            }
            //altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            //_physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            //_physicsData = _physicsComponent.GetPhysicsData();
            //_entityStatsComponent = entity.Components.GetComponent<EntityStatsComponent>();
            //altitudeData = altitudePhysicsComponent.GetAltitudeData();
            Tick();
        }

        public override AtomActionResults OnUpdate()
        {
            Tick();
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        private void Tick()
        {
            Value = GetValue();
        }

        private Vector2 GetValue()
        {
            Transform target;
            switch (_variable)
            {
                case VectorVariableEnum.FacingDirectionVector:
                    //if (_animComponent == null)
                    //{
                    //    _animComponent = _entity.Components.GetComponents<AnimationComponent>().Where(i => i.IsMain()).FirstOrDefault();
                    //}
                    return _animComponent.GetFacingDirectionVector();
                case VectorVariableEnum.TargetFacingDirectionVector:
                    if (_targetAnimComponent == null)
                    {
                        target = GetTarget();
                        var targetRepo = target.GetComponent<SpriteBaseComponent>();
                        var animComponents = targetRepo.Components.GetComponents<AnimationComponent>();
                        for (int i = 0; i < animComponents.Count; i++)
                        {
                            var animComponent = animComponents[i] as AnimationComponent;
                            if (animComponent.IsMain())
                            {
                                _targetAnimComponent = animComponent;
                                break;
                            }
                        }
                    }
                    //if (_animComponent == null)
                    //{
                    //    _animComponent = targetRepo.Components.GetComponents<AnimationComponent>().Where(i => i.IsMain()).FirstOrDefault();
                    //}
                    return _targetAnimComponent.GetFacingDirectionVector();
                case VectorVariableEnum.VectorToTarget:
                    target = GetTarget();
                    return target.transform.position - _entity.transform.position;
                case VectorVariableEnum.Position:
                    return _entity.transform.position;
                case VectorVariableEnum.DirectionToDamageDealer:
                    var damageComponent = _entity.Components.GetComponent<DamageComponent>();
                    var damageInfo = damageComponent.GetDamageInfo();
                    return damageInfo.DamagedByEntity.transform.position - _entity.transform.position;
                case VectorVariableEnum.Velocity:
                    return _physicsComponent.GetVelocity();
                case VectorVariableEnum.FootPosition:
                    return _physicsComponent.GetFeetWorldPosition();
                case VectorVariableEnum.InputVelocity:
                    return _physicsComponent.GetPhysicsData().InputVelocity;
            }
            return Vector2.zero;
        }

        private Transform GetTarget()
        {
            if (_aiComponent == null)
                _aiComponent = _entity.Components.GetComponent<AIComponent>();
            Transform target;
            if (_aiComponent != null)
            {
                target = _aiComponent.Target;
            }
            else
            {
                var physicsComponent = _entity.Components.GetComponent<PhysicsComponent>();
                target = (physicsComponent.GetSteering() as SteeringBehaviorManager).TargetAgent1.GetComponentRepository().transform;
            }
            return target;
        }
    }
}
