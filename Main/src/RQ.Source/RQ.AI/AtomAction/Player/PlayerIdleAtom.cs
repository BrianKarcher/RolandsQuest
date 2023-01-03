using RQ.Entity;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Extensions;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.AtomAction.Player
{
    public class PlayerIdleAtom : AtomActionBase
    {
        private float _pushBeginTime;

        private PhysicsComponent _physicsComponent;
        private PlayerComponent _playerComponent;
        //private FixedJoint _pushableFixedJoint = null;

        private GameObject _otherObject;
        
        private bool _inPushWaitTime;
        private bool _isPushing;

        private float _pushWaitTime;
        private float _pushRayLength;
        //private Direction _pushDirection;
        private string _nextEvent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();

            _pushWaitTime = _playerComponent.GetPushWaitTime();
            _pushRayLength = _playerComponent.GetPushRayLength();
            _isPushing = false;
            _inPushWaitTime = false;
            //MessageDispatcher2.Instance.DispatchMsg("TweenToColor", 0f, string.Empty, "Graphics Engine", _overlayColor);
            //_endTime = UnityEngine.Time.time + _overlayColor.Delay + _overlayColor.Duration;
        }


        public override AtomActionResults OnUpdate()
        {
            ProcessPushing();
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        //public void Tick()
        //{
            
        //}

        private void ProcessPushing()
        {
            var inputAxis = _playerComponent.GetInputAxis();
            var inputMagnitude = inputAxis.magnitude;
            var physicsData = _physicsComponent.GetPhysicsData();
            //var velocityMagnitude = physicsData.Velocity.magnitude;
            var velocityMagnitude = _physicsComponent.GetPreviousVelocity().magnitude;
            var footPos = _physicsComponent.GetFeetWorldPosition3();
            var isPushing = _playerComponent.IsPushing(footPos, inputAxis, inputMagnitude, velocityMagnitude, out var raycastHit);
            if (!isPushing)
            {
                _inPushWaitTime = false;
                // Are we switching from pushing to not pushing?
                //if (_isPushing)
                //{
                //    _playerComponent.SetMoveOnInput(true, 0, false);
                //    //_moveOnInput = true;
                //}
                _isPushing = false;
                _pushBeginTime = 0f;
                //_pushingObject?.SetIsPushing(false, Direction.None);
                //if (_pushableFixedJoint != null)
                //{
                //    GameObject.Destroy(_pushableFixedJoint);
                //    _pushableFixedJoint = null;
                //}
            }
            else
            {
                if (_isPushing)
                    return;
                if (!_inPushWaitTime)
                {
                    _pushBeginTime = Time.time;
                    _inPushWaitTime = true;
                }
                else
                {
                    // Has the player been pushing long enough?
                    if (Time.time < _pushBeginTime + _pushWaitTime)
                        return;
                    _isPushing = true;
                    _inPushWaitTime = false;
                    //var entityHit = raycastHit.rigidbody?.GetComponent<IComponentRepository>();
                    //if (entityHit == null)
                    //    return;

                    // Well, we hit something. Now to decide what to do.
                    PushProcessing(/*entityHit, */raycastHit);
                }
            }
        }

        private void PushProcessing(/*IComponentRepository entityHit, */RaycastHit raycastHit)
        {
            _otherObject = raycastHit.transform.gameObject;
            var dir = raycastHit.transform.position - _physicsComponent.GetFeetWorldPosition3();
            //_pushDirection = dir.GetDirection();
            var entityHit = raycastHit.rigidbody?.GetComponent<IComponentRepository>();
            var otherJumpDownTriggerComponent = entityHit?.Components.GetComponent<JumpDownTriggerComponent>();
            if (otherJumpDownTriggerComponent != null)
            {
                _nextEvent = "JumpDown";
                _isRunning = false;
                return;
            }
            var otherPushableComponent = entityHit?.Components.GetComponent<PushableComponent>();
            if (otherPushableComponent != null)
            {
                _nextEvent = "Push";
                _isRunning = false;
                return;
            }
            // Put Rolan in the Push state even if there is nothing pushable about this object.
            _nextEvent = "Push";
            _isRunning = false;

            //otherPushableComponent.SetIsPushing(true, _pushDirection);
            ////_pushingObject = otherPushableComponent;
            //if (_pushableFixedJoint != null)
            //    GameObject.Destroy(_pushableFixedJoint);
            ////_pushableFixedJoint = gameObject.AddComponent<FixedJoint>();
            //_pushableFixedJoint = _physicsComponent.gameObject.AddComponent<FixedJoint>();
            //_pushableFixedJoint.connectedBody = raycastHit.rigidbody;
            //_playerComponent.SetMoveOnInput(false, 0, false);
        }

        //public Direction GetDirection()
        //{
        //    return _pushDirection;
        //}

        public GameObject GetOtherObject()
        {
            return _otherObject;
        }

        public string GetNextEvent()
        {
            return _nextEvent;
        }
    }
}
