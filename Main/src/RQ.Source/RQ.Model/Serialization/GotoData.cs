using RQ.Controller.Actions;
using RQ.Physics;
using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class GoToData
    {
        public GoToType GoToType { get; set; }

        public bool Immediate { get; set; }

        public string WaypointUniqueId { get; set; }

        public Vector2D Velocity { get; set; }

        //public bool LookToTarget { get; set; }

        //public bool PlayAnimation { get; set; }

        public Vector2D FeetPosition { get; set; }
    }
}
