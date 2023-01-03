using Rewired;
using RQ.Animation;
using RQ.Audio;
using RQ.Entity.Data;
using RQ.Enums;
using RQ.Input;
using RQ.Messaging;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using System.Collections;
using RQ.Entity.Components;
using UnityEngine;
using RQ.Extensions;
using RQ.Common.Controllers;

namespace RQ.Entity
{
    [AddComponentMenu("RQ/Components/Player")]
    public class PlayerComponent : InputComponent
    {
        [SerializeField]
        protected PhysicsAffector _inputPhysicsAffector;
        PhysicsComponent _physicsComponent;
        private bool _reverseMovement;
        [SerializeField]
        private float _reverseTimeLimit = 5f;
        [SerializeField]
        private AudioClip _levelUp;
        private Action _destroyTracker;
        [SerializeField]
        private GameObject _dizzyParent;
        [SerializeField]
        private GameObject _dizzyTransform;
        private GameObject _dizzyGO;
        [SerializeField]
        private bool _moveOnInput = true;
        [SerializeField]
        private int _moveOnInputPriority = 0;
        [SerializeField]
        private bool _setFacingDirectionOnInput = true;

        [SerializeField]
        private bool _inputUseVelocity = false;

        [SerializeField]
        private ForceMode _inputForceMode;


        private AnimationComponent _animComponent;
        private Rewired.Player _player;
        private bool _initialized = false;

        [SerializeField]
        private LayerMask _obstacles;

        [SerializeField]
        private float _pushWaitTime;
        [SerializeField]
        private float _pushRayLength;

        [SerializeField]
        private GameObject _liftingObject;

        [SerializeField]
        // Where to place the object being lifted every frame
        private GameObject _liftingObjectPosition;

        private Vector2 _vmove;


        // Comment out these next few SerializeField attributes after testing is complete.
        //[SerializeField]
        //private float _pushBeginTime;
        //[SerializeField]
        //private bool _inPushWaitTime;
        //[SerializeField]
        //private bool _isPushing;
        //public bool IsPushing { get { return _isPushing; } set { _isPushing = value; } }


        private long _enableInputId2, _reverseMovementId, _stopReverseMovementId;
        private Action<Telegram2> _reverseMovementDelegate, _stopReverseMovementDelegate, _enableInputDelegate;

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;
            _reverseMovement = false;

            //Debug.LogError("Awake called on PlayerComponent");
            _reverseMovementDelegate = (data) =>
            {
                _reverseMovement = true;
                MessageDispatcher2.Instance.RemoveMessages("StopReverseMovement", this.UniqueId);
                MessageDispatcher2.Instance.DispatchMsg("StopReverseMovement", _reverseTimeLimit,
                    this.UniqueId, this.UniqueId, null);
                if (_dizzyTransform != null && _dizzyParent != null)
                {
                    if (_dizzyGO != null)
                        GameObject.Destroy(_dizzyGO);
                    _dizzyGO = GameObject.Instantiate(_dizzyTransform,
                        _dizzyParent.transform.position + new Vector3(0f, 0f, -.1f), Quaternion.identity) as GameObject;
                    _dizzyGO.transform.parent = _dizzyParent.transform;
                }
            };
            _stopReverseMovementDelegate = (data) =>
            {
                _reverseMovement = false;
                if (_dizzyGO != null)
                {
                    GameObject.Destroy(_dizzyGO);
                    _dizzyGO = null;
                }
            };
            _enableInputDelegate = (data) =>
            {
                _moveOnInput = (string) data.ExtraInfo == "1" ? true : false;
            };
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            _reverseMovementId = MessageDispatcher2.Instance.StartListening("ReverseMovement", _componentRepository.UniqueId, _reverseMovementDelegate);
            //_componentRepository.StartListening("ReverseMovement", this.UniqueId, );
            _stopReverseMovementId = MessageDispatcher2.Instance.StartListening("StopReverseMovement", _componentRepository.UniqueId, 
                _stopReverseMovementDelegate);
            //MessageDispatcher2.Instance.StartListening("StopReverseMovement", this.UniqueId, );
            //StartCoroutine(AIUpdate());
        }

