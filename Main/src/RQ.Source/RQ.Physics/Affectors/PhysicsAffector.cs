using RQ.Model.Interfaces;
using System;
using UnityEngine;

namespace RQ.Model.Physics
{
    [Serializable]
    public class PhysicsAffector : IPhysicsAffector
    {
        //[SerializeField]
        //protected Vector2 _velocity;
        //public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
        //[SerializeField]
        //protected Vector2 _originaVelocity;
        //public Vector2 OriginalVelocity { get { return _originaVelocity; } }
        [SerializeField]
        protected Vector2 _force;
        public Vector2 Force { get { return _force; } set { _force = value; } }
        [SerializeField]
        protected float _maxForce = 1f;
        public float MaxForce { get { return _maxForce; } set { _maxForce = value; } }
        [SerializeField]
        protected float _originalMaxForce = 1f;
        public float OriginalMaxForce { get { return _originalMaxForce; } set { _originalMaxForce = value; } }
        //the maximum speed this entity may travel at.
        [SerializeField]
        protected float _maxSpeed = 1f;
        public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
        //[SerializeField]
        protected float _originalMaxSpeed = 1f;
        public float OriginalMaxSpeed { get { return _originalMaxSpeed; } }
        //[SerializeField]
        //protected float _maxSpeedMultiplier = 1f;
        //public float MaxSpeedMultiplier { get { return _maxSpeedMultiplier; } set { _maxSpeedMultiplier = value; } }

        [SerializeField]
        protected float _mass;
        public float Mass { get { return _mass; } set { _mass = value; } }

        [SerializeField]
        protected bool _enabled = true;
        public bool Enabled { get { return _enabled; } set { _enabled = value; } }

        public string Name { get; set; }

        public virtual void Init(IBasicPhysicsComponent physicsComponent)
        {
            _originalMaxSpeed = _maxSpeed;
            //_originaVelocity = _velocity;
            _originalMaxForce = _maxForce;
            Enabled = true;
        }

        //public virtual void Update()
        //{
        //    if (!Enabled)
        //        return;

        //    Vector2 acceleration = CalculateAcceleration();

        //    //update velocity
        //    // This gets called 50 times per second, so divide acceleration by 50.
        //    _velocity += (acceleration / 50f);

        //    //make sure vehicle does not exceed maximum velocity
        //    _velocity = Vector2.ClampMagnitude(_velocity, _maxSpeed * _maxSpeedMultiplier);
        //    if (_velocity.sqrMagnitude < float.Epsilon)
        //        _velocity = Vector2.zero;
        //}

        public virtual Vector2 CalculateForce()
        {
            return Vector2.ClampMagnitude(_force, _maxForce);
        }

        public void Stop()
        {
            //_velocity = Vector2.zero;
            _force = Vector2.zero;
        }
    }
}
