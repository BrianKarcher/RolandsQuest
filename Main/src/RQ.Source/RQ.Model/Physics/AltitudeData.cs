using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Model.Physics
{
    [Serializable]
    public class  AltitudeData
    {
        public Vector2D Altitude;
        public bool IsAirborn;
        [HideInInspector]
        public Vector2D AirVelocity;
        public Vector2D JumpVelocity;
        public Vector2D Gravity;
        public bool StopWhenHitGround;

        public void CopyTo(AltitudeData altitudeData)
        {
            altitudeData.Altitude = this.Altitude;
            altitudeData.IsAirborn = this.IsAirborn;
            altitudeData.AirVelocity = this.AirVelocity;
            altitudeData.JumpVelocity = this.JumpVelocity;
            altitudeData.Gravity = this.Gravity;
            altitudeData.StopWhenHitGround = this.StopWhenHitGround;
        }
    }
}
