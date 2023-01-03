using RQ.AI;
using RQ.Physics.SteeringBehaviors.RQ.Physics.SteeringBehaviors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class FollowPath : SteeringBehaviorBase, ISteeringBehavior
    {
        //used in path following
        // Determines the distance you can be from a waypoint before triggering the next one
        public float WaypointSeekDist = .08f;
        //the distance (squared) a vehicle has to be from a path waypoint before
        //it starts seeking to the next waypoint
        public float WaypointSeekDistSq;

        //pointer to any current path
        public Path Path { get; set; }

        /// <summary>
        /// The new force gets sent to the WaypointChanged delegate so the receiver knows what the new direction is
        /// </summary>
        public Action<Vector2D> WaypointChanged;
        //private bool _isFinished;

        //public bool IsFinished { get { return _isFinished; } set { _isFinished = value; } }

        //public event Action PathComplete;

        public FollowPath(SteeringBehaviorManager manager)
            : base(manager)
        {
            CreateWaypointSeekDistSq();
            _constantWeight = Constants.FollowPathWeight;
            //create a Path
            Path = new Path();
            //Path.LoopOn();
            //_isFinished = false;
            //manager.GetSteeringBehavior(behavior_type.follow_path).;
        }

        private void CreateWaypointSeekDistSq()
        {
            WaypointSeekDistSq = WaypointSeekDist * WaypointSeekDist;
        }

        //given a series of Vector2Ds, this method produces a force that will
        //move the agent along the waypoints in order
        protected override Vector2 CalculateForce()
        {
            //if (Path.IsFinished())
            //{
            //    _isFinished = true;
            //    //_steeringBehaviorManager.
            //    return Vector2D.Zero();
            //}
            bool hasWaypointChanged = false;

            //Vector3 pos;
            //if (_steeringBehaviorManager.Entity is ISprite)
            //{

            //}
            //else
            //{

            //}

            if (CheckAndUpdateWaypoint(_steeringBehaviorManager.Entity.GetFeetWorldPosition()))
                hasWaypointChanged = true;

            var force = SteeringBehaviorCalculations.FollowPath(_steeringBehaviorManager.Entity, Path);

            if (hasWaypointChanged)
            {
                WaypointChanged?.Invoke(force);
            }
            return force;
        }

        public bool CheckAndUpdateWaypoint(Vector2 position)
        {
            Vector2 currentWaypointPos;
            if (!Path.CurrentWaypoint(out currentWaypointPos))
                return false;
            var feetPos = position;
            var distanceSq = (currentWaypointPos - feetPos).sqrMagnitude;
            if (distanceSq < WaypointSeekDistSq)
            {
                Path.SetNextWaypoint();
                return true;
            }
            return false;
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            //if (!Path.HasWaypoints())
            //    return;
            Vector2 wayPoint;
            if (!Path.CurrentWaypoint(out wayPoint))
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_steeringBehaviorManager.Entity.transform.position, wayPoint);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wayPoint, 0.04f);

        }

        public void SetPath(List<Vector3> new_path)
        {
            Path.Set(new_path);
        }

        public bool ComparePath(List<Vector3> path)
        {
            //bool isEqual = true;
            var currentWaypoints = Path.GetWaypoints();
            if (currentWaypoints == null)
                return false;
            if (path.Count != currentWaypoints.Count)
                return false;
            for (int i = 0; i < currentWaypoints.Count; i++)
            {
                if (path[i] != currentWaypoints[i])
                    return false;
            }
            return true;
        }
        //public void SetPath(Vector2D[] new_path)
        //{
        //    Path.Set(new_path);
        //}
        //public void CreateRandomPath(int num_waypoints, int mx, int my, int cx, int cy)
        //{
        //    Path.CreateRandomPath(num_waypoints, mx, my, cx, cy);
        //}

        //public override void Serialize(SteeringBehaviorData data)
        //{
        //    data.WayPoints = Path.GetWaypoints().Select(i => (Vector2D)i).ToArray();
        //    data.CurWaypoint = Path.GetCurWaypointIndex();
        //    data.PathType = Path.PathType;
        //}

        //public override void Deserialize(SteeringBehaviorData data)
        //{
        //    Path.Set(data.WayPoints.Select(i => (Vector2)i).ToArray());
        //    Path.SetCurrentWaypoint(data.CurWaypoint);
        //    Path.PathType = data.PathType;
        //    //if (data.Looped)
        //    //    Path.LoopOn();
        //    //else
        //    //    Path.LoopOff();
        //}
    }
}
