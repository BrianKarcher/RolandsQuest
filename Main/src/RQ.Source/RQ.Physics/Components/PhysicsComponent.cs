using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.ManageScene;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Physics.SteeringBehaviors;
using System;
using System.Collections.Generic;
using RQ.Model.Enums;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Physics/Physics")]
    public class PhysicsComponent : ComponentPersistence<PhysicsComponent>, IBasicPhysicsComponent
    {
        [SerializeField]
        protected PhysicsData _physicsData = new PhysicsData();
        [SerializeField]
        protected SteeringPhysicsAffector _steeringPhysicsAffector;
        [SerializeField]
        protected PhysicsData _originalPhysicsData = new PhysicsData();
        [SerializeField]
        private bool _autoUpdateHeading = true;

        //the steering behavior class
        [SerializeField]
        protected SteeringBehaviorManager _steering;

        public float FieldOfView { get { return _physicsData.FieldOfView; } set { _physicsData.FieldOfView = value; } }

        public float LineOfSight { get { return _physicsData.LineOfSight; } set { _physicsData.LineOfSight = value; } }

        private Rigidbody2D _rigidBody2D;
        private Rigidbody _rigidBody3D;
        private long _setSteeringTargetId;
        private long _setTargetEntityId;
        private Dictionary<string, IPhysicsAffector> _physicsAffectors { get; set; }
        private List<IPhysicsAffector> _physicsAffectorsList { get; set; }
        private Coroutine _lateFixedUpdate;
        private GameConfig _gameConfig;
        private Vector3 _previousVelocity;

        private long _setPosId, _setMaxSpeedId, _setMaxSpeedToOriginalId, _getFeetPositionId, _setPathId, _stopMovementId;

        private Action<Telegram2> _setMaxSpeedDelegate;
        private Action<Telegram2> _setPosDelegate;
        private Action<Telegram2> _setMaxSpeedToOriginalDelegate;
        private Action<Telegram2> _getFeetPositionDelegate;
        private Action<Telegram2> _setPathDelegate;
        private Action<Telegram2> _stopMovementDelegate;
        private Action<Telegram2> _setSteeringTargetDelegate;
        private Action<Telegram2> _setAITargetDelegate;

        public override void Awake()
        {
            base.Awake();
            _physicsAffectors = new Dictionary<string, IPhysicsAffector>();
            _physicsAffectorsList = new List<IPhysicsAffector>();
            _steeringPhysicsAffector?.Init(this);
            _physicsAffectors["Steering"] = _steeringPhysicsAffector;
            _physicsAffectorsList.Add(_steeringPhysicsAffector);
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _rigidBody3D = GetComponent<Rigidbody>();
            if (_physicsData.Foot != null)
                _physicsData._footOffset = _physicsData.Foot.transform.localPosition;
            //_gameConfig = GameObject.FindObjectOfType(typeof(IGameStateController));

            if (_physicsData.InitialVelocity != Vector2.zero)
                SetVelocity(_physicsData.InitialVelocity);

            var velocity = GetVelocity();

            if (_autoUpdateHeading && velocity != Vector2.zero)
            {
                _physicsData.Heading = Vector2D.Vec2DNormalize(velocity);
                var heading = _physicsData.Heading;
                _physicsData.Side = new Vector2(-heading.y, heading.x);
            }
            //_physicsData.OriginalZ = GetPos().z;
            //_physicsData._zOffset = 0f;

            //if (_rigidBody2D == null && _rigidBody3D == null)
            //    Debug.LogError("No RigidBody exists for " + transform.name);
            SetupSteeringManager();

            _setMaxSpeedDelegate = (data) =>
            {
                _steeringPhysicsAffector.MaxSpeed = Convert.ToSingle(data.ExtraInfo);
            };
            _setPosDelegate = (data) =>
            {
                SetWorldPos((Vector2D) data.ExtraInfo);
            };
            _setMaxSpeedToOriginalDelegate = (data) =>
            {
                _steeringPhysicsAffector.MaxSpeed = _steeringPhysicsAffector.OriginalMaxSpeed;
            };
            _getFeetPositionDelegate = (data) =>
            {
                MessageDispatcher2.Instance.DispatchMsg("SetFeetPosition", 0f, this.UniqueId, data.SenderId,
                    GetFeetWorldPosition());
            };
            _setPathDelegate = (data) =>
            {
                var path = (List<Vector3>) data.ExtraInfo;
                var behavior =
                    _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
                behavior.SetPath(path);
            };
            _stopMovementDelegate = (data) =>
            {
                Stop();
            };
            _setSteeringTargetDelegate = (data) =>
            {
                var target = (Vector2D) data.ExtraInfo;
                _steering.Target = target;
            };
            _setAITargetDelegate = (data) =>
            {
                var target = data.ExtraInfo as Transform;
                var repo = target.GetComponent<ComponentRepository>();
                var physicsComponent = repo.Components.GetComponent<PhysicsComponent>();
                // TODO Change this to the repo, a repo can contain many physics components.
                _steering.TargetAgent1 = physicsComponent;
            };
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            _steering.CollisionComponent = GetCollisonComponent();
            //_lateFixedUpdate = StartCoroutine(LateFixedUpdate());
        }

        public override void StartListening()
        {
            base.StartListening();
            _setPosId = MessageDispatcher2.Instance.StartListening("SetPos", this.UniqueId, _setPosDelegate);

            _setMaxSpeedId = MessageDispatcher2.Instance.StartListening("SetMaxSpeed", this.UniqueId, _setMaxSpeedDelegate);
            //_componentRepository.StartListening("SetMaxSpeed", this.UniqueId, _setMaxSpeedDelegate);
            _setMaxSpeedToOriginalId = MessageDispatcher2.Instance.StartListening("SetMaxSpeedToOriginal", this.UniqueId, 
                _setMaxSpeedToOriginalDelegate);
            //_componentRepository.StartListening("SetMaxSpeedToOriginal", this.UniqueId, _setMaxSpeedToOriginalDelegate);
            _getFeetPositionId = MessageDispatcher2.Instance.StartListening("GetFeetPosition", this.UniqueId, _getFeetPositionDelegate);
            _setPathId = MessageDispatcher2.Instance.StartListening("SetPath", this.UniqueId, _setPathDelegate);

            _stopMovementId = MessageDispatcher2.Instance.StartListening("StopMovement", this.UniqueId, _stopMovementDelegate);
            //_componentRepository.StartListening("StopMovement", this.UniqueId, _stopMovementDelegate);
            _setSteeringTargetId = MessageDispatcher2.Instance.StartListening("SetSteeringTarget", _componentRepository.UniqueId, _setSteeringTargetDelegate);
            _setTargetEntityId = MessageDispatcher2.Instance.StartListening("SetAITarget", _componentRepository.UniqueId, _setAITargetDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetPos", this.UniqueId, _setPosId);
            MessageDispatcher2.Instance.StopListening("SetMaxSpeed", this.UniqueId, _setMaxSpeedId);

            //_componentRepository.StopListening("SetMaxSpeed", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetMaxSpeedToOriginal", this.UniqueId, _setMaxSpeedToOriginalId);
            //_componentRepository.StopListening("SetMaxSpeedToOriginal", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("GetFeetPosition", this.UniqueId, _getFeetPositionId);
            MessageDispatcher2.Instance.StopListening("SetPath", this.UniqueId, _setPathId);
            MessageDispatcher2.Instance.StopListening("StopMovement", this.UniqueId, _stopMovementId);
            //_componentRepository.StopListening("StopMovement", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetSteeringTarget", _componentRepository.UniqueId, _setSteeringTargetId);
            MessageDispatcher2.Instance.StopListening("SetAITarget", _componentRepository.UniqueId, _setTargetEntityId);
        }

        //------------------------------ Update ----------------------------------
        //
        //  Updates the vehicle's position from a series of steering behaviors
        //------------------------------------------------------------------------
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_physicsData.IsStopped)
            {
                Stop();
                return;
            }

            //_physicsData.Velocity += (_physicsData.InputForce / 50f);

            //if (_physicsData.DragForce != 0f)
            //{
            //    // Calculate the drag force and add it to Velocity.
            //    var normalizedVelocity = ((Vector2)_physicsData.Velocity).normalized;
            //    var currentDragForce = normalizedVelocity * -1f * _physicsData.DragForce / 50f;
            //    //if (currentDragForce )
            //    _physicsData.Velocity += (Vector2D)currentDragForce;
            //}

            // Update by Brian Karcher 9/26/2018
            // Having the RigidBody control the Velocity value. This allows us to take into account
            // collision physics automatically via said RigidBody.

            var force = Vector2.zero;
            //foreach (var affector in _physicsAffectorsList.Values)
            for (int i = 0; i < _physicsAffectorsList.Count; i++)
            {
                var affector = _physicsAffectorsList[i];
                if (affector.Enabled)
                    force += affector.CalculateForce();
                //newVelocity += affector.Velocity;
            }
            force = force * _physicsData.ForceMultiplier;
            var maxSpeed = _physicsData.MaxSpeed * _physicsData.MaxSpeedMultiplier;
            //if (force.magnitude > maxSpeed)
            //    force = force.normalized * maxSpeed;
            //_physicsData.Velocity = newVelocity;

            //_physicsData.ExternalVelocity += (_physicsData.ExternalForce / 50f);
            //_physicsData.InputVelocity += (_physicsData.InputForce / 50);
            //_physicsData.InputVelocity.Truncate(_physicsData.MaxSpeed);

            //update the position
            // This will be done by sending the Velocity to the MB, which will then update the
            // RigidBody's velocity.  This will give us automatic collision detection.

            //////////////////m_vPos += Velocity;
            //var newVelocity = _physicsData.Velocity + (_physicsData.ExternalVelocity) + _physicsData.InputVelocity;
            //var newVelocity = _physicsData.Velocity + (_physicsData.ExternalVelocity);
            //_physicsData.Velocity = newVelocity;

            //if (newVelocity.Length() > 2f)
            //{
            //    int i = 1;
            //}
            //_physicsData.

            var velocity = GetVelocity();
            //_previousVelocity = velocity;

            _physicsData.Velocity = velocity;
            // Cap the force added so it does not exceed max velocity
            //var forceTick = force / 50f;
            //var newMaxSpeed = (velocity + forceTick).magnitude;
            //if (newMaxSpeed > _physicsData.MaxSpeed)
            //{
            //    // How much we are over
            //    var adjustForceMagnitude = (newMaxSpeed - _physicsData.MaxSpeed) * 50f;
            //    var drag = Vector2.ClampMagnitude(force, adjustForceMagnitude);
            //    if (_rigidBody3D != null)
            //        _rigidBody3D.AddForce(-drag);
            //    if (_rigidBody2D != null)
            //        _rigidBody2D.AddForce(-drag);
            //}

            AddForce(force);
            
            if (velocity.sqrMagnitude < float.Epsilon)
            {
                velocity = Vector2.zero;
            }

            float speed = velocity.magnitude;  // test current object speed

            if (speed > maxSpeed)
            {
                float brakeSpeed = speed - maxSpeed; // calculate the speed decrease

                Vector3 normalisedVelocity = velocity.normalized;
                Vector3 brakeVelocity = normalisedVelocity * brakeSpeed * 50f; // make the brake Vector3 value
                //if (_rigidBody3D != null)
                //    rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
                AddForce(-brakeVelocity);
                //if (_rigidBody3D != null)
                //_rigidBody3D.drag = brakeSpeed * 50f;
            }
            else
            {
                //if (_rigidBody3D != null)
                //_rigidBody3D.drag = 0f;
            }

            //velocity = Vector2.ClampMagnitude(velocity, _physicsData.MaxSpeed * _physicsData.MaxSpeedMultiplier);
            //SetVelocity(velocity);

            //update the heading if the vehicle has a non zero velocity

            if (_autoUpdateHeading && velocity != Vector2.zero)
            {
                SetHeading(velocity);
            }
            UpdateZ();
        }

        //IEnumerator LateFixedUpdate()
        //{
        //    //while (true)
        //    //{

        //    //    float speed = velocity.magnitude;  // test current object speed
        //    //    if (speed > _physicsData.MaxSpeed)
        //    //    {
        //    //        float brakeSpeed = speed - _physicsData.MaxSpeed;  // calculate the speed decrease

        //    //        Vector3 normalisedVelocity = velocity.normalized;
        //    //        Vector3 brakeVelocity = normalisedVelocity * brakeSpeed * 50f;  // make the brake Vector3 value

        //    //        //rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
        //    //        if (_rigidBody3D != null)
        //    //            _rigidBody3D.AddForce(-brakeVelocity);
        //    //        if (_rigidBody2D != null)
        //    //            _rigidBody2D.AddForce(-brakeVelocity);
        //    //    }

        //    //    yield return new WaitForFixedUpdate();
        //    //}
        //}

        void OnDrawGizmos()
        {
            if (_steering == null)
                return;
            _steering.OnDrawGizmos();
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, _physicsData.BoundingRadius);
            if (_steering.Target != Vector2.zero)
            {
                Gizmos.color = new Color(0,0,255,0.5f);
                Gizmos.DrawSphere(_steering.Target.ToVector3(0f), 0.06f);
            }
            //Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
        }

        public void AddForce(Vector2 force)
        {
            if (_rigidBody2D != null)
                _rigidBody2D.AddForce(force);
            if (_rigidBody3D != null)
                _rigidBody3D.AddForce(force);
        }

        public void AddForce(Vector2 force, ForceMode forceMode)
        {
            if (_rigidBody2D != null)
                _rigidBody2D.AddForce(force);
            if (_rigidBody3D != null)
                _rigidBody3D.AddForce(force, forceMode);
        }

        public void SetSteeringTarget(Vector2D target)
        { 
            _steering.Target = target;
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.RotateObjectToDirection:
                    // If the object needs to physically rotate, rotate it to point to the new Direction
                    if (_physicsData.DirectionRotate)
                    {
                        var direction = (Direction) msg.ExtraInfo;
                        //var transform = GetEntityUI().GetTransform();
                        transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                            direction.GetDirectionAngle());
                    }
                    break;
                case Telegrams.StopMovement:
                    Stop();
                    break;
                case Telegrams.GetPhysicsData:
                    if (msg.Act != null)
                        msg.Act(_physicsData);
                    else
                    {
                        // Someone wants the physics data, so send it back to them
                        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, msg.SenderId,
                            Telegrams.SetPhysicsData, _physicsData);
                    }
                    break;
                case Telegrams.GetSteering:
                    //if (_steering == null)
                    //    SetupSteeringManager();
                    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, msg.SenderId,
                    //    Telegrams.SetSteering, _steering);
                    msg.Act(_steering);
                    break;
                case Telegrams.SetPos:
                    var pos = (Vector2D)msg.ExtraInfo;
                    SetWorldPos(pos - _physicsData._footOffset);
                    break;
                case Telegrams.GetPos:
                    msg.Act((Vector2D)GetWorldPos());
                    return true;
                case Telegrams.SetMaxSpeedMultiplier:
                    _physicsData.MaxSpeedMultiplier = (float)msg.ExtraInfo;
                    //_steeringPhysicsAffector.MaxSpeedMultiplier = (float)msg.ExtraInfo;
                    break;
            }
            return false;
        }

        public void SetupSteeringManager()
        {
            _steering?.Setup(this, transform);
            //var collisionComponent = 
            //if (_steering == null)
            //    _steering = new SteeringBehaviorManager(this, transform);
        }

        public ISteeringBehaviorManager GetSteering()
        {
            return _steering;
        }

        public CollisionComponent GetCollisonComponent()
        {
            return base._componentRepository.Components.GetComponent<CollisionComponent>();
        }

        private void UpdateZ()
        {
            float newZ;
            newZ = GetZOffset();
            var pos = GetLocalPos();
            SetLocalPos(new Vector3(pos.x, pos.y, newZ));
        }

        private float GetZOffset()
        {
            var gameConfig = GameDataController.Instance?.GetGameConfig();
            if (gameConfig == null)
                return 0f;
            float spriteZOffset = gameConfig.GlobalSpriteZOffset;
            return spriteZOffset + _physicsData.ZOffsetByLevel + _physicsData.ZDepthByY + _physicsData.ZOffset;
            //return _physicsData.OriginalZ + _physicsData.ZOffsetByLevel + _physicsData.ZDepthByY + _physicsData.ZOffset;
        }

        public void SetZOffsetByLevel(float offset)
        {
            _physicsData.ZOffsetByLevel = offset;
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            //_physicsData.Velocity = velocity;
            if (_rigidBody2D != null)
            {
                _previousVelocity = _rigidBody2D.velocity;
                _rigidBody2D.velocity = velocity;
            }
            if (_rigidBody3D != null)
            {
                _previousVelocity = _rigidBody3D.velocity;
                _rigidBody3D.velocity = velocity;
            }
            SetHeading(velocity);
        }

        public virtual void StopRigidbody()
        {
            //_physicsData.Velocity = velocity;
            if (_rigidBody2D != null)
            {
                _rigidBody2D.velocity = Vector3.zero;
                _rigidBody2D.angularVelocity = 0;
            }
            if (_rigidBody3D != null)
            {
                _rigidBody3D.velocity = Vector3.zero;
                _rigidBody3D.angularVelocity = Vector3.zero;
            }
        }

        public virtual void AddVelocity(Vector3 velocity)
        {
            if (_rigidBody2D != null)
                _rigidBody2D.velocity += (Vector2)velocity;
            if (_rigidBody3D != null)
                _rigidBody3D.velocity += velocity;
        }

        public void Stop()
        {
            SetVelocity(Vector2.zero);
            _physicsData.InputVelocity = Vector2.zero;
            //_physicsData.Velocity.SetToZero();
            if (_physicsAffectors == null)
                return;
            //foreach (var affector in _physicsAffectors.Values)
            for (int i = 0; i < _physicsAffectorsList.Count; i++)
            {
                var affector = _physicsAffectorsList[i];
                affector.Stop();
                affector.Force = Vector2.zero;
            }
            if (_rigidBody2D != null)
                _rigidBody2D.Sleep();
            if (_rigidBody3D != null)
                _rigidBody3D.Sleep();
        }

        //public Vector3 GetPos()
        //{
        //    return this.transform.localPosition;
        //}

        //public Vector3 GetWorldPos()
        //{
        //    return this.transform.position;
        //}

        public void SetLocalPos(Vector2 new_pos)
        {
            // Depth needs to stay the same
            this.transform.localPosition = new Vector3(new_pos.x, new_pos.y, this.transform.localPosition.z);
            //m_vPosition = new_pos;
        }
        public void SetLocalPos(Vector3 new_pos)
        {
            // Depth needs to stay the same
            this.transform.localPosition = new_pos;
            //m_vPosition = new_pos;
        }
        public void SetWorldPos(Vector2 new_pos)
        {
            // Depth needs to stay the same
            this.transform.position = new Vector3(new_pos.x, new_pos.y, this.transform.position.z);
            //m_vPosition = new_pos;
        }
        public void SetWorldPos(Vector3 new_pos)
        {
            // Depth needs to stay the same
            this.transform.position = new_pos;
            //m_vPosition = new_pos;
        }
        public Vector2 GetVelocity()
        {
            if (_rigidBody2D != null)
                return _rigidBody2D.velocity;
            if (_rigidBody3D != null)
                return _rigidBody3D.velocity;
            return Vector2.zero;
        }

        //public bool IsSpeedMaxedOut()
        //{
        //    return _steeringPhysicsAffector.MaxSpeed * _steeringPhysicsAffector.MaxSpeed >= _physicsData.Velocity.LengthSq();
        //}

        //--------------------------- RotateHeadingToFacePosition ---------------------
        //
        //  given a target position, this method rotates the entity's heading and
        //  side vectors by an amount not greater than m_dMaxTurnRate until it
        //  directly faces the target.
        //
        //  returns true when the heading is facing in the desired direction
        //-----------------------------------------------------------------------------
        public bool RotateHeadingToFacePosition(Vector2D target)
        {
            Vector2D toTarget = Vector2D.Vec2DNormalize(target - GetWorldPos());

            float dot = Vector2.Dot(_physicsData.Heading, toTarget);

            //some compilers lose acurracy so the value is clamped to ensure it
            //remains valid for the acos
            Mathf.Clamp(dot, -1f, 1f);

            //first determine the angle between the heading vector and the target
            float angle = Mathf.Acos(dot);

            //return true if the player is facing the target
            if (angle < 0.00001) return true;

            //clamp the amount to turn to the max turn rate
            if (angle > _physicsData._maxTurnRate) angle = _physicsData._maxTurnRate;

            //The next few lines use a rotation matrix to rotate the player's heading
            //vector accordingly
            C2DMatrix RotationMatrix = new C2DMatrix();
            RotationMatrix.Identity();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            Vector2D heading2D = _physicsData.Heading;
            RotationMatrix.Rotate(angle * heading2D.Sign(toTarget));
            RotationMatrix.TransformVector2Ds(_physicsData.Heading);
            RotationMatrix.TransformVector2Ds(GetVelocity());

            //finally recreate m_vSide
            var heading = _physicsData.Heading;
            _physicsData.Side = new Vector2(-heading.y, heading.x);

            return false;
        }

        //------------------------- SetHeading ----------------------------------------
        //
        //  first checks that the given heading is not a vector of zero length. If the
        //  new heading is valid this fumction sets the entity's heading and side 
        //  vectors accordingly
        //-----------------------------------------------------------------------------
        public void SetHeading(Vector2 new_heading)
        {
            if (new_heading == Vector2.zero)
            {
                return;
            }
            new_heading = new_heading.normalized;

            _physicsData.Heading = new_heading;

            //the side vector must always be perpendicular to the heading
            var heading = _physicsData.Heading;
            _physicsData.Side = new Vector2(-heading.y, heading.x);
        }

        //public virtual Vector2D GetFeetPosition()
        //{
        //    return GetPos() + _physicsData._footOffset;
        //}

        public virtual Vector2 GetFeetWorldPosition()
        {
            return transform.position + _physicsData._footOffset;
        }

        public virtual void SetFeetWorldPosition(Vector2 pos)
        {
            SetWorldPos(new Vector2(pos.x, pos.y) - (Vector2)_physicsData._footOffset);
        }

        public virtual void SetFeetWorldPosition(Vector3 pos)
        {
            SetWorldPos((Vector3)pos - _physicsData._footOffset.ToVector3(0f));
        }

        public virtual Vector3 GetFeetWorldPosition3()
        {
            return transform.position + _physicsData._footOffset.ToVector3(0f);
        }

        public virtual Vector3 GetFeetPosition3()
        {
            return (GetWorldPos() + _physicsData._footOffset).ToVector3(this.GetZOffset());
        }

        public BasicPhysicsData GetPhysicsData()
        {
            return _physicsData;
        }

        public BasicPhysicsData GetOriginalPhysicsData()
        {
            return _originalPhysicsData;
        }

        public IPhysicsAffector GetPhysicsAffector(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            // ehhhhh
            if (!_physicsAffectors.TryGetValue(name, out var affector))
            {
                affector = new PhysicsAffector();
                affector.Name = name;
                affector.MaxForce = 1f;
                affector.MaxSpeed = 1f;
                affector.Init(this);                
                _physicsAffectors.Add(name, affector);
                _physicsAffectorsList.Add(affector);
            }
            
            return affector;
        }

        public void SetPhysicsAffector(string name, IPhysicsAffector physicsAffector)
        {
            _physicsAffectors[name] = physicsAffector;
            if (!_physicsAffectorsList.Contains(physicsAffector))
                _physicsAffectorsList.Add(physicsAffector);
        }

        /// <summary>
        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
        /// may populate different parts of this object
        /// </summary>
        /// <param name="entityData"></param>
        //public override void Serialize(EntitySerializedData entityData)
        //{
        //    base.Serialize(entityData);
        //    SetupSteeringManager();
        //    //base.Serialize(ref entityData);
        //    _physicsData.SteeringBehaviourData = _steering.Serialize();
        //    base.SerializeComponent(entityData, _physicsData);
        //    //entityData.PhysicsData = _physicsData;
        //    //_physicsData = entityData.PhysicsData;

        //    //entityData.SteeringBehaviorData = _steering.Serialize();
        //}

        //public override void Deserialize(EntitySerializedData entityData)
        //{
        //    base.Deserialize(entityData);
        //    SetupSteeringManager();
        //    var data = base.DeserializeComponent<PhysicsData>(entityData);
        //    _physicsData.CopyFrom(data);            
        //    _steering.Deserialize(_physicsData.SteeringBehaviourData);
        //}

        public bool EntityInFOV(GameObject entity, Vector2 lookingVector, bool castRay, int obstacleLayerMask)
        {
            //var tileMap = GameObject.FindObjectOfType<tk2dTileMap>();

            //var layers = tileMap.Layers[0];
            //tileMap.GetTileIdAtPosition(transform.position, );

            //var isPositive = false;
            //AnimationComponent 
            //var facing = _animationComponent.GetFacingDirectionVector();

            // Look At care more about the facing direction than the heading.
            //Vector2D heading;
            //if (_directionToCheck == DirectionToCheck.FacingDirection)
            //    heading = _animationComponent.GetFacingDirectionVector();
            //else
            //    heading = _physicsComponent.GetPhysicsData().Heading;

            //var facingDirection = _animationComponent.GetFacingDirectionVector();
            //var heading = _physicsComponent.GetPhysicsData().Heading;
            //if (_aiComponent == null)
            //    throw new System.Exception("Could not locate AI Component.");
            //if (_aiComponent.Target == null)
            //    return false;
            var vectorToTarget = entity.transform.position - GetWorldPos();

            var angle = Vector2.Angle(lookingVector, vectorToTarget);

            // TODO Add a "sniff test" condition to tell if the target is close, regardless of angle

            // Within field of view?  Worthy of further consideration
            if (angle > FieldOfView)
                return false;

            // Using distance squared space because a square root is slow
            if ((GetWorldPos() - entity.transform.position).sqrMagnitude >= LineOfSight * LineOfSight)
                return false;

            // TODO Cast a ray to check for a clear line of sight
            if (!castRay)
                return true;

            //var otherBaseComponent = _aiComponent.Target.GetComponent<SpriteBaseComponent>();

            var target = entity.transform.position;

            //var raycastObjects = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId);

            return HasLineOfSight(target, obstacleLayerMask);

            //var hasExcludedColliders = raycastObjects.Any(i => _tagsThatCancelRaycast.Contains(i.collider.tag));

            //return !hasExcludedColliders;
        }

        public bool HasLineOfSight(Vector3 target, int obstacleLayerMask)
        {
            var currentPos = GetWorldPos();
            //var layerMask = 1 << _obstacleLayerMask;
            return (!UnityEngine.Physics.Raycast(currentPos, target - currentPos, (target - currentPos).magnitude, obstacleLayerMask));

        }

        public void SetKinematic(bool isKinematic)
        {
            if (_rigidBody2D != null)
                _rigidBody2D.isKinematic = isKinematic;
            if (_rigidBody3D != null)
                _rigidBody3D.isKinematic = isKinematic;
        }

        public bool GetKinematic()
        {
            if (_rigidBody2D != null)
                return _rigidBody2D.isKinematic;
            if (_rigidBody3D != null)
                return _rigidBody3D.isKinematic;
            return false;
        }

        public Vector3 GetPreviousVelocity()
        {
            return _previousVelocity;
        }

        public void SetPreviousVelocity(Vector3 velocity)
        {
            _previousVelocity = velocity;
        }
    }
}
