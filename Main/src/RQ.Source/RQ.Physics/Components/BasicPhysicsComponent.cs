//using RQ.Common.Components;
//using RQ.Enums;
//using RQ.Messaging;
//using RQ.Model.Interfaces;
//using RQ.Model.Physics;
//using RQ.Physics.SteeringBehaviors;
//using RQ.Serialization;
//using System;
//using UnityEngine;

//namespace RQ.Physics.Components
//{
//    [Obsolete]
//    [AddComponentMenu("RQ/Components/Physics/Basic Physics")]
//    public class BasicPhysicsComponent : ComponentPersistence<BasicPhysicsComponent>, IBasicPhysicsComponent
//    {
//        [SerializeField]
//        protected BasicPhysicsData _basicPhysicsData;
//        [SerializeField]
//        private bool _autoUpdateHeading = true;

//        //the steering behavior class
//        [SerializeField]
//        protected SteeringBehaviorManager _steering;

//        public float FieldOfView { get { return _basicPhysicsData.FieldOfView; } set { _basicPhysicsData.FieldOfView = value; } }

//        public float LineOfSight { get { return _basicPhysicsData.LineOfSight; } set { _basicPhysicsData.LineOfSight = value; } }

//        private Rigidbody2D _rigidBody;
//        //private Rigidbody _rigidBody3D;

//        public override void Awake()
//        {
//            Debug.LogError($"{_componentRepository.name} contains outdated BasicPhysicsComponent");
//            base.Awake();
//            if (_autoUpdateHeading)
//            {
//                _basicPhysicsData.Heading = Vector2D.Vec2DNormalize(_basicPhysicsData.Velocity);
//                _basicPhysicsData.Side = _basicPhysicsData.Heading.Perp();
//            }
//            //_physicsData.OriginalZ = GetPos().z;
//            //_physicsData._zOffset = 0f;
            
//            //_rigidBody3D = GetComponent<Rigidbody>();
            
//        }

//        public override void Init()
//        {
//            base.Init();
//            _rigidBody = GetComponent<Rigidbody2D>();
//            SetupSteeringManager();
//        }

//        public override void Start()
//        {
//            base.Start();
//            if (!Application.isPlaying)
//                return;
//            _steering.CollisionComponent = GetCollisonComponent();
//        }

//        //public override void OnEnable()
//        //{
//        //    base.OnEnable();
//        //    if (!Application.isPlaying)
//        //        return;
//        //}

//        //public override void OnDisable()
//        //{
//        //    base.OnDisable();
//        //    if (!Application.isPlaying)
//        //        return;
//        //}

//        public override void StartListening()
//        {
//            base.StartListening();
//            MessageDispatcher2.Instance.StartListening("SetPos", this.UniqueId, (data) =>
//            {
//                SetWorldPos((Vector2D)data.ExtraInfo);
//            });
//            MessageDispatcher2.Instance.StartListening("GetFeetPosition", this.UniqueId, (data) =>
//            {
//                MessageDispatcher2.Instance.DispatchMsg("SetFeetPosition", 0f, this.UniqueId, data.SenderId,
//                    GetFeetPosition());
//            });
//            _componentRepository.StartListening("SetMaxSpeed", this.UniqueId, (data) =>
//            {
//                //_basicPhysicsData.MaxSpeed = Convert.ToSingle(data.ExtraInfo);
//            });
//            _componentRepository.StartListening("StopMovement", this.UniqueId, (data) =>
//            {
//                Stop();
//            });
//        }

//        public override void StopListening()
//        {
//            base.StopListening();
//            MessageDispatcher2.Instance.StopListening("SetPos", this.UniqueId, -1);
//            MessageDispatcher2.Instance.StopListening("GetFeetPosition", this.UniqueId, -1);
//            _componentRepository.StopListening("SetMaxSpeed", this.UniqueId);
//            _componentRepository.StopListening("StopMovement", this.UniqueId);
//        }

