using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Launch Self")]
    public class LaunchSelf : AnimatorState
    {
        public override void Enter()
        {
            //if (_sprite == null)
            //    return;
            base.Enter();
            var altitudePhysicsComponent = _componentRepository.Components.GetComponent<AltitudePhysicsComponent>();
            var altitudeData = altitudePhysicsComponent.GetAltitudeData();
            altitudeData.IsAirborn = true;
            altitudeData.AirVelocity = altitudeData.JumpVelocity;
            //sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }
    }
}
