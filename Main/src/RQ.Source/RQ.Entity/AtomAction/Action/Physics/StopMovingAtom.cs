using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    public class StopMovingAtom : AtomActionBase
    {
        [SerializeField]
        [UniqueIdentifier]
        public string _uniqueId;
        public string _physicsAffector;
        //public bool _firstFrameOnly = true;
        private PhysicsComponent _physicsComponent;
        private AltitudePhysicsComponent _altitudePhysicsComponent;
        private Rigidbody _rigidbody;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            if (_physicsComponent == null)
            {
                _rigidbody = _entity.GetComponent<Rigidbody>();
            }
            Tick();
        }

        public void Tick()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            if (String.IsNullOrEmpty(_physicsAffector))
            {
                _physicsComponent.Stop();
                //if (!_firstFrameOnly)
                //    _physicsComponent.GetPhysicsData().IsStopped = true;
            }
            else
            {
                var affector = _physicsComponent.GetPhysicsAffector(_physicsAffector);
                affector.Stop();
            }
            _altitudePhysicsComponent?.Stop();
        }

        public override void End()
        {
            if (_physicsComponent != null)
            {
                _physicsComponent.GetPhysicsData().IsStopped = false;
            }
            base.End();
        }

        public override AtomActionResults OnUpdate()
        {
            Tick();
            return AtomActionResults.Success;
        }

        public void StopPhysics(bool stop)
        {
            if (_physicsComponent != null)
            {
                _physicsComponent.GetPhysicsData().IsStopped = stop;
            }
        }
    }
}