//        //------------------------------ Update ----------------------------------
//        //
//        //  Updates the vehicle's position from a series of steering behaviors
//        //------------------------------------------------------------------------
//        //public override void FixedUpdate()
//        //{
//        //    base.FixedUpdate();

//        //    //base.FixedUpdate();
//        //    //keep a record of its old position so we can update its cell later
//        //    //in this method
//        //    //Vector2D OldPos = GetPos();

//        //    Vector2D acceleration = CalculateSteeringAcceleration();

//        //    //update velocity
//        //    _basicPhysicsData.Velocity += acceleration;

//        //    _basicPhysicsData.Velocity += (_basicPhysicsData.InputForce / 50);

//        //    //make sure vehicle does not exceed maximum velocity
//        //    _basicPhysicsData.Velocity.Truncate(_basicPhysicsData.MaxSpeed * _basicPhysicsData.MaxSpeedMultiplier);
//        //    if (_basicPhysicsData.Velocity.LengthSq() < .001f)
//        //        _basicPhysicsData.Velocity = Vector2D.Zero();

//        //    _basicPhysicsData.ExternalVelocity += (_basicPhysicsData.ExternalForce / 50f);
//        //    //_physicsData.InputVelocity += (_physicsData.InputForce / 50);
//        //    //_physicsData.InputVelocity.Truncate(_physicsData.MaxSpeed);

//        //    //update the position
//        //    // This will be done by sending the Velocity to the MB, which will then update the
//        //    // RigidBody's velocity.  This will give us automatic collision detection.

//        //    //////////////////m_vPos += Velocity;
//        //    //var newVelocity = _physicsData.Velocity + (_physicsData.ExternalVelocity) + _physicsData.InputVelocity;
//        //    var newVelocity = _basicPhysicsData.Velocity + (_basicPhysicsData.ExternalVelocity);

//        //    //if (_rigidBody3D != null)
//        //    //    _rigidBody3D.velocity = newVelocity.ToVector3(0f);
//        //    if (_rigidBody != null)
//        //        _rigidBody.velocity = newVelocity;
//        //    else
//        //    {
//        //        throw new Exception(this.name + " has no 2D Rigid Body");
//        //    }

//        //    //update the heading if the vehicle has a non zero velocity
//        //    if (_autoUpdateHeading && !_basicPhysicsData.Velocity.isZero())
//        //    {
//        //        _basicPhysicsData.Heading = Vector2D.Vec2DNormalize(_basicPhysicsData.Velocity);

//        //        _basicPhysicsData.Side = _basicPhysicsData.Heading.Perp();
//        //    }
//        //    UpdateZ();
//        //}

//        //private Vector2D CalculateSteeringAcceleration()
//        //{
//        //    Vector2D SteeringForce;

//        //    //calculate the combined force from each steering behavior in the 
//        //    //sprite's list
//        //    SteeringForce = _steering.Calculate();

//        //    //if (_basicPhysicsData.Friction > float.Epsilon)
//        //    //    SteeringForce += CalculateFrictionForce();

//        //    //SteeringForce += GetPhysicsData().ExternalForce;

//        //    //Acceleration = Force/Mass
//        //    Vector2D acceleration = SteeringForce / _basicPhysicsData.Mass;
//        //    return acceleration;
//        //}

//        //private Vector2D CalculateFrictionForce()
//        //{
//        //    if (_basicPhysicsData.Velocity.Length() < _basicPhysicsData.Friction / 50)
//        //        return _basicPhysicsData.Velocity * -1;

//        //    return Vector2D.Vec2DNormalize(_basicPhysicsData.Velocity) * _basicPhysicsData.Friction / -50;
//        //}

//        public override bool HandleMessage(Telegram msg)
//        {
//            if (base.HandleMessage(msg))
//                return true;

