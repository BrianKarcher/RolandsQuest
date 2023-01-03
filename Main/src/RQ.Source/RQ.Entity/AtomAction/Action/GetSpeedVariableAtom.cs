using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class GetSpeedAtom : AtomActionBase
    {
        private PhysicsComponent _physicsComponent;
        public float Speed;
        BasicPhysicsData _physicsData;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _physicsData = _physicsComponent.GetPhysicsData();            
        }

        public override AtomActionResults OnUpdate()
        {
            Speed = _physicsComponent.GetVelocity().magnitude;
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
