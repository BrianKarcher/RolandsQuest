using RQ.Model.Interfaces;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Model.Physics
{
    [Serializable]
    public class SteeringPhysicsAffector : PhysicsAffector
    {
        private IBasicPhysicsComponent _physicsComponent;
        public float Friction;
        public float ForceMultiplier = 1f;


        public override void Init(IBasicPhysicsComponent physicsComponent)
        {
            base.Init(physicsComponent);
            _physicsComponent = physicsComponent;
        }

        public override Vector2 CalculateForce()
        {
            Vector2D SteeringForce;

            //calculate the combined force from each steering behavior in the 
            //sprite's list
            var steering = _physicsComponent.GetSteering();
            SteeringForce = steering.Calculate();
            SteeringForce *= ForceMultiplier;
            //if (Friction > float.Epsilon)
            //    SteeringForce += CalculateFrictionForce();

            //SteeringForce += GetPhysicsData().ExternalForce;

            //Acceleration = Force/Mass
            //Vector2D acceleration = SteeringForce / Mass;
            return Vector2.ClampMagnitude(SteeringForce, _maxForce);
        }

        //private Vector2D CalculateFrictionForce()
        //{
        //    if (_velocity.magnitude < Friction / 50f)
        //        return _velocity * -1f;
        //    //if (_physicsData.Friction < _physicsData.Velocity.Length())
        //    //    return _physicsData.Velocity / -50;
        //    //else
        //    //    return _physicsData.Friction / -50;

        //    return Vector2D.Vec2DNormalize(_velocity) * Friction / -50f;
        //}
    }
}
