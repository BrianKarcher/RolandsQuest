using RQ.Animation;
using RQ.Animation.V2;
using RQ.AnimationV2;
using RQ.Common;
using RQ.Controller.SequencerEvents;
using RQ.Entity.Components;
using RQ.Extensions;
using RQ.FSM.V2.States;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Pathfinding;
using RQ.Physics.SteeringBehaviors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [Obsolete]
    [AddComponentMenu("RQ/Action/Physics/Follow Path Request")]
    public class FollowPathRequest : ActionBase, IAnimatedObject
    {
        [SerializeField]
        private Transform _waypoints = null;

        //[SerializeField]
        //private bool _looped = false;

        [SerializeField]
        private PathType _pathType = PathType.Pingpong;

        [SerializeField]
        private PathWalkingDirection _pathWalkingDirection = PathWalkingDirection.Forwards;

        [SerializeField]
        private DirectionMode _facingDirectionMode = DirectionMode.Automatic;
        
        [SerializeField]
        private Direction _facingDirection = Direction.Right;

        private RQ.Physics.SteeringBehaviors.FollowPath _behavior;
        private List<Vector3> _path;
        [SerializeField]
        protected AnimationComponent _animationComponent;

        [SerializeField]
        [AnimationTypeAttribute]
        private string _startAnimation = null;

        [SerializeField]
        [AnimationTypeAttribute]
        private string _endAnimation = null;
        //private ISprite _sprite;

        //public 

        public override void InitAction()
        {
            base.InitAction();
            //_sprite = EntityUIBase.GetEntity(entity);
            //if (_sprite == null)
            //    throw new Exception("FSM - Sprite not set.");
            if (_animationComponent == null)
            {
                var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }

                //var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                //if (animationComponents != null)
                //{
                //    _animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                //}
            }

            _behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
            //var transforms = _waypoints.GetPositionsInChildrenOneDeep();
            _waypoints.GetPositionsInChildrenOneDeep(_path);
            //_path = new Vector2D[transforms.Length];
            //for (int i = 0; i < transforms.Length; i++)
            //{
            //    _path[i] = transforms[i].position;
            //}
        }


        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (_waypoints != null)
            {
                if (_aiComponent == null)
                    Debug.Log("Cannot set Waypoints, AI Component not found");
                else
                    MessageDispatcher2.Instance.DispatchMsg("SetWaypoints", 0f, this.UniqueId, _aiComponent.UniqueId, _waypoints);
            }

            MessageDispatcher2.Instance.DispatchMsg("FollowPath", 0f, this.UniqueId, GetEntity().UniqueId, null);

            if (_facingDirectionMode == DirectionMode.Manual)
            {
                _animationComponent.SetFacingDirection(_facingDirection);
                _animationComponent.ProcessDirectionChange(_facingDirection);
                _animationComponent.GetSpriteAnimator().Render(_facingDirection);
            }

            if (!String.IsNullOrEmpty(_startAnimation))
            {
                _animationComponent.Animate(_startAnimation);
            }
        }

        //private void WaypointChanged()
        //{
        //    if (_facingDirectionMode == DirectionMode.Automatic)
        //    {
        //        var newVelocity = _behavior.Path.CurrentWaypoint() - _physicsComponent.GetPos();
        //        Debug.Log("FollowPath, (" + GetEntity().name + ") Velocity changed to " + newVelocity.ToString());
        //        SendMessageToSpriteBase(0f, Telegrams.VelocityChanged, newVelocity);
        //    }
        //}

        //public override void FixedUpdate()
        //{
        //    if (!_isRunning)
        //        return;

        //    base.FixedUpdate();
        //    if (_behavior.Path.IsFinished)
        //    {
        //        _behavior.TurnOff();
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
        //            Telegrams.StopMovement, null);
        //        _isRunning = false;
        //        if (!String.IsNullOrEmpty(_endAnimation))
        //        {
        //            _animationComponent.Animate(_endAnimation);
        //        }
        //        //_physicsComponent.Stop();
        //    }
        //}

        //public override void ActExit(Component otherRigidBody)
        //{
        //    base.ActExit(otherRigidBody);
        //    _behavior.TurnOff();
        //    if (!String.IsNullOrEmpty(_endAnimation))
        //    {
        //        _animationComponent.Animate(_endAnimation);
        //    }
        //    //sprite.GetSteering().TurnOff(behavior_type.wander);
        //}

        public AnimationComponent GetAnimationComponent()
        {
            return _animationComponent;
        }

        public RQ.AnimationV2.ISpriteRenderer GetSpriteRenderer()
        {
            if (_animationComponent == null)
            {
                var entity = GetRepo();
                if (entity == null)
                    return null;

                var animationComponents = entity.Components.GetComponents<AnimationComponent>();

                if (animationComponents == null)
                    return null;
                //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }
                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain() == true);
            }

            //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
            if (_animationComponent == null)
                return null;

            return _animationComponent.GetSpriteAnimator();
        }

        private IComponentRepository GetRepo()
        {
            var entity = GetEntity();
            if (entity == null)
            {
                var runActionsState = GetComponent<RunActionsState>();
                //var stateMachine = runActionsState.StateMachine;
                if (runActionsState != null)
                    entity = runActionsState.GetEntity();

                //Debug.Log("Could not locate entity");
                //return null;
            }
            if (entity == null)
            {
                var playActionEvent = GetComponent<IPlayActionsEvent>();
                if (playActionEvent != null)
                    entity = playActionEvent.AffectedObject.GetComponent<IComponentRepository>();
            }
            return entity;
        }

        public List<SpriteAnimationType> GetStoredSpriteAnimations()
        {
            if (_animationComponent == null)
            {
                var entity = GetRepo();

                var animationComponents = entity.Components.GetComponents<AnimationComponent>();

                if (animationComponents == null)
                    return null;

                //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }

                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain() == true);
            }

            //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
            if (_animationComponent == null)
                return null;

            return _animationComponent.GetSpriteAnimator().GetStoredSpriteAnimations();
        }
    }
}
