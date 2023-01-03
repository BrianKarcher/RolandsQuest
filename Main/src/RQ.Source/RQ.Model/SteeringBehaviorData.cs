using RQ.Physics;
using RQ.Physics.Pathfinding;
using RQ.Physics.SteeringBehaviors;
using System;
using System.Collections.Generic;

namespace RQ.Serialization
{
    [Serializable]
    public class SteeringBehaviorData
    {
        public List<behavior_type> ActiveBehaviors { get; set; }

        #region Steering Behavior Manager
        //these can be used to keep track of friends, pursuers, or prey
        public string TargetAgent1 { get; set; }
        public string TargetAgent2 { get; set; }
        public Vector3Serializer Offset { get; set; }

        //the current target
        public Vector2D Target { get; set; }

        #endregion

        #region Wander
        public Vector2D last_wander_pos { get; set; }

        public Vector2D _wanderTarget { get; set; }
        #endregion

        #region Arrive
        public Deceleration Deceleration { get; set; }
        public float Weight { get; set; }
        #endregion

        #region FollowPath
        public Vector2D[] WayPoints { get; set; }

        //points to the current waypoint
        public int CurWaypoint { get; set; }
        
        //flag to indicate if the path should be looped
        //(The last waypoint connected to the first)
        public PathType PathType { get; set; }
        #endregion

        #region ObstacleAvoidance
        //length of the 'detection box' utilized in obstacle avoidance
        public float BoxLength { get; set; }

        public List<string> Obstacles { get; set; }
        #endregion

        #region Wall Avoidance
        //a vertex buffer to contain the feelers rqd for wall avoidance  
        public List<Vector2D> Feelers { get; set; }
        #endregion
    }
}
