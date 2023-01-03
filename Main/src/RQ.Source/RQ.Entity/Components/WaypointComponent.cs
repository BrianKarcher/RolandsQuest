using RQ.Common.Components;
using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Waypoint")]
    public class WaypointComponent : ComponentPersistence<WaypointComponent>, IWaypointComponent
    {
        public List<GameObject> _waypoints;

        [SerializeField]
        private FollowPathData _followPathData;
        public FollowPathData FollowPathData { get { return _followPathData; } }
        //private WaypointData _waypointData;
        private List<Vector3> _waypointVector3s;
        private int _currentIndex;

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;

            _currentIndex = -1;
            //_waypointData = new WaypointData();
            //_waypointData.Waypoints = new List<KeyValuePair<string, Vector3>>();
            _waypointVector3s = new List<Vector3>();
            //if (_waypoints.Any())
            if (_waypoints.Count != 0)
            {
                int i = 0;
                foreach (var waypoint in _waypoints)
                {
                    if (waypoint == null)
                        continue;
                    //_waypointData.Waypoints.Add(new KeyValuePair<string, Vector3>(waypoint.name + i, waypoint.transform.position));
                    _waypointVector3s.Add(waypoint.transform.position);
                    i++;
                }
            }
            else
            {
                var waypoints = GetComponentsInChildren<Transform>();
                foreach (var waypoint in waypoints)
                {
                    //_waypointData.Waypoints.Add(new KeyValuePair<string, Vector3>(waypoint.name, waypoint.position));
                    _waypointVector3s.Add(waypoint.position);
                }
            }
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    base.SerializeComponent(entitySerializedData, _waypointData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    _waypointData = base.DeserializeComponent<WaypointData>(entitySerializedData);
            
        //    CreateWaypoints();
        //}

        //private void CreateWaypoints()
        //{
        //    var currentWaypoints = GetComponentsInChildren<Transform>();
        //    foreach (var waypoint in _waypointData.Waypoints)
        //    {
        //        bool isFound = false;
        //        foreach (var currentWaypoint in currentWaypoints)
        //        {
        //            if (currentWaypoint.name == waypoint.Key)
        //            {
        //                isFound = true;
        //                break;
        //            }
        //        }
        //        //if (currentWaypoints.Any(i => i.name == waypoint.Key))
        //        if (!isFound)
        //            continue;
        //        var go = new GameObject(waypoint.Key);
        //        go.transform.position = waypoint.Value;
        //        go.transform.parent = transform;
        //    }
        //}

        public IList<Vector3> GetWaypoints()
        {
            return _waypointVector3s;
            //return _waypointData.Waypoints.Select(i => i.Value);
        }

        public Vector3 NextWaypoint()
        {
            _currentIndex++;
            //if (_currentIndex >= _waypointData.Waypoints.Count)
            //    _currentIndex = 0;
            //return _waypointData.Waypoints[_currentIndex].Value;
            if (_currentIndex >= _waypointVector3s.Count)
                _currentIndex = 0;
            return _waypointVector3s[_currentIndex];
        }
    }
}
