using BehaviorDesigner.Runtime;
using RQ.Common.Components;
using UnityEngine;
using static BehaviorDesigner.Runtime.BehaviorManager;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Behavior Tree")]
    public class BehaviorTreeComponent : ComponentPersistence<BehaviorTreeComponent>
    {
        private Behavior _behaviorTree;

        //private WaypointData _waypointData;

        public override void Awake()
        {
            base.Awake();
            _behaviorTree = GetComponent<Behavior>();
        }

        public void EnableBT()
        {
            gameObject.SetActive(true);
            _behaviorTree.EnableBehavior();
            
            //BehaviorManager.instance.EnableBehavior(_behaviorTree);
            //_behaviorTree.Initialize()
        }

        public void DisableBT()
        {
            gameObject.SetActive(false);
            _behaviorTree.DisableBehavior();
        }
        //    base.Awake();
        //    if (!Application.isPlaying)
        //        return;

        //    _waypointData = new WaypointData();
        //    _waypointData.Waypoints = new Dictionary<string, Serialization.Vector3Serializer>();
        //    var waypoints = GetComponentsInChildren<Transform>();
        //    foreach (var waypoint in waypoints)
        //    {
        //        _waypointData.Waypoints.Add(waypoint.name, waypoint.position);
        //    }
        //}

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
        //        if (currentWaypoints.Any(i => i.name == waypoint.Key))
        //            continue;
        //        var go = new GameObject(waypoint.Key);
        //        go.transform.position = waypoint.Value;
        //        go.transform.parent = transform;
        //    }
        //}
    }
}