        public override void OnDisable()
        {
            base.OnDisable();
            MessageDispatcher2.Instance.StopListening("ReverseMovement", _componentRepository.UniqueId, _reverseMovementId);
            //_componentRepository.StopListening("ReverseMovement", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("StopReverseMovement", this.UniqueId, _stopReverseMovementId);
            //StopCoroutine(AIUpdate());
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //Debug.LogError("Destroy called on PlayerComponent");
            if (_destroyTracker != null)
                _destroyTracker();
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            _physicsComponent = base._componentRepository.Components.GetComponent<PhysicsComponent>();
            _animComponent = _componentRepository.Components.GetComponent<AnimationComponent>();
            _physicsComponent.SetPhysicsAffector("Input", _inputPhysicsAffector);
        }

        public override void StartListening()
        {
            base.StartListening();
            _enableInputId2 = MessageDispatcher2.Instance.StartListening("EnableInput", GetComponentRepository().UniqueId, _enableInputDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("EnableInput", GetComponentRepository().UniqueId, _enableInputId2);
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.SetCurrentHealth:
                    UpdateStatsInHud(msg.ExtraInfo as EntityStatsData);
                    break;
            }

            return false;
        }

        private void Initialize()
        {
            _player = Rewired.ReInput.players.GetPlayer(0); // get the player by id
            _initialized = true;
        }

        public override void Update()
        {
            base.Update();
            if (!ReInput.isReady) return; // check if Rewired is ready (if false, editor is compiling)
            if (!_initialized)
                Initialize();
            if (!IsInputEnabled())
                return;

            if (_setFacingDirectionOnInput)
                SetFacingDirectionOnInput();

            //if (_liftingObject != null)
            //{
            //    _liftingObject.transform.position = new Vector3(_liftingObjectPosition.transform.position.x, _liftingObjectPosition.transform.position.y,
            //        _liftingObject.transform.position.z);
            //}
            if (_moveOnInput)
                MoveOnInput();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var maxForce = _inputPhysicsAffector.MaxForce;
            var maxSpeed = _physicsComponent.GetPhysicsData().MaxSpeed;
            //var currentVelocity = _physicsComponent.GetVelocity();

            //var destinationVelocity = vmove * _inputPhysicsAffector.MaxSpeed;

            //var force = (destinationVelocity - currentVelocity).normalized * maxForce;

            //if (force.sqrMagnitude < float.Epsilon)
            //    force = Vector2.zero;

            if (_moveOnInput)
            {
                if (_vmove == Vector2.zero)
                {
                    //_physicsComponent.AddForce(-currentVelocity);
                    _inputPhysicsAffector.Stop();
                    _physicsComponent.SetVelocity(Vector2.zero);
                    _physicsComponent.GetPhysicsData().InputVelocity = Vector2.zero;
                }
                else
                {
                    //_inputPhysicsAffector.Velocity = Vector2.zero;
                    //_inputPhysicsAffector.Force = force;
                    //_inputPhysicsAffector.Force = vmove * maxForce;
                    //_physicsComponent.AddForce(vmove * maxForce);
                    if (_inputUseVelocity)
                    {
                        _physicsComponent.SetPreviousVelocity(_physicsComponent.GetVelocity());
                        //Debug.Log(_physicsComponent.GetVelocity());
                        var velocity = _vmove * maxSpeed;
                        _physicsComponent.SetVelocity(velocity);
                    }
                    else
                    {
                        var velocity = _vmove * maxForce;
                        _physicsComponent.AddForce(velocity, _inputForceMode);
                    }

                    //_physicsComponent.SetVelocity(velocity);
                    //var currentSpeed = _physicsComponent.GetPhysicsData().Velocity.magnitude;
                    //var currentVelocity = _physicsComponent.GetPhysicsData().Velocity;
                    ////Mathf.Max(currentSpeed, )
                    //var targetVelocity = vmove * _physicsComponent.GetPhysicsData().MaxSpeed;
                    ////_physicsComponent.AddForce(velocity.normalized * 50f);
                    ////_physicsComponent.AddForce(currentVelocity - targetVelocity, ForceMode.Impulse);
                    ////_physicsComponent.AddForce(targetVelocity - currentVelocity, ForceMode.Impulse);
                    //_physicsComponent.AddForce((targetVelocity - currentVelocity) * 50f);

                    //_physicsComponent.GetPhysicsData().InputVelocity = targetVelocity;
                }
            }
        }

