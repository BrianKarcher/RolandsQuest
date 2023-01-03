using RQ.Animation;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AI.Action
{
    public enum LosTargetEnum
    {
        Target = 0,
        SteeringTargetOffset = 1
    }

    /// <summary>
    /// Checks for unobstructed line between entity and target
    /// </summary>
    public class HasLOSAtom : AtomActionBase
    {
        public LosTargetEnum Target;

        protected PhysicsComponent _physicsComponent;
        protected AnimationComponent _animationComponent;
        protected AIComponent _aiComponent;
        private CollisionComponent _collisionComponent;
        private ISteeringBehaviorManager _steeringBehaviorManager;

        // TODO Allow choosing of target entity that is not the AI target

        //public bool _castRay = false;
        //[SerializeField]
        //private int _layerMask;

        //[SerializeField]
        public int _obstacleLayerMask = 0;

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
            _steeringBehaviorManager = _physicsComponent.GetSteering();
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
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>("Collider");
            
            if (_collisionComponent == null)
                _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _obstacleLayerMask |= LayerMask.NameToLayer("Environment");
            //_entity = entity as ISprite;
            //_behavior = _entity.GetSteering().GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
            //Tick();
        }

        public override AtomActionResults OnUpdate()
        {
            return Tick() ? AtomActionResults.Success : AtomActionResults.Failure;
        }

        private bool Tick()
        {
            //var tileMap = GameObject.FindObjectOfType<tk2dTileMap>();

            //var layers = tileMap.Layers[0];
            //tileMap.GetTileIdAtPosition(transform.position, );

            //var isPositive = false;

            //var facing = _animationComponent.GetFacingDirectionVector();

            // Look At care more about the facing direction than the heading.
            //var facingDirection = _animationComponent.GetFacingDirectionVector();
            //var heading = _physicsComponent.GetPhysicsData().Heading;
            if (_aiComponent == null)
                throw new System.Exception("Could not locate AI Component.");
            //var vectorToTarget = _aiComponent.Target.position - _physicsComponent.GetWorldPos();

            //var angle = Vector2.Angle(facingDirection, vectorToTarget);

            // TODO Add a "sniff test" condition to tell if the target is close, regardless of angle

            // Within field of view?  Worthy of further consideration
            //if (angle > _physicsComponent.FieldOfView)
            //    return false;

            // Using distance squared space because a square root is slow
            //if ((_physicsComponent.GetWorldPos() - _aiComponent.Target.position).sqrMagnitude >= _physicsComponent.LineOfSight * _physicsComponent.LineOfSight)
            //    return false;

            // TODO Cast a ray to check for a clear line of sight
            //if (!_castRay)
            //    return true;

            //var otherBaseComponent = _aiComponent.Target.GetComponent<SpriteBaseComponent>();

            var target = GetTargetPos();

            //var raycastObjects = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId);

            return HasLineOfSight(target);

            //var hasExcludedColliders = raycastObjects.Any(i => _tagsThatCancelRaycast.Contains(i.collider.tag));

            //return !hasExcludedColliders;
        }

        public Vector3 GetTargetPos()
        {
            switch (Target)
            {
                case LosTargetEnum.Target:
                    return _aiComponent.Target.transform.position;
                case LosTargetEnum.SteeringTargetOffset:
                    return _steeringBehaviorManager.GetTargetAgent1().GetWorldPos() + _steeringBehaviorManager.GetOffset();
            }
            return Vector3.zero;
        }

        public bool HasLineOfSight(Vector3 target)
        {
            var currentPos = (Vector3)_entity.transform.position;
            //var rayCasts = UnityEngine.Physics.RaycastAll(currentPos, target - currentPos, (target - currentPos).magnitude, _obstacleLayerMask);
            //var layerMask = 1 << _obstacleLayerMask;
            return (!UnityEngine.Physics.Raycast(currentPos, target - currentPos, (target - currentPos).magnitude, _obstacleLayerMask));

        }
    }
}
