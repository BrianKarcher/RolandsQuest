using RQ.Common.Components;
using RQ.Extensions;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Physics/Jump Down Trigger Component")]
    public class JumpDownTriggerComponent : ComponentPersistence<JumpDownTriggerComponent>
    {
        //public int Floor = 1;
        private PhysicsComponent _physicsComponent;

        //[SerializeField]
        //private float _moveSpeed = 1.0f;

        //[SerializeField]
        //private float _waitBetweenMoves = 0.2f;

        //[SerializeField]
        //private float _obstacleRayLength;
        //[SerializeField]
        //private LayerMask _obstacles;

        //private float _waitTimer;

        //private bool _isPushing = false;
        //private bool _isMoving = false;
        //private bool _isActive = false;
        //private Direction _direction;
        //private int x, y;

        public override void Awake()
        {
            base.Awake();
            _physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();
            //x = Mathf.FloorToInt(transform.position.x / 16);
            //y = Mathf.FloorToInt(transform.position.y / 16);
        }

        //public override void FixedUpdate()
        //{
        //    //if (!_isActive)
        //    //    return;
        //    //Vector2 moveDirection = _direction.ToVector();
        //    //var moveVelocity = moveDirection * _moveSpeed;
        //    //// Check if being pushed into another object - which is illegal. Must stop it.
        //    //var raycast = UnityEngine.Physics.Raycast(new Ray(transform.position, moveDirection), out var raycastHit, _obstacleRayLength, _obstacles);
        //    //// Can't be pushed into another object
        //    //if (raycast)
        //    //    return;
        //    //this.transform.position += (Vector3)moveVelocity * Time.deltaTime;
        //}

        //void OnCollisionStay(UnityEngine.Collision collision)
        //{
        //    if (!collision.rigidbody.CompareTag("Player"))
        //        return;

        //    //collision.
        //}

        //public void SetIsPushing(bool isPushing, Direction direction)
        //{
        //    _isPushing = isPushing;
        //    _direction = direction;
        //    // Start the move immediately
        //    if (_isPushing)
        //        MoveBlock();
        //    if (!_isPushing)
        //    {
        //        _isMoving = false;
        //        _isActive = false;
        //    }
        //}

        //private void MoveBlock()
        //{
        //    _isMoving = true;
        //    _isActive = true;
        //}
    }
}
