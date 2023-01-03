using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetGravityAtom : AtomActionBase
    {
        public Vector2 Gravity;

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
            physicsData.Gravity = Gravity;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
