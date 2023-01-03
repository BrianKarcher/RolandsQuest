using RQ.Common;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Model.Interfaces
{
    public interface IBasicPhysicsComponent : IComponentBase
    {
        BasicPhysicsData GetPhysicsData();
        Vector2 GetVelocity();
        void AddVelocity(Vector3 velocity);
        //Vector2D GetFeetPosition();
        Vector2 GetFeetWorldPosition();
        Vector3 GetFeetWorldPosition3();
        Vector3 GetLocalPos();
        Vector3 GetWorldPos();
        Vector2 GetWorldPos2D();
        void SetWorldPos(Vector2 new_pos);
        void SetWorldPos(Vector3 new_pos);
        void Stop();
        //Transform transform { get; }
        ISteeringBehaviorManager GetSteering();
        IPhysicsAffector GetPhysicsAffector(string name);
        void SetPhysicsAffector(string name, IPhysicsAffector physicsAffector);
    }
}
