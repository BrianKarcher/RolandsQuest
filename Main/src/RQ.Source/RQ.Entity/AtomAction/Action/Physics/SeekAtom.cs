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
    public class SeekAtom : AtomActionBase
    {
        private SteeringBehaviorManager _steering;
        public string _physicsComponentName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);
            if (physicsComponent == null)
            {
                Debug.Log("(SeekAtom) Unable to locate physics component " + _physicsComponentName);
                return;
            }
            _steering = physicsComponent.GetSteering() as SteeringBehaviorManager;
            _steering.TurnOn(Physics.behavior_type.seek);
        }

        public override void End()
        {
            _steering.TurnOff(Physics.behavior_type.seek);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
