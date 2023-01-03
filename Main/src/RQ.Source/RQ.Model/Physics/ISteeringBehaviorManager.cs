using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public interface ISteeringBehaviorManager
    {
        IBasicPhysicsComponent GetTargetAgent1();
        //CollisionComponent CollisionComponent { get; set; }
        IBasicPhysicsComponent Entity { get; set; }
        IPhysicsAffector SteeringPhysicsAffector { get; set; }
        //PhysicsComponent TargetAgent2 { get; set; }
        float ViewDistance { get; set; }
        ISteeringBehavior GetSteeringBehavior(behavior_type behaviortype);

        Vector2D Calculate();
        void CalculateSteeringModes();
        //void Deserialize(SteeringBehaviorData data);
        float ForwardComponent();
        Vector2D GetForce();
        //ISteeringBehavior GetSteeringBehavior(behavior_type behaviortype);
        bool IsOn(behavior_type bt);
        void OnDrawGizmos();
        //SteeringBehaviorData Serialize();
        //void SetSummingMethod(SteeringBehaviorManager.summing_method sm);
        void Setup(IBasicPhysicsComponent entity, Transform transform);
        float SideComponent();
        void TurnOff(behavior_type behaviortype);
        void TurnOn(behavior_type behaviortype);
        Vector2 GetTarget();
        Vector3 GetTarget3();
        Vector3 GetOffset();
    }
}