using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Model.Enums;
using RQ.Model.Interfaces;
using System;
using UnityEngine;

namespace RQ.Entity.AtomAction
{
    [Serializable]
    public abstract class AtomActionBase : IAtomAction
    {
        protected IComponentRepository _entity;
        protected bool _isRunning;
        //protected bool _isRunning = true;

        public virtual void Start(IComponentRepository entity)
        {
            _entity = entity;
            _isRunning = true;
            StartListening(_entity);
        }

        public virtual void End()
        {
            StopListening(_entity);
            _isRunning = false;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public IComponentRepository GetTarget(ActionTarget ActionTarget)
        {
            switch (ActionTarget)
            {
                case ActionTarget.Self:
                    return _entity;
                case ActionTarget.Target:
                    var targetingData = _entity.Components.GetComponent<AIComponent>()?.Target;
                    if (targetingData == null)
                    {
                        Debug.LogError($"Could not locate AIComponent in {_entity.name}");
                        return null;
                    }
                    return targetingData.GetComponent<IComponentRepository>();
                case ActionTarget.MainCharacter:
                    return EntityContainer._instance.GetMainCharacter() as IComponentRepository;
                case ActionTarget.Companion:
                    return EntityContainer._instance.GetCompanionCharacter() as IComponentRepository;
                case ActionTarget.MCJointUnit:
                    var mainCharacter = EntityContainer._instance.GetMainCharacter() as IComponentRepository;
                    var joint = mainCharacter.GetComponent<Joint>();
                    var mcRigidBody = joint.connectedBody;
                    var jointUnit = mcRigidBody.GetComponent<IComponentRepository>();
                    return jointUnit;

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
                    return parentData.GetComponent<IComponentRepository>();
                //case ActionTarget.Waypoint1:
                //    var waypointComponent = _entity.Components.GetComponent<IWaypointComponent>();
                    
                default:
                    return null;
            }
        }

        public Vector3 GetTargetPosition(ActionTarget ActionTarget)
        {
            switch (ActionTarget)
            {
                case ActionTarget.Self:
                    return _entity.transform.position;
                case ActionTarget.Target:
                    var targetingData = _entity.Components.GetComponent<AIComponent>()?.Target;
                    return targetingData.GetComponent<IComponentRepository>().transform.position;
                case ActionTarget.MainCharacter:
                    return (EntityContainer._instance.GetMainCharacter() as IComponentRepository).transform.position;
                case ActionTarget.Companion:
                    return (EntityContainer._instance.GetCompanionCharacter() as IComponentRepository).transform.position;
                case ActionTarget.Waypoint1:
                    var waypointComponent = _entity.Components.GetComponent<IWaypointComponent>();
                    var wayPoints = waypointComponent.GetWaypoints();
                    if (wayPoints == null || wayPoints.Count == 0)
                        return Vector3.zero;
                    return wayPoints[0];
                    //return wayPoints.FirstOrDefault();
                case ActionTarget.Waypoint2:
                    var waypointComponent2 = _entity.Components.GetComponent<IWaypointComponent>();
                    //var wayPoints2 = waypointComponent2.GetWaypoints().ToList();
                    //return wayPoints2[1];
                    var wayPoints2 = waypointComponent2.GetWaypoints();
                    if (wayPoints2 == null || wayPoints2.Count < 2)
                        return Vector3.zero;
                    return wayPoints2[1];
                case ActionTarget.Parent:
                    var parentData = _entity.Components.GetComponent<AIComponent>()?.Parent;
                    if (parentData == null)
                        Debug.LogError("Could not locate Parent.");
                    return parentData.GetComponent<IComponentRepository>().transform.position;
                default:
                    return Vector3.zero;
            }
        }

        public virtual void StartListening(IComponentRepository entity)
        {

        }

        public virtual void StopListening(IComponentRepository entity)
        {

        }

        public abstract AtomActionResults OnUpdate();

        public virtual void OnLateUpdate()
        {

        }
    }
}
