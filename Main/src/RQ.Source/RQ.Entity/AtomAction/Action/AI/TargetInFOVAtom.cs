using RQ.Animation;
using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Physics;
using RQ.Physics.Components;
using System.Collections.Generic;
using RQ.Model.Enums;
using UnityEngine;

namespace RQ.AI.Action
{
    public class TargetInFOVAtom : AtomActionBase
    {
        protected PhysicsComponent _physicsComponent;
        protected AnimationComponent _animationComponent;
        protected AIComponent _aiComponent;
        private CollisionComponent _collisionComponent;
        public DirectionToCheck _directionToCheck;
        public bool _castRay = false;
        //[SerializeField]
        //private int _layerMask;

        //[SerializeField]
        public int _obstacleLayerMask = -1;

        /// <summary>
        /// Excluded tags can include things like the tilemap if you want to check the raycast between
        /// sprites and want the tilemap to stop the raycast.
        /// </summary>
        //[SerializeField]
        //[Tag]
        public List<string> _tagsThatCancelRaycast = null;

        //public int LayerMask { get { return _layerMask; } set { _layerMask = value; } }
        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //var spriteBase = entity.GetComponent<IComponentRepository>();
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animationComponents.Count; i++)
            {
                var aniamtionComponent = animationComponents[i] as AnimationComponent;
                if (aniamtionComponent.IsMain())
                {
                    _animationComponent = aniamtionComponent;
                    break;
                }
            }
            //_animationComponent = entity.Components.GetComponents<AnimationComponent>().FirstOrDefault(i => i.IsMain());
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>("Collider");
            if (_collisionComponent == null)
                _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _obstacleLayerMask = LayerMask.NameToLayer("Environment");
            //_entity = entity as ISprite;
            //_behavior = _entity.GetSteering().GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        }

        public override AtomActionResults OnUpdate()
        {
            return Tick() ? AtomActionResults.Success : AtomActionResults.Running;
        }

        private bool Tick()
        {
            // Look At care more about the facing direction than the heading.
            Vector2D heading;
            if (_directionToCheck == DirectionToCheck.FacingDirection)
                heading = _animationComponent.GetFacingDirectionVector();
            else
                heading = _physicsComponent.GetPhysicsData().Heading;

            //var facingDirection = _animationComponent.GetFacingDirectionVector();
            //var heading = _physicsComponent.GetPhysicsData().Heading;
            if (_aiComponent == null)
                throw new System.Exception("Could not locate AI Component.");
            if (_aiComponent.Target == null)
                return false;

            return _physicsComponent.EntityInFOV(_aiComponent.Target.gameObject, heading, _castRay, _obstacleLayerMask);

            //var vectorToTarget = _aiComponent.Target.position - _physicsComponent.GetWorldPos();

            //var angle = Vector2.Angle(heading, vectorToTarget);

            //// TODO Add a "sniff test" condition to tell if the target is close, regardless of angle

            //// Within field of view?  Worthy of further consideration
            //if (angle > _physicsComponent.FieldOfView)
            //    return false;

            //// Using distance squared space because a square root is slow
            //if ((_physicsComponent.GetWorldPos() - _aiComponent.Target.position).sqrMagnitude >= _physicsComponent.LineOfSight * _physicsComponent.LineOfSight)
            //    return false;

            //// TODO Cast a ray to check for a clear line of sight
            //if (!_castRay)
            //    return true;

            //var otherBaseComponent = _aiComponent.Target.GetComponent<SpriteBaseComponent>();

            //var target = otherBaseComponent.transform.position;

            ////var raycastObjects = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId);

            //return HasLineOfSight(target);

            //var hasExcludedColliders = raycastObjects.Any(i => _tagsThatCancelRaycast.Contains(i.collider.tag));

            //return !hasExcludedColliders;
        }

        //public bool HasLineOfSight(Vector3 target)
        //{
        //    var currentPos = (Vector3)_entity.transform.position;
        //    //var layerMask = 1 << _obstacleLayerMask;
        //    return (!UnityEngine.Physics.Raycast(currentPos, target - currentPos, (target - currentPos).magnitude, _obstacleLayerMask));

        //}
    }
}