//            switch (msg.Msg)
//            {
//                case Telegrams.RotateObjectToDirection:
//                    // If the object needs to physically rotate, rotate it to point to the new Direction
//                    if (_basicPhysicsData.DirectionRotate)
//                    {
//                        var direction = (Direction) msg.ExtraInfo;
//                        transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
//                            direction.GetDirectionAngle());
//                    }
//                    break;
//                case Telegrams.StopMovement:
//                    Stop();
//                    break;
//                case Telegrams.GetPhysicsData:
//                    if (msg.Act != null)
//                        msg.Act(_basicPhysicsData);
//                    else
//                    {
//                        // Someone wants the physics data, so send it back to them
//                        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, msg.SenderId,
//                            Telegrams.SetPhysicsData, _basicPhysicsData);
//                    }
//                    break;
//                case Telegrams.GetSteering:
//                    msg.Act(_steering);
//                    break;
//                case Telegrams.SetPos:
//                    var pos = (Vector2D)msg.ExtraInfo;
//                    SetWorldPos(pos - _basicPhysicsData._footOffset);
//                    break;
//                case Telegrams.GetPos:
//                    msg.Act((Vector2D)GetPos());
//                    return true;
//                case Telegrams.SetMaxSpeedMultiplier:
//                    //_basicPhysicsData.MaxSpeedMultiplier = (float)msg.ExtraInfo;
//                    break;
//            }
//            return false;
//        }

//        public void SetupSteeringManager()
//        {
//            _steering.Setup(this, transform);
//            //if (_steering == null)
//            //    _steering = new SteeringBehaviorManager(this, transform);
//        }

//        public ISteeringBehaviorManager GetSteering()
//        {
//            return _steering;
//        }

//        public CollisionComponent GetCollisonComponent()
//        {
//            return base._componentRepository.Components.GetComponent<CollisionComponent>();
//        }

//        private void UpdateZ()
//        {
//            float newZ;
//            newZ = _basicPhysicsData.OriginalZ + _basicPhysicsData.ZOffsetByLevel + 
//                _basicPhysicsData.ZDepthByY;
//            var pos = GetPos();
//            SetWorldPos(new Vector3(pos.x, pos.y, newZ));
//        }

//        public void SetZOffsetByLevel(float offset)
//        {
//            _basicPhysicsData.ZOffsetByLevel = offset;
//        }

//        public virtual bool SetVelocity(Vector3 velocity)
//        {
//            _basicPhysicsData.Velocity = velocity;
//            if (_rigidBody != null)
//                _rigidBody.velocity = velocity;
//            return true;
//        }

//        public void Stop()
//        {
//            _basicPhysicsData.Velocity.SetToZero();
//        }

//        public Vector3 GetPos()
//        {
//            return this.transform.position;
//        }
//        public void SetLocalPos(Vector2D new_pos)
//        {
//            // Depth needs to stay the same
//            this.transform.localPosition = new_pos.ToVector3(this.transform.localPosition.z);
//            //m_vPosition = new_pos;
//        }
//        public void SetLocalPos(Vector3 new_pos)
//        {
//            // Depth needs to stay the same
//            this.transform.localPosition = new_pos;
//            //m_vPosition = new_pos;
//        }
//        public void SetWorldPos(Vector2 new_pos)
//        {
//            // Depth needs to stay the same
//            this.transform.position = new Vector3(new_pos.x, new_pos.y, this.transform.position.z);
//            //m_vPosition = new_pos;
//        }
//        public void SetWorldPos(Vector3 new_pos)
//        {
//            // Depth needs to stay the same
//            this.transform.position = new_pos;
//            //m_vPosition = new_pos;
//        }

//        public bool IsSpeedMaxedOut()
//        {
//            //return _basicPhysicsData.MaxSpeed * _basicPhysicsData.MaxSpeed >= 
//            //    _basicPhysicsData.Velocity.LengthSq();
//            return false;
//        }

