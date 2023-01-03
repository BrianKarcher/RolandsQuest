using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.AtomAction.Physics
{
    [Serializable]
    public class WaypointsToPathAtom : AtomActionBase
    {
        private Vector3[] _path;
        public Vector3[] Path { get { return _path; } }
        private WaypointComponent _waypointComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _waypointComponent = entity.Components.GetComponent<WaypointComponent>();
            var waypointsList = _waypointComponent.GetWaypoints();
            _path = new Vector3[waypointsList.Count];
            for (int i = 0; i < waypointsList.Count; i++)
            {
                _path[i] = waypointsList[i];
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