        //public override void FixedUpdate()
        //{
        //    if (GameDataController.Instance.LoadingGame)
        //        return;

        //}

        //public IEnumerator AIUpdate()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(.1f);
        //        ProcessPushing();
        //    }
        //}

        //private void ProcessPushing()
        //{
        //    var inputAxis = GetInputAxis();
        //    var inputMagnitude = inputAxis.magnitude;
        //    var physicsData = _physicsComponent.GetPhysicsData();
        //    var velocityMagnitude = physicsData.Velocity.magnitude;
        //    var footPos = _physicsComponent.GetFeetWorldPosition3();
        //    var isPushing = IsPushing(footPos, inputAxis, inputMagnitude, velocityMagnitude, out var raycastHit);
        //    if (!isPushing)
        //    {
        //        _inPushWaitTime = false;
        //        // Are we switching from pushing to not pushing?
        //        if (_isPushing)
        //        {
        //            _moveOnInput = true;
        //        }
        //        _isPushing = false;
        //        _pushBeginTime = 0f;
        //        _pushingObject?.SetIsPushing(false, Direction.None);
        //        if (_pushableFixedJoint != null)
        //        {
        //            GameObject.Destroy(_pushableFixedJoint);
        //            _pushableFixedJoint = null;
        //        }
        //    }
        //    else
        //    {
        //        if (_isPushing)
        //            return;
        //        if (!_inPushWaitTime)
        //        {
        //            _pushBeginTime = Time.time;
        //            _inPushWaitTime = true;
        //        }
        //        else
        //        {
        //            // Has the player been pushing long enough?
        //            if (Time.time < _pushBeginTime + _pushWaitTime)
        //                return;
        //            _isPushing = true;
        //            _inPushWaitTime = false;
        //            var entityHit = raycastHit.rigidbody?.GetComponent<IComponentRepository>();
        //            if (entityHit == null)
        //                return;
        //            var otherPushableComponent = entityHit.Components.GetComponent<PushableComponent>();
        //            if (otherPushableComponent == null)
        //                return;
        //            var dir = raycastHit.transform.position - _physicsComponent.GetFeetWorldPosition3();
        //            _pushDirection = dir.GetDirection();
        //            otherPushableComponent.SetIsPushing(true, _pushDirection);
        //            _pushingObject = otherPushableComponent;
        //            if (_pushableFixedJoint != null)
        //                GameObject.Destroy(_pushableFixedJoint);
        //            _pushableFixedJoint = gameObject.AddComponent<FixedJoint>();
        //            _pushableFixedJoint.connectedBody = raycastHit.rigidbody;
        //            _moveOnInput = false;
        //        }
        //    }
        //}

        /// <summary>
        /// Is player pushing against something that is blocking their movement?
        /// </summary>
        /// <param name="footPos"></param>
        /// <param name="inputAxis"></param>
        /// <param name="inputMagnitude"></param>
        /// <param name="velocityMagnitude"></param>
        /// <param name="raycastHit"></param>
        /// <returns></returns>
        public bool IsPushing(Vector3 footPos, Vector2 inputAxis, float inputMagnitude, float velocityMagnitude, out RaycastHit raycastHit)
        {
            //Debug.Log("(PlayerComponent) IsPushing called.");
            //var isPushing = false;
            if (inputMagnitude < 0.3f || velocityMagnitude > 0.3f)
            {
                raycastHit = new RaycastHit();
                return false;
            }
            //working here
            // Is something blocking our path?
            var raycast = UnityEngine.Physics.Raycast(new Ray(footPos, inputAxis), out raycastHit, _pushRayLength, _obstacles);
            if (!raycast)
                return false;

            return true;
        }

