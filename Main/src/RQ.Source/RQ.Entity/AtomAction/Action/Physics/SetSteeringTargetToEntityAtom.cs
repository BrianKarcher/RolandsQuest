using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using RQ.Common.Container;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetSteeringTargetToEntityAtom : AtomActionBase
    {
        //public bool _lookToTarget;

        public enum GoToType
        {
            Target = 0,
            //Random = 1,
            //RandomLocation = 2,
            //RandomSubLocation = 3,
            //CurrentDestinationLocation = 4,
            Waypoint = 5,
            //CustomVelocity = 6,
            Self = 7,
            Player = 8,
            Custom = 9
        }

        public GoToType _goToType;
        //public string[] _messageReceivers;
        //public bool sendToSelf;
        public string physicsComponentName;
        public string waypointName;
        public GameObject _customGameObject;

        private PhysicsComponent _physicsComponent;
        private AIComponent _aIComponent;
        //private SteeringBehaviorManager _steering;
        //private Steering
        //[SerializeField]
        //private float _delay = 0f;
        //[SerializeField]
        //private bool _killWhenAnimationCompletes = false;
        //public string AnimationType;
        //private AnimationComponent _animComponent;
        //private IComponentRepository _entity;
        //private bool _isRunning;
        //private long _killSelfIndex;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(physicsComponentName);
            if (_aIComponent == null)
                _aIComponent = entity.Components.GetComponent<AIComponent>();
            
            //if (sendToSelf)
            //{
            //    _physicsComponent.SetSteeringTarget(goToVector);
            //    //SendMessage(_entity.UniqueId, goToVector);
            //}

            //_steering = _physicsComponent.GetSteering();
            //if (_steering == null)
            //    return;
            //_steering.Target = goToVector;
        }

        public void SendMessage(string messageReceiver)
        {
            var goToTransform = GetTargetLocation();
            SendMessage(messageReceiver, goToTransform);
        }

        private void SendMessage(string messageReceiver, Transform goToTransform)
        {
            MessageDispatcher2.Instance.DispatchMsg("SetAITarget", 0f, _entity.UniqueId, messageReceiver, goToTransform);
        }

        public Transform GetTargetLocation()
        {
            Vector2D feetPos;
            if (_physicsComponent != null)
                feetPos = _physicsComponent.GetFeetWorldPosition();
            else
                feetPos = _entity.transform.position;

            switch (_goToType)
            {
                case GoToType.Target:
                    return _aIComponent.Target.transform;
                case GoToType.Self:
                    return _entity.transform;
                //case GoToType.RandomLocation:
                //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                //        Telegrams.ChooseRandomLocation, feetPos, (location) =>
                //        {
                //            _targetLocation = (Location)location;
                //        });
                //    return _targetLocation.transform.position;
                //case GoToType.RandomSubLocation:
                //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                //        Telegrams.ChooseRandomSubLocation, feetPos, (location) =>
                //        {
                //            _targetLocation = (Location)location;
                //        });
                //    return _targetLocation.transform.position;
                //case GoToType.Random:
                //    _targetLocation = null;
                //    throw new NotImplementedException("Random not implemented in GoTo");
                //case GoToType.CurrentDestinationLocation:
                //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
                //        Telegrams.GetCurrentDestinationLocation, null, (location) =>
                //        {
                //            _targetLocation = (Location)location;
                //        });
                //    return _targetLocation.transform.position;
                case GoToType.Waypoint:
                    var waypointComponent = _entity.Components.GetComponent<WaypointComponent>(waypointName);
                    if (waypointComponent == null)
                        return null;
                    return waypointComponent.transform;
                //return _waypoint.transform.position;
                //case GoToType.CustomVelocity:
                //    return _velocity;
                case GoToType.Player:
                    return EntityContainer._instance.GetMainCharacter().transform;
                case GoToType.Custom:
                    return _customGameObject.transform;
            }
            return null;
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _killSelfIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
        //    {
        //        //var animation = _animComponent.Get
        //        if ((string)data.ExtraInfo != AnimationType)
        //            return;
        //        _isRunning = false;
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        //}

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
