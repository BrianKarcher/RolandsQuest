using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Extensions;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetSteeringTargetToWaypointAtom : AtomActionBase
    {
        //public bool _lookToTarget;

        //public enum GoToType
        //{
        //    Target = 0,
        //    //Random = 1,
        //    //RandomLocation = 2,
        //    //RandomSubLocation = 3,
        //    //CurrentDestinationLocation = 4,
        //    Waypoint = 5,
        //    //CustomVelocity = 6,
        //    Self = 7
        //}

        //public GoToType _goToType;
        public string[] _messageReceivers;
        public bool sendToSelf;
        public string physicsComponentName;
        public string leftWaypointName;
        public string rightWaypointName;

        private PhysicsComponent _physicsComponent;
        private AIComponent _aIComponent;
        private AnimationComponent _animComponent;
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
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(physicsComponentName);
            _aIComponent = entity.Components.GetComponent<AIComponent>();
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animationComponents.Count; i++)
            {
                var aniamtionComponent = animationComponents[i] as AnimationComponent;
                if (aniamtionComponent.IsMain())
                {
                    _animComponent = aniamtionComponent;
                    break;
                }
            }
            //_animComponent = entity.Components.GetComponents<AnimationComponent>().Where(i => i.IsMain()).FirstOrDefault();
            
            if (sendToSelf)
            {
                //_physicsComponent.SetSteeringTarget(goToVector);
                //SendMessage(_entity.UniqueId, goToVector);
            }
            if (_messageReceivers != null)
            {
                foreach (var messageReceiver in _messageReceivers)
                {
                    var goToVector = GetTargetLocation(messageReceiver);
                    SendMessage(messageReceiver, goToVector);
                }
            }

            //_steering = _physicsComponent.GetSteering();
            //if (_steering == null)
            //    return;
            //_steering.Target = goToVector;
        }

        private void SendMessage(string messageReceiver, Vector2D goToVector)
        {
            MessageDispatcher2.Instance.DispatchMsg("SetSteeringTarget", 0f, _entity.UniqueId, messageReceiver, goToVector);
        }

        public Vector2D GetTargetLocation(string receiverId)
        {
            Vector2D feetPos;
            if (_physicsComponent != null)
                feetPos = _physicsComponent.GetFeetWorldPosition();
            else
                feetPos = _entity.transform.position;

            var leftWaypointComponent = _entity.Components.GetComponent<WaypointComponent>(leftWaypointName);
            //if (waypointComponent == null)
            //    return Vector2D.Zero();
            //return waypointComponent.transform.position;
            var rightWaypointComponent = _entity.Components.GetComponent<WaypointComponent>(rightWaypointName);

            var facingVector = _animComponent.GetFacingDirectionVector();
            var leftSign = ((Vector2)feetPos).DetermineWhichSidePointIsOn(facingVector, leftWaypointComponent.transform.position) < 0 ? true : false;

            var receiverEntity = EntityContainer._instance.GetEntity(receiverId);
            var receiverSign = ((Vector2)feetPos).DetermineWhichSidePointIsOn(facingVector, receiverEntity.transform.position) < 0 ? true : false;

            if (receiverSign == leftSign)
                return leftWaypointComponent.transform.position;
            else
                return rightWaypointComponent.transform.position;
            //switch (_goToType)
            //{
            //    case GoToType.Target:
            //        return _aIComponent.Target.transform.position;
            //    case GoToType.Self:
            //        return _physicsComponent.GetPos();
            //    //case GoToType.RandomLocation:
            //    //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
            //    //        Telegrams.ChooseRandomLocation, feetPos, (location) =>
            //    //        {
            //    //            _targetLocation = (Location)location;
            //    //        });
            //    //    return _targetLocation.transform.position;
            //    //case GoToType.RandomSubLocation:
            //    //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
            //    //        Telegrams.ChooseRandomSubLocation, feetPos, (location) =>
            //    //        {
            //    //            _targetLocation = (Location)location;
            //    //        });
            //    //    return _targetLocation.transform.position;
            //    //case GoToType.Random:
            //    //    _targetLocation = null;
            //    //    throw new NotImplementedException("Random not implemented in GoTo");
            //    //case GoToType.CurrentDestinationLocation:
            //    //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _aiComponent.UniqueId,
            //    //        Telegrams.GetCurrentDestinationLocation, null, (location) =>
            //    //        {
            //    //            _targetLocation = (Location)location;
            //    //        });
            //    //    return _targetLocation.transform.position;
            //    case GoToType.Waypoint:

            //        //return _waypoint.transform.position;
            //        //case GoToType.CustomVelocity:
            //        //    return _velocity;
            //}
            //return Vector2D.Zero();
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
