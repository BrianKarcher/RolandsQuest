using RQ.Physics;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Custom/Flying Racoon Glide")]
    public class FlyingRacoonGlideState : SimpleMovementToRandomTargetsState
    {
        public override void FixedUpdate()
        {
            // Once the bottom of the arc is reached, keep it at that altitude
            if (_altitudeData.AirVelocity.y > 0)
            {
                _altitudeData.AirVelocity = Vector2D.Zero();
                _altitudeData.Gravity = Vector2D.Zero();
            }
            base.FixedUpdate();
        }
    }
}
