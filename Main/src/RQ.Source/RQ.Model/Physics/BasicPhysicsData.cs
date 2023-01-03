using RQ.Model.Physics.Data;
using RQ.Serialization;
using System;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public class BasicPhysicsData
    {
        public SteeringData SteeringData;
        public Vector2 InitialVelocity;

        [SerializeField]
        //public Vector2D Velocity;
        //public float Speed { get { return Velocity.Length(); } }
        public Vector2D Size;

        /// <summary>
        /// This field is read only, used for debugging and determining current velocity
        /// </summary>
        public Vector2 Velocity;

        //[HideInInspector]
        //a normalized vector pointing in the direction the entity is heading. 
        public Vector2 Heading;

        //[HideInInspector]
        //a vector perpendicular to the heading vector
        public Vector2 Side;

        public Vector2D Offset;

        [SerializeField]
        private float Mass = 1f;

        //the maximum speed this entity may travel at.
        [SerializeField]
        public float MaxSpeed = 1f;
        [SerializeField]
        public float OriginalMaxSpeed = 1f;

        [SerializeField]
        public float MaxSpeedMultiplier = 1f;
        /// <summary>
        /// Used for obstacle avoidance
        /// </summary>
        public float BoundingRadius;

        public float MaxDistanceToTarget;

        [SerializeField]
        private bool _directionRotate = false;
        public bool DirectionRotate { get { return _directionRotate; } set { _directionRotate = value; } }
        //public bool 

        public float DamagedBounceForce = 1f;
        public float DamageDrag = 1f;

        //public PhysicsAffector[] physicsAffector;


        //[SerializeField]
        //private Vector2D _externalForce;
        //[HideInInspector]
        //public Vector2D ExternalForce { get { return _externalForce; } set { _externalForce = value; } }

        //[SerializeField]
        //private Vector2D _inputForce;
        //[HideInInspector]
        //public Vector2D InputForce { get { return _inputForce; } set { _inputForce = value; } }
        //[HideInInspector]
        [SerializeField]
        private Vector2D _externalVelocity;
        private Vector2D ExternalVelocity { get { return _externalVelocity; } set { _externalVelocity = value; } }

        //[SerializeField]
        private Vector2 _inputVelocity;
        public Vector2 InputVelocity { get { return _inputVelocity; } set { _inputVelocity = value; } }

        //[HideInInspector]
        public float OriginalZ = -0.5f;
        //public float ZOffset = 0.0f;
        [HideInInspector]
        //public float _zOffset;

        [SerializeField]
        private Direction _headingDirection = Direction.Right;


        /// <summary>
        /// the maximum force this entity can produce to power itself 
        /// (think rockets and thrust)
        /// </summary>
        [SerializeField]
        private float MaxForce = 1f;

        public float ForceMultiplier = 1f;

        [SerializeField]
        private float _friction;
        //[HideInInspector]
        //public float Friction { get { return _friction; } set { _friction = value; } }

        //the maximum rate (radians per second)this vehicle can rotate      
        [HideInInspector]
        public float _maxTurnRate = 6f;

        [SerializeField]
        public Vector2D _footOffset;

        [SerializeField]
        private GameObject _foot;
        public GameObject Foot { get => _foot;  set => _foot = value; }

        [SerializeField]
        private float _fieldOfView;
        public float FieldOfView { get { return _fieldOfView; } set { _fieldOfView = value; } }

        [SerializeField]
        private float _lineOfSight;
        public float LineOfSight { get { return _lineOfSight; } set { _lineOfSight = value; } }

        [SerializeField]
        private float _dragForce;
        public float DragForce { get { return _dragForce; } set { _dragForce = value; } }

        //[HideInInspector]
        //public float InitialZ { get; set; }

        //public float CurrentZ;

        public float ZDepthByY;

        public float ZOffsetByLevel;

        public float ZOffset;

        public SteeringBehaviorData SteeringBehaviourData { get; set; }

        public bool IsStopped { get; set; }

        public virtual void CopyFrom(BasicPhysicsData from)
        {
            this._directionRotate = from._directionRotate;
            //this._externalForce = from._externalForce;
            //this._externalVelocity = from._externalVelocity;
            this._fieldOfView = from._fieldOfView;
            this._footOffset = from._footOffset;
            this._friction = from._friction;
            this._headingDirection = from._headingDirection;
            //this._inputForce = from._inputForce;
            //this._inputVelocity = from._inputVelocity;
            this._lineOfSight = from._lineOfSight;
            this._maxTurnRate = from._maxTurnRate;
            this.DamagedBounceForce = from.DamagedBounceForce;
            this.DamageDrag = from.DamageDrag;
            this.Heading = from.Heading;
            this.Mass = from.Mass;
            this.MaxForce = from.MaxForce;
            this.MaxSpeed = from.MaxSpeed;
            this.MaxSpeedMultiplier = from.MaxSpeedMultiplier;
            this.OriginalZ = from.OriginalZ;
            this.Side = from.Side;
            this.Size = from.Size;
            //this.Velocity = from.Velocity;
            this.ZDepthByY = from.ZDepthByY;
            this.ZOffsetByLevel = from.ZOffsetByLevel;
            this.SteeringBehaviourData = from.SteeringBehaviourData;
        }
    }
}
