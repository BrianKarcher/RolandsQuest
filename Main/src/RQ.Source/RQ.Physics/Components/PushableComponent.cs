using System;
using RQ.Common.Components;
using RQ.Extensions;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Pushable Component")]
    public class PushableComponent : ComponentPersistence<PushableComponent>
    {
        //public int Floor = 1;
        private PhysicsComponent _physicsComponent;

        [SerializeField]
        private float _moveSpeed = 1.0f;

        [SerializeField]
        private float _waitBetweenMoves = 0.2f;

        [SerializeField]
        private float _obstacleRayLength;

        [SerializeField]
        private float _radius;

        [SerializeField]
        private LayerMask _obstacles;

        [SerializeField]
        private string _message;
        [SerializeField]
        private bool _isOn = true;

        private float _waitTimer;

        private bool _isPushing = false;
        private bool _isMoving = false;
        private bool _isActive = false;
        private Direction _direction;
        private int x, y;

        public override void Awake()
        {
            base.Awake();
            x = Mathf.FloorToInt(transform.position.x / 16);
            y = Mathf.FloorToInt(transform.position.y / 16);
        }

        public override void Start()
        {
            base.Start();
            _physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();
        }

        public override void FixedUpdate()
        {
            if (!_isActive)
                return;
            Vector2 moveDirection = _direction.ToVector();
            var moveVelocity = moveDirection * _moveSpeed;
            var footPos = _physicsComponent == null ? _componentRepository.transform.position : _physicsComponent.GetFeetWorldPosition3();
            var middlePos = footPos;
            var side = _direction.GetSideVector();
            var sideLeft = footPos - (Vector3)(side * _radius);
            var sideRight = footPos + (Vector3)(side * _radius);
            // Check if being pushed into another object - which is illegal. Must stop it.
            // Can't be pushed into another object
            if (UnityEngine.Physics.Raycast(new Ray(middlePos, moveDirection), _obstacleRayLength, _obstacles))
                return;
            if (UnityEngine.Physics.Raycast(new Ray(sideLeft, moveDirection), _obstacleRayLength, _obstacles))
                return;
            if (UnityEngine.Physics.Raycast(new Ray(sideRight, moveDirection), _obstacleRayLength, _obstacles))
                return;


            this.transform.position += (Vector3)moveVelocity * Time.deltaTime;
        }

        //void OnCollisionStay(UnityEngine.Collision collision)
        //{
        //    if (!collision.rigidbody.CompareTag("Player"))
        //        return;

        //    //collision.
        //}

        /// <summary>
        /// This gets called externally by the FSM to start pushing the object
        /// </summary>
        /// <param name="isPushing"></param>
        /// <param name="direction"></param>
        public void SetIsPushing(bool isPushing, Direction direction)
        {
            if (!String.IsNullOrEmpty(_message))
                MessageDispatcher2.Instance.DispatchMsg(_message, 0f, this.UniqueId, _componentRepository.UniqueId, null);
            if (!_isOn)
                return;
            _isPushing = isPushing;
            _direction = direction;
            // Start the move immediately
            if (_isPushing)
                MoveBlock();
            if (!_isPushing)
            {
                _isMoving = false;
                _isActive = false;
            }
        }

        private void MoveBlock()
        {
            if (!_isOn)
                return;
            _isMoving = true;
            _isActive = true;
        }
    }
}
