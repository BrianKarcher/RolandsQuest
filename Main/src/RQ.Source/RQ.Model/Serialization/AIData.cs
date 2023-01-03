using RQ.Physics;
using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class AIData
    {
        public string Target { get; set; }
        public string[] Targets;
        public string CurrentDestinationLocation { get; set; }
        public string CurrentDestinationSubLocation { get; set; }
        public string[] Locations;
        public string Parent { get; set; }
        //[NonSerialized]
        //public LayerMask TargetMask;
        //[HideInInspector]
        public int TargetMaskInt;
        public string Follow { get; set; }
        public Vector2D HomePosition { get; set; }
        public string WaypointUniqueId { get; set; }
    }
}
