using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class FleeAtom : AtomActionBase
    {
        private SteeringBehaviorManager _steering;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _steering = physicsComponent.GetSteering() as SteeringBehaviorManager;
            _steering.TurnOn(Physics.behavior_type.flee);
        }

        public override void End()
        {
            _steering.TurnOff(Physics.behavior_type.flee);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
