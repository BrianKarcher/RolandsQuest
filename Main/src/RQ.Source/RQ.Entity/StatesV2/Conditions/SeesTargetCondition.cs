using System;
using RQ.Animation;
using RQ.Common;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics;
using RQ.Physics.Components;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Sees Target")]
    public class SeesTargetCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        protected AnimationComponent _animationComponent;
        protected AIComponent _aiComponent;
        private CollisionComponent _collisionComponent;
        [SerializeField]
        private bool _castRay = false;
        //[SerializeField]
        //private int _layerMask;

        [SerializeField]
        private LayerMask _obstacleLayerMask = 0;

        /// <summary>
        /// Excluded tags can include things like the tilemap if you want to check the raycast between
        /// sprites and want the tilemap to stop the raycast.
        /// </summary>
        //[SerializeField]
        //[Tag]
        //private List<string> _tagsThatCancelRaycast = null;

        //public int LayerMask { get { return _layerMask; } set { _layerMask = value; } }
        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            //var spriteBase = entity.GetComponent<IComponentRepository>();
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            foreach (var baseObject in animationComponents)
            {
                var animationComponent = baseObject as AnimationComponent;
                if (animationComponent.IsMain())
                {
                    _animationComponent = animationComponent;
                    break;
                }
            }
            //_animationComponent = entity.Components.GetComponents<AnimationComponent>().FirstOrDefault(i => i.IsMain());
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            //_entity = entity as ISprite;
            //_behavior = _entity.GetSteering().GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            //var tileMap = GameObject.FindObjectOfType<tk2dTileMap>();

            //var layers = tileMap.Layers[0];
            //tileMap.GetTileIdAtPosition(transform.position, );

            //var isPositive = false;

            //var facing = _animationComponent.GetFacingDirectionVector();

            // Look At care more about the facing direction than the heading.
            var facingDirection = _animationComponent.GetFacingDirectionVector();
            //var heading = _physicsComponent.GetPhysicsData().Heading;

            var vectorToTarget = _aiComponent.Target.position - _physicsComponent.GetWorldPos();

            var angle = Vector2.Angle(facingDirection, vectorToTarget);

            // TODO Add a "sniff test" condition to tell if the target is close, regardless of angle

            // Within field of view?  Worthy of further consideration
            if (angle > _physicsComponent.FieldOfView)
                return false;

            // Using distance squared space because a square root is slow
            if (Vector2D.Vec2DDistanceSq(_physicsComponent.GetWorldPos(), _aiComponent.Target.position) >= _physicsComponent.LineOfSight * _physicsComponent.LineOfSight)
                return false;

            // TODO Cast a ray to check for a clear line of sight
            if (!_castRay)
                return true;

            var otherBaseComponent = _aiComponent.Target.GetComponent<SpriteBaseComponent>();

            var hit = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId, _obstacleLayerMask, out var raycastHit);

            //bool hasExcludedColliders = false;
            //foreach (var raycastObject in raycastObjects)
            //{
            //    foreach (var tagThatCancelsRaycast in _tagsThatCancelRaycast)
            //    {
            //        if (tagThatCancelsRaycast == raycastObject.collider.tag)
            //        {
            //            hasExcludedColliders = true;
            //            break;
            //        }
            //    }
            //    if (hasExcludedColliders)
            //        break;
            //    //var hasTagThatCancelsRaycast = Array.IndexOf(_tagsThatCancelRaycast, i.collider.tag) > -1;
            //}
            //var hasExcludedColliders = raycastObjects.Any(i => _tagsThatCancelRaycast.Contains(i.collider.tag));

            //return !hasExcludedColliders;
            return hit;
        }
    }
}
