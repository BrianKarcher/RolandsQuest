using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetSteeringTargetToVectorAtom : AtomActionBase
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
            NextWaypoint = 6,
            //CustomVelocity = 6,
            Self = 7,
            Home = 8,
            Player = 9
        }

        public GoToType _goToType;
        public int WaypointIndex;
        //public string[] _messageReceivers;
        public bool sendToSelf;
        public string physicsComponentName;
        public string waypointName;

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
            Tick();
            //_steering = _physicsComponent.GetSteering();
            //if (_steering == null)
            //    return;
            //_steering.Target = goToVector;
        }

        private void Tick()
        {
            var goToVector = GetTargetLocation();
            if (goToVector == null)
                return;
            if (sendToSelf)
            {
                _physicsComponent.SetSteeringTarget(goToVector.Value);
                //SendMessage(_entity.UniqueId, goToVector);
            }
        }

        public void SendMessage(string messageReceiver)
        {
            var goToVector = GetTargetLocation();
            if (goToVector == null)
                return;
            //for (int i = 0; i < messageReceiver.Length; i++)
            //{
            //    var receiver = messageReceiver[i];
                MessageDispatcher2.Instance.DispatchMsg("SetSteeringTarget", 0f, _entity.UniqueId, messageReceiver,
                    goToVector);
            //}
        }

        private void SendMessage(string messageReceiver, Vector2D goToVector)
        {
            MessageDispatcher2.Instance.DispatchMsg("SetSteeringTarget", 0f, _entity.UniqueId, messageReceiver, goToVector);
        }

        public Vector2D? GetTargetLocation()
        {
            Vector2D feetPos;
            if (_physicsComponent != null)
                feetPos = _physicsComponent.GetFeetWorldPosition();
            else
                feetPos = _entity.transform.position;

            switch (_goToType)
            {
                case GoToType.Target:
                    if (_aIComponent.Target == null)
                        return null;
                    return _aIComponent.Target.transform.position;
                case GoToType.Self:
                    return _physicsComponent?.GetWorldPos();
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
                        return Vector2D.Zero();
                    if (waypointComponent._waypoints.Count == 0)
                        return waypointComponent.transform.position;
                    return waypointComponent._waypoints[WaypointIndex].transform.position;
                //return _waypoint.transform.position;
                //case GoToType.CustomVelocity:
                //    return _velocity;
                case GoToType.NextWaypoint:
                    var nextWaypointComponent = _entity.Components.GetComponent<WaypointComponent>(waypointName);
                    if (nextWaypointComponent == null)
                        return Vector2D.Zero();
                    if (nextWaypointComponent._waypoints.Count == 0)
                        return nextWaypointComponent.transform.position;
                    return nextWaypointComponent.NextWaypoint();
                case GoToType.Home:
                    return _aIComponent.GetHomePosition();
                case GoToType.Player:
                    return EntityContainer._instance.GetMainCharacter().transform.position;
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
            Tick();
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