        private void MoveOnInput()
        {
            // TODO Get rid of the tight coupling to the Physics Component and GetPhysicsData
            // if caching the PhysicsData, need to be aware that deserialization will destroy it.
            //var physicsData = _physicsComponent.GetPhysicsData();
            //var moveRight = _player.GetButton("Horizontal");
            //var moveLeft = _player.GetNegativeButton("Horizontal");

            //if (xAxis > 0.0f)
            //{
            //    //Debug.Log("Hi");
            //}
            _vmove = GetInputAxis();

            //if (input.axisInput.y > 0f)
            //{
            //    int i = 1;
            //}
        }

        private void SetFacingDirectionOnInput()
        {
            var vmove = GetInputAxis();
            if (vmove.sqrMagnitude > float.Epsilon)
                _animComponent.SetFacingDirection(vmove);
        }

        public Vector2 GetInputAxis()
        {
            if (_player == null || !ReInput.isReady)
                return Vector2.zero;
            Vector2 vmove = new Vector2();
            var deadZone = 0.2f;
            var xAxis = _player.GetAxis("Horizontal");
            var yAxis = _player.GetAxis("Vertical");
            //vmove.x = moveRight ? 1 : 0f;
            //vmove.x = moveLeft ? -1 : 0f;
            vmove.x = xAxis > deadZone ? 1 : 0f;
            vmove.x = xAxis < -deadZone ? -1 : vmove.x;
            vmove.y = yAxis > deadZone ? 1 : 0f;
            vmove.y = yAxis < -deadZone ? -1 : vmove.y;
            vmove.Normalize();
            if (_reverseMovement)
                vmove = -vmove;
            return vmove;
        }

        //public void ProcessDirectionalInput(RawInput input)
        //{

        //}

        public void ProcessMovementInput(RawInput input)
        {
            //if (!isActive)
            //    return;
            // @todo FINISH THIS!
            //MovementData movementData = new MovementData();
            //if (input != null)
            //{
            //    //if (input.IsButtonDown(Button.Fire))
            //    //{
            //    //    //movementData.PerformAttack = true;
            //    //    //MainCharacter.Attack();
            //    //}
            //    ProcessDirectionalInput(input);
            //    //aiSystem.ProcessAIResponse(movementData);
            //}

            //return movementData;
        }

        public void UpdateStatsInHud(EntityStatsData entityStatsData)
        {
            // Update the UI with the new health information
            MessageDispatcher2.Instance.DispatchMsg("UpdateStatsInHud", 0f, this.UniqueId, "UI Manager", entityStatsData);
            //MessageDispatcher2.Instance.DispatchMsg(0f, this.UniqueId, "UI Manager",
            //    Telegrams.SetCurrentHealth, entityStatsData);
        }

        public void SetDestroyTracker(Action destroyTracker)
        {
            _destroyTracker = destroyTracker;
        }

        public void SetMoveOnInput(bool moveOnInput, int priority, bool resetPriority)
        {
            if (priority >= _moveOnInputPriority)
            {
                //Debug.LogError($"(PlayerComponent) Setting MoveOnInput to {moveOnInput}");
                _moveOnInput = moveOnInput;
                if (resetPriority)
                    _moveOnInputPriority = 0;
                else
                    _moveOnInputPriority = priority;
            }
        }

        public void SetFacingDirectionOnInput(bool setFacingDirectionOnInput)
        {
            _setFacingDirectionOnInput = setFacingDirectionOnInput;
        }

        public bool GetSetFacingDirectionOnInput()
        {
            return _setFacingDirectionOnInput;
        }

        //public bool GetIsPushing()
        //{
        //    return _isPushing;
        //}

        public float GetPushWaitTime()
        {
            return _pushWaitTime;
        }

        public float GetPushRayLength()
        {
            return _pushRayLength;
        }

        public void SetLiftingObject(GameObject liftingObject)
        {
            _liftingObject = liftingObject;
        }

        public GameObject GetLiftingObject()
        {
            return _liftingObject;
        }

        public Vector3 GetLiftingObjectPosition()
        {
            return _liftingObjectPosition.transform.position;
        }
    }
}
