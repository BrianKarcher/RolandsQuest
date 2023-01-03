using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetAirVelocityAtom : AtomActionBase
    {
        private Vector2 AirVelocity;
        public bool IsAirborn;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var altitudePhysicsComponent = entity.Components.GetComponent<AltitudePhysicsComponent>();
            if (altitudePhysicsComponent == null)
            {
                Debug.Log(entity.name + " has no Altitude Physics Component");
                return;
            }
            var physicsData = altitudePhysicsComponent.GetAltitudeData();
            //if (_resetToOriginal)
            //    _speed = physicsData.OriginalMaxSpeed;
            //physicsData.MaxSpeed = _speed;
            physicsData.AirVelocity = AirVelocity;
            physicsData.IsAirborn = IsAirborn;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetVelocity(Vector2 velocity)
        {
            AirVelocity = velocity;
        }
    }
}