//        //--------------------------- RotateHeadingToFacePosition ---------------------
//        //
//        //  given a target position, this method rotates the entity's heading and
//        //  side vectors by an amount not greater than m_dMaxTurnRate until it
//        //  directly faces the target.
//        //
//        //  returns true when the heading is facing in the desired direction
//        //-----------------------------------------------------------------------------
//        public bool RotateHeadingToFacePosition(Vector2D target)
//        {
//            Vector2D toTarget = Vector2D.Vec2DNormalize(target - GetPos());

//            float dot = _basicPhysicsData.Heading.Dot(toTarget);

//            //some compilers lose acurracy so the value is clamped to ensure it
//            //remains valid for the acos
//            Mathf.Clamp(dot, -1f, 1f);

//            //first determine the angle between the heading vector and the target
//            float angle = Mathf.Acos(dot);

//            //return true if the player is facing the target
//            if (angle < 0.00001) return true;

//            //clamp the amount to turn to the max turn rate
//            if (angle > _basicPhysicsData._maxTurnRate) angle = _basicPhysicsData._maxTurnRate;

//            //The next few lines use a rotation matrix to rotate the player's heading
//            //vector accordingly
//            C2DMatrix RotationMatrix = new C2DMatrix();

//            //notice how the direction of rotation has to be determined when creating
//            //the rotation matrix
//            RotationMatrix.Rotate(angle * _basicPhysicsData.Heading.Sign(toTarget));
//            RotationMatrix.TransformVector2Ds(_basicPhysicsData.Heading);
//            RotationMatrix.TransformVector2Ds(_basicPhysicsData.Velocity);

//            //finally recreate m_vSide
//            _basicPhysicsData.Side = _basicPhysicsData.Heading.Perp();

//            return false;
//        }

//        //------------------------- SetHeading ----------------------------------------
//        //
//        //  first checks that the given heading is not a vector of zero length. If the
//        //  new heading is valid this fumction sets the entity's heading and side 
//        //  vectors accordingly
//        //-----------------------------------------------------------------------------
//        public void SetHeading(Vector2D new_heading)
//        {
//            if ((new_heading.LengthSq() - 1.0) < 0.00001)
//                throw new Exception("Heading has a length of zero");

//            _basicPhysicsData.Heading = new_heading;

//            //the side vector must always be perpendicular to the heading
//            _basicPhysicsData.Side = _basicPhysicsData.Heading.Perp();
//        }

//        public virtual Vector2D GetFeetPosition()
//        {
//            return GetPos() + _basicPhysicsData._footOffset;
//        }

//        public virtual Vector2 GetFeetWorldPosition()
//        {
//            return transform.position + _basicPhysicsData._footOffset;
//        }

//        public virtual Vector3 GetFeetWorldPosition3()
//        {
//            return transform.position + _basicPhysicsData._footOffset.ToVector3(0f);
//        }

//        public BasicPhysicsData GetPhysicsData()
//        {
//            return _basicPhysicsData;
//        }

//        public IPhysicsAffector GetPhysicsAffector(string name)
//        {
//            // ehhhhh
//            throw new NotImplementedException();

//            return null;
//        }

//        public void SetPhysicsAffector(string name, IPhysicsAffector physicsAffector)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
//        /// may populate different parts of this object
//        /// </summary>
//        /// <param name="entityData"></param>
//        //public override void Serialize(EntitySerializedData entityData)
//        //{
//        //    base.Serialize(entityData);
//        //    SetupSteeringManager();
//        //    _basicPhysicsData.SteeringBehaviourData = _steering.Serialize();
//        //    base.SerializeComponent(entityData, _basicPhysicsData);
//        //}

//        //public override void Deserialize(EntitySerializedData entityData)
//        //{
//        //    base.Deserialize(entityData);
//        //    SetupSteeringManager();
//        //    var data = base.DeserializeComponent<BasicPhysicsData>(entityData);
//        //    if (data == null)
//        //        throw new Exception("Basic Physics Data is null in " + entityData.Name);
//        //    _basicPhysicsData.CopyFrom(data);

//        //    _steering.Deserialize(_basicPhysicsData.SteeringBehaviourData);
//        //}
//    }
//}
