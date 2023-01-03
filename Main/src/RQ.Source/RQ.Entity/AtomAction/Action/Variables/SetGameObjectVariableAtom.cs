using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;
using RQ.Common.Container;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetGameObjectVariableAtom2 : AtomActionBase
    {
        public ActionTarget Type = ActionTarget.Self;
        private GameObject _gameObject;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            SetValue();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void SetValue()
        {
            var repo = GetTargetGameObject(Type);
            _gameObject = repo;
        }

        public GameObject GetGameObject()
        {
            return _gameObject;
        }

        public GameObject GetTargetGameObject(ActionTarget ActionTarget)
        {
            switch (ActionTarget)
            {
                case ActionTarget.Self:
                    return _entity.gameObject;
                case ActionTarget.Target:
                    var targetingData = _entity.Components.GetComponent<AIComponent>()?.Target;
                    if (targetingData == null)
                    {
                        Debug.LogError($"Could not locate AIComponent in {_entity.name}");
                        return null;
                    }
                    return targetingData.gameObject;
                case ActionTarget.MainCharacter:
                    return EntityContainer._instance.GetMainCharacter().gameObject;
                case ActionTarget.Companion:
                    return EntityContainer._instance.GetCompanionCharacter().gameObject;
                case ActionTarget.MCJointUnit:
                    var mainCharacter = EntityContainer._instance.GetMainCharacter() as IComponentRepository;
                    var joint = mainCharacter.GetComponent<Joint>();
                    var mcRigidBody = joint.connectedBody;
                    //var jointUnit = mcRigidBody.GetComponent<IComponentRepository>();
                    return mcRigidBody.gameObject;

                //case ActionTarget.Waypoint1:
                //    var waypointComponent = _entity.Components.GetComponent<IWaypointComponent>();
                //    var wayPoints = waypointComponent.GetWaypoints();
                //    return wayPoints.FirstOrDefault();
                case ActionTarget.Parent:
                    var parentData = _entity.Components.GetComponent<AIComponent>()?.Parent;
                    if (parentData == null)
                    {
                        //Debug.LogError($"Could not locate AIComponent or Parent in {_entity.name}");
                        return null;
                    }
                    return parentData.gameObject;
                case ActionTarget.Waypoint1:
                    var waypointComponent = _entity.Components.GetComponent<WaypointComponent>();
                    return waypointComponent._waypoints[0];
                case ActionTarget.Waypoint2:
                    var waypointComponent2 = _entity.Components.GetComponent<WaypointComponent>();
                    return waypointComponent2._waypoints[1];
                case ActionTarget.Waypoint3:
                    var waypointComponent3 = _entity.Components.GetComponent<WaypointComponent>();
                    return waypointComponent3._waypoints[2];
                default:
                    return null;
            }
        }
    }
}
