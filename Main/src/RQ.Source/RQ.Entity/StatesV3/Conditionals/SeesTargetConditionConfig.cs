using RQ.Animation;
using RQ.Common;
using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics;
using RQ.Physics.Components;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Sees Target")]
    public class SeesTargetConditionConfig : StateTransitionConditionBaseConfig
    {
        //protected PhysicsComponent _physicsComponent;
        //protected AnimationComponent _animationComponent;
        //protected AIComponent _aiComponent;
        //private CollisionComponent _collisionComponent;
        [SerializeField]
        private bool _castRay = false;
        //[SerializeField]
        //private int _layerMask;

        [SerializeField]
        private LayerMask _layerMask = 0;

        /// <summary>
        /// Excluded tags can include things like the tilemap if you want to check the raycast between
        /// sprites and want the tilemap to stop the raycast.
        /// </summary>
        //[SerializeField]
        //[Tag]
        //private List<string> _tagsThatCancelRaycast = null;

        //public int LayerMask { get { return _layerMask; } set { _layerMask = value; } }
        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    //var spriteBase = entity.GetComponent<IComponentRepository>();

        //    //_entity = entity as ISprite;
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetComponentRepository();
            var _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var _animationComponent = entity.Components.GetComponent<AnimationComponent>();
            var _aiComponent = entity.Components.GetComponent<AIComponent>();
            var _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
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
            if (angle > _physicsComponent.FieldOfView / 2)
                return false;

            // Using distance squared space because a square root is slow
            if (Vector2D.Vec2DDistanceSq(_physicsComponent.GetWorldPos(), _aiComponent.Target.position) >= _physicsComponent.LineOfSight * _physicsComponent.LineOfSight)
                return false;

            // TODO Cast a ray to check for a clear line of sight
            if (!_castRay)
                return true;

            var otherBaseComponent = _aiComponent.Target.GetComponent<SpriteBaseComponent>();

            // TODO MAJOR PRIORITY
            // On test, this picked up the Shield Collider which is wrong position. Move the Raycast Check function
            // to the Physics Component so we can use either the foot position or center of body mass
            var raycastHit = _aiComponent.RaycastCheck(otherBaseComponent.UniqueId, _layerMask);

            //var hasExcludedColliders = raycastObjects.Any(i => _tagsThatCancelRaycast.Contains(i.collider.tag));

            //if (!hasExcludedColliders)
            //{
            //    int i = 1;
            //}
            //return !hasExcludedColliders;
            return !raycastHit;


            //UnityEngine.Physics.Raycast
            //var raycastHit = UnityEngine.Physics2D.Raycast(_physicsComponent.GetPos(), facing, _physicsComponent.LineOfSight, _layerMask);
            //if (raycastHit)
            //{
            //if (raycastHit.rigidbody == null)
            //    return false;

            //if (raycastHit.collider == null)
            //    return false;

            //var sprite = raycastHit.rigidbody.GetComponent<EntityUIBase>();
            //var sprite = raycastHit.collider.attachedRigidbody.GetComponent<EntityUIBase>();
            //if (sprite != null)
            //{
            //if (sprite.GetInstanceID() == _entity.Target.GetInstanceID())
            //if (raycastHit.collider.tag == _aiComponent.Target.tag)
            //{
            //    return true;
            //}
            //}
            //}

            //var isFinished = _behavior.Path.IsFinished;

            //if (isFinished)
            //{
            //    string hi = "hi8";
            //}

            //return false;

            //return _entity.IsAnimationComplete;
        }
    }
}
