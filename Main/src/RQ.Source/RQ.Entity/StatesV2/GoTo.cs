using RQ.Common.Components;
using RQ.Controller.Actions;
using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Go To")]
    public class GoToState : AnimatorState
    {
        private Location _targetLocation;
        //private Vector2D _targetPosition;

        //public enum GoToType
        //{
        //    Target = 0,
        //    Ranodm = 1,
        //    RandomLocation = 2,
        //    RandomSubLocation = 3,
        //    CurrentDestinationLocation = 4
        //}

        [SerializeField]
        private Transform _waypoint = null;

        [SerializeField]
        private bool _useEntityData = false;

        [SerializeField]
        private bool _useEntityWaypoints = false;

        [SerializeField]
        private GoToType _goToType;

        //[SerializeField]
        //private bool _playAnimation = true;

        private Transform _currentWaypoint = null;

        public override void Enter()
        {
            //if (_sprite == null)
            //    return;
            base.Enter();
            if (_useEntityData)
            {
                _goToType = _aiComponent.TargetingData.GoToType;
            }
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
            //_aiComponent.GoingToVector = goToVector;
            _steering.Target = goToVector;

            _steering.TurnOn(behavior_type.seek);
        }

        public Vector2D GetTargetLocation()
        {
            var feetPos = _physicsComponent.GetFeetWorldPosition();
            switch (_goToType)
            {
                case GoToType.Target:
                    _targetLocation = null;
                    return _aiComponent.GetTarget().transform.position;
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
            }
            return Vector2D.Zero();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsComplete())
                return;
            var feetPos = _physicsComponent.GetFeetWorldPosition();
            if (Vector2D.Vec2DDistanceSq(feetPos, _steering.Target) < .015f)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Telegrams.StopMovement, null);
                _steering.TurnOff(behavior_type.seek);
                if (_targetLocation != null)
                {
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _animationComponent.UniqueId,
                        Telegrams.SetFacingDirection, _targetLocation.Direction);
                }
                Complete();
            }
        }

        public override void Exit()
        {
            base.Exit();

            if (_targetLocation != null)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _animationComponent.UniqueId,
                    Telegrams.SetFacingDirection, _targetLocation.Direction);
            }

            if (_steering == null)
                return;
            _steering.TurnOff(behavior_type.seek);
        }
    }
}
