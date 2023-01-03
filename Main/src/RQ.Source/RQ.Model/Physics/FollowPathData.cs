using RQ.Physics.Pathfinding;
using RQ.Physics.SteeringBehaviors;
using System;

namespace RQ.Model.Physics
{
    [Serializable]
    public class FollowPathData
    {
        public string WaypointUniqueId { get; set; }

        public PathType PathType;

        public PathWalkingDirection PathWalkingDirection;

        public DirectionMode FacingDirectionMode;

        public Direction FacingDirection;
        public int CurrentWaypoint { get; set; }
    }
}
