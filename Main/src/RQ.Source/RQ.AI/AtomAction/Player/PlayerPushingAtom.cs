using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using RQ.Entity;
using RQ.Extensions;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class PlayerPushingAtom : AtomActionBase
    {
        //public TweenToColorInfo _overlayColor = null;
        //private float _endTime;
        private PushableComponent _pushingObject = null;
        private PlayerComponent _playerComponent;
        private PhysicsComponent _physicsComponent;
        private FixedJoint _pushableFixedJoint = null;

        //private Direction _direction;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            if (_physicsComponent == null)
                _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();
            if (_pushableFixedJoint != null)
                GameObject.Destroy(_pushableFixedJoint);
            _playerComponent.SetMoveOnInput(false, 0, false);
            if (_pushingObject == null)
            {
                // Nothing to do, finish
                //_isRunning = false;
                return;
            }
            var direction = _pushingObject.transform.position - _entity.transform.position;
            _pushingObject.SetIsPushing(true, direction.GetDirection());
            //_pushableFixedJoint = gameObject.AddComponent<FixedJoint>();
            _pushableFixedJoint = _physicsComponent.gameObject.AddComponent<FixedJoint>();
            // TODO Assuming the other Rigidbody is at the root, probably bad practice.
            var otherRigidBody = _pushingObject.GetComponentRepository().GetComponent<Rigidbody>();
            _pushableFixedJoint.connectedBody = otherRigidBody;
        }

        public override void End()
        {
            _pushingObject?.SetIsPushing(false, Direction.None);
            _pushingObject = null;
            _playerComponent.SetMoveOnInput(true, 0, false);
            if (_pushableFixedJoint != null)
            {
                GameObject.Destroy(_pushableFixedJoint);
                _pushableFixedJoint = null;
            }
        }

        public override AtomActionResults OnUpdate()
        {
            Tick();
            //if (_endTime > UnityEngine.Time.time)
            //    return AtomActionResults.Success;
            //return AtomActionResults.Running;
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        private void Tick()
        {
            var inputAxis = _playerComponent.GetInputAxis();
            var inputMagnitude = inputAxis.magnitude;
            var physicsData = _physicsComponent.GetPhysicsData();
            var velocityMagnitude = physicsData.Velocity.magnitude;
            var footPos = _physicsComponent.GetFeetWorldPosition3();
            var isPushing = _playerComponent.IsPushing(footPos, inputAxis, inputMagnitude, velocityMagnitude, out var raycastHit);
            if (!isPushing)
                _isRunning = false;
        }

        //public void SetDirection(Direction direction)
        //{
        //    _direction = direction;
        //}

        public void SetPushableObject(GameObject gameObject)
        {
            var entityHit = gameObject.GetComponent<IComponentRepository>();
            if (entityHit == null)
            {
                _pushingObject = null;
                return;
            }
            var otherPushableComponent = entityHit.Components.GetComponent<PushableComponent>();
            _pushingObject = otherPushableComponent;
        }
    }
}
