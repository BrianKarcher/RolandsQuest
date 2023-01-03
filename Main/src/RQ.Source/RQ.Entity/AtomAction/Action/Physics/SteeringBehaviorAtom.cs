using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SteeringBehaviorAtom : AtomActionBase
    {
        private SteeringBehaviorManager _steering;

        public enum SteeringBehaviors
        {
            Seek = 0,
            Arrive = 1,
            Pursuit = 2,
            Flee = 3,
            Evade = 4,
            Interpose = 5,
            FollowPath = 6,
            Wander = 7,
            Hide = 8,
            WallAvoidance = 9,
            OffsetPursuit = 10,
            ObstacleAvoidance = 11,
            RadiusClamp = 12
        }

        public SteeringBehaviors SteeringBehavior;
        private behavior_type behavior_Type;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _steering = physicsComponent.GetSteering() as SteeringBehaviorManager;
            behavior_Type = GetBehavior();
            _steering.TurnOn(behavior_Type);
        }

        public override void End()
        {
            _steering.TurnOff(behavior_Type);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private behavior_type GetBehavior()
        {
            switch (SteeringBehavior)
            {
                case SteeringBehaviors.Arrive:
                    return behavior_type.arrive;
                case SteeringBehaviors.Evade:
                    return behavior_type.evade;
                case SteeringBehaviors.Flee:
                    return behavior_type.flee;
                case SteeringBehaviors.FollowPath:
                    return behavior_type.follow_path;
                case SteeringBehaviors.Hide:
                    return behavior_type.hide;
                case SteeringBehaviors.Interpose:
                    return behavior_type.interpose;
                case SteeringBehaviors.ObstacleAvoidance:
                    return behavior_type.obstacle_avoidance;
                case SteeringBehaviors.OffsetPursuit:
                    return behavior_type.offset_pursuit;
                case SteeringBehaviors.Pursuit:
                    return behavior_type.pursuit;
                case SteeringBehaviors.Seek:
                    return behavior_type.seek;
                case SteeringBehaviors.WallAvoidance:
                    return behavior_type.wall_avoidance;
                case SteeringBehaviors.Wander:
                    return behavior_type.wander;
                case SteeringBehaviors.RadiusClamp:
                    return behavior_type.radius_clamp;
                default:
                    return behavior_type.none;
            }
        }
    }
}
