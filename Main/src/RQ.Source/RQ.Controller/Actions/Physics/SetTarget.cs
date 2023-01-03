using RQ.Common.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Set Target")]
    public class SetTarget : ActionBase
    {
        private Location _targetLocation;
        //private Vector2D _targetPosition;

        public enum GoToType
        {
            Target = 0,
            Random = 1,
            RandomLocation = 2,
            RandomSubLocation = 3,
            CurrentDestinationLocation = 4,
            Waypoint = 5,
            CustomVelocity = 6
        }

        [SerializeField]
        private GoToType _goToType = GoToType.Target;

        [SerializeField]
        private bool _immediate = false;

        [SerializeField]
        private Transform _waypoint = null;

        [SerializeField]
        private Vector2D _velocity;

        //[SerializeField]
        //private bool _lookToTarget = false;

        [SerializeField]
        private bool _playAnimation = true;

        //private Vector2D _feetPosition;

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
        //    MessageDispatcher2.Instance.StopListening("SetFeetPosition", this.UniqueId);
        //}

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //if (_sprite == null)
            //    return;
            //base.Act(otherRigidBody);
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
                    return _waypoint.transform.position;
                case GoToType.CustomVelocity:
                    return _velocity;
            }
            return Vector2D.Zero();
        }

        //public override void ActExit(Component otherRigidBody)
        //{
        //    base.ActExit(otherRigidBody);

        //    if (_targetLocation != null)
        //    {
        //        foreach (var animationComponent in _animationComponents)
        //        {
        //            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, animationComponent.UniqueId,
        //                Telegrams.SetFacingDirection, _targetLocation.Direction);
        //        }
        //    }

        //    if (_steering == null)
        //        return;
        //    _steering.TurnOff(behavior_type.seek);
        //}
    }
}
