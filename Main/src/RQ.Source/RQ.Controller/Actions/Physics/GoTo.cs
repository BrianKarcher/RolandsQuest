﻿using RQ.Animation;
using RQ.Animation.V2;
using RQ.AnimationV2;
using RQ.Common;
using RQ.Common.Components;
using RQ.Controller.SequencerEvents;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.FSM.V2.States;
using RQ.Messaging;
using RQ.Physics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Go To")]
    [Obsolete("This can potentially run forever and cause hard to find bugs, use GoToRequest instead.")]
    public class GoTo : ActionBase, IAnimatedObject
    {
        private Location _targetLocation;
        //private Vector2D _targetPosition;

        [SerializeField]
        private GoToType _goToType = GoToType.Target;

        [SerializeField]
        private bool _immediate = false;

        [SerializeField]
        private Transform _waypoint = null;

        [SerializeField]
        private bool _useEntityWaypoints = false;

        [SerializeField]
        private Vector2D _velocity;

        //[SerializeField]
        //private bool _lookToTarget = false;

        //[SerializeField]
        //private bool _playAnimation = true;

        //private Vector2D _feetPosition;

        [SerializeField]
        protected AnimationComponent _animationComponent;

        [SerializeField]
        [AnimationTypeAttribute]
        private string _startAnimation = null;

        [SerializeField]
        [AnimationTypeAttribute]
        private string _endAnimation = null;

        private Transform _currentWaypoint = null;

        //public override void StartListening()
        //{
        //    base.StartListening();
        //    MessageDispatcher2.Instance.StartListening("SetFeetPosition", this.UniqueId, (data) =>
        //    {
        //        _feetPosition = (Vector2D)data.ExtraInfo;
        //    });
        //}

        //public override void StopListening()
        //{
        //    base.StopListening();
        //    MessageDispatcher2.Instance.StopListening("SetFeetPosition", this.UniqueId, -1);
        //}

        public override void InitAction()
        {
            base.InitAction();
            if (_animationComponent == null)
            {
                var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                if (animationComponents != null)
                {
                    for (int i = 0; i < animationComponents.Count; i++)
                    {
                        var animationComponent = animationComponents[i] as AnimationComponent;
                        if (animationComponent == null)
                            continue;
                        if (animationComponent.IsMain())
                        {
                            _animationComponent = animationComponent;
                            break;
                        }
                    }
                    //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                }
            }
        }

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //if (_sprite == null)
            //    return;
            //base.Act(otherRigidBody);
            if (_useEntityWaypoints)
            {
                _currentWaypoint = _aiComponent.GetWaypoints();
            }
            else
            {
                _currentWaypoint = _waypoint;
            }
            if (_steering == null)
                return;

            var goToVector = GetTargetLocation();
            if (_immediate)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Telegrams.SetPos, goToVector);
                Complete();
            }
            else
            {
                _steering.Target = goToVector;
                _steering.TurnOn(behavior_type.seek);
            }
            if (!String.IsNullOrEmpty(_startAnimation))
            {
                _animationComponent.Animate(_startAnimation);
            }
        }

        public Vector2D GetTargetLocation()
        {
            var feetPos = _physicsComponent.GetFeetWorldPosition();
            switch (_goToType)
            {
                case GoToType.Target:
                    _targetLocation = null;
                    return _aiComponent.Target.transform.position;
                case GoToType.RandomLocation:
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                        Telegrams.ChooseRandomLocation, feetPos, (location) =>
                        {
                            _targetLocation = (Location)location;
                        });
                    return _targetLocation.transform.position;
                case GoToType.RandomSubLocation:
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                        Telegrams.ChooseRandomSubLocation, feetPos, (location) =>
                        {
                            _targetLocation = (Location)location;
                        });
                    return _targetLocation.transform.position;
                case GoToType.Random:
                    _targetLocation = null;
                    throw new NotImplementedException("Random not implemented in GoTo");
                case GoToType.CurrentDestinationLocation:
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                        Telegrams.GetCurrentDestinationLocation, null, (location) =>
                        {
                            _targetLocation = (Location)location;
                        });
                    return _targetLocation.transform.position;
                case GoToType.Waypoint:
                    return _currentWaypoint.transform.position;
                case GoToType.CustomVelocity:
                    return _velocity;
            }
            return Vector2D.Zero();
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);

            if (_targetLocation != null)
            {
                foreach (var animationComponent in _animationComponents)
                {
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, animationComponent.UniqueId,
                        Telegrams.SetFacingDirection, _targetLocation.Direction);
                }
            }
            if (!String.IsNullOrEmpty(_endAnimation))
            {
                _animationComponent.Animate(_endAnimation);
            }

            if (_steering == null)
                return;
            _steering.TurnOff(behavior_type.seek);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsComplete())
                return;
            if (_physicsComponent == null)
                return;
            if (_steering == null)
                return;
            var feetPosition = _physicsComponent.GetFeetWorldPosition();
            //MessageDispatcher2.Instance.DispatchMsg("GetFeetPosition", 0f, this.UniqueId, 
            //    _physicsComponent.UniqueId, null);
            // TODO Move this logic elsewhere, possibly in the PHysics Component
            if (Vector2D.Vec2DDistanceSq(feetPosition, _steering.Target) < .015f)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Telegrams.StopMovement, null);
                _steering.TurnOff(behavior_type.seek);
                if (_targetLocation != null)
                {
                    foreach (var animationComponent in _animationComponents)
                    {
                        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, animationComponent.UniqueId,
                            Telegrams.SetFacingDirection, _targetLocation.Direction);
                    }
                }
                Complete();
                MessageDispatcher2.Instance.DispatchMsg("GoToComplete", 0f, this.UniqueId, 
                    GetEntity().UniqueId, null);
                if (!String.IsNullOrEmpty(_endAnimation))
                {
                    _animationComponent.Animate(_endAnimation);
                }
            }
            //if (_facingDirectionMode == DirectionMode.Automatic)
            //if (_playAnimation)
            //    SendMessageToSpriteBase(0f, Telegrams.VelocityChanged, _physicsComponent.GetPhysicsData().Velocity);
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    string waypointUniqueId = null;
        //    if (_waypoint != null)
        //    {
        //        var repo = _waypoint.GetComponent<IComponentRepository>();
        //        if (repo != null)
        //            waypointUniqueId = repo.UniqueId;
        //    }

        //    var goToData = new GoToData()
        //    {
        //        GoToType = _goToType,
        //        Immediate = _immediate,
        //        WaypointUniqueId = waypointUniqueId,
        //        Velocity = _velocity,
        //        //LookToTarget = _lookToTarget,
        //        //PlayAnimation = _playAnimation,
        //        FeetPosition = _feetPosition
        //    };
        //    base.SerializeComponent(entitySerializedData, goToData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var goToData = base.DeserializeComponent<GoToData>(entitySerializedData);
        //    if (goToData == null)
        //        return;
        //    if (!string.IsNullOrEmpty(goToData.WaypointUniqueId))
        //    {
        //        _waypoint = EntityContainer._instance.GetEntity(goToData.WaypointUniqueId).transform;
        //    }
        //    _goToType = goToData.GoToType;
        //    _immediate = goToData.Immediate;
        //    _velocity = goToData.Velocity;
        //    //_lookToTarget = goToData.LookToTarget;
        //    //_playAnimation = goToData.PlayAnimation;
        //    _feetPosition = goToData.FeetPosition;
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

                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent == null)
                        continue;
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

                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent == null)
                        continue;
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
