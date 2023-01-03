using RQ.Model.Interfaces;
using UnityEngine;

namespace RQ.Model.Physics
{
    public interface IPhysicsAffector
    {
        Vector2 CalculateForce();
        void Stop();
        //void Update();
        //Vector2 Velocity { get; set; }
        //Vector2 OriginalVelocity { get; }
        Vector2 Force { get; set; }
        float MaxForce { get; set; }
        float OriginalMaxForce { get; set; }
        float MaxSpeed { get; set; }
        bool Enabled { get; set; }
        float OriginalMaxSpeed { get; }
        float Mass { get; set; }
        string Name { get; set; }
        void Init(IBasicPhysicsComponent physicsComponent);
    }
}