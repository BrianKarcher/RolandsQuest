using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetFacingDirectionAtom : AtomActionBase
    {
        public enum FacingDirectionEnum
        {
            ToVelocity = 0,
            ToTarget = 1,
            CurrentDestinationLocation = 2,
            CurrentDestinationSubLocation = 3
        }

        public FacingDirectionEnum FacingDirection;
        private PhysicsComponent _physicsComponent;
        private AIComponent _aiComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            Tick();
        }

        public override AtomActionResults OnUpdate()
        {
            Tick();
            return AtomActionResults.Running;
        }

        private void Tick()
        {
            var velocity = GetVelocity();
            MessageDispatcher2.Instance.DispatchMsg("VelocityChanged", 0f, _entity.UniqueId, _entity.UniqueId, velocity);
        }

        private Vector2D GetVelocity()
        {
            var entityPos = _physicsComponent.GetFeetWorldPosition3();
            switch (FacingDirection)
            {
                case FacingDirectionEnum.ToVelocity:
                    var physicsData = _physicsComponent.GetPhysicsData();
                    var velocity = _physicsComponent.GetVelocity();
                    return velocity;
                case FacingDirectionEnum.ToTarget:
                    //var entityPos = _physicsComponent.GetWorldPos();
                    
                    if (_aiComponent.Target == null)
                        return Vector2D.Zero();
                    var targetPos = _aiComponent.Target.position;
                    var direction = (Vector2D)(targetPos - entityPos);
                    return direction;
                case FacingDirectionEnum.CurrentDestinationLocation:
                    var currentDestinationLocation = _aiComponent.CurrentDestinationLocation.transform.position;
                    return (Vector2D)(currentDestinationLocation - entityPos);
                case FacingDirectionEnum.CurrentDestinationSubLocation:
                    var currentDestinationSubLocation = _aiComponent.CurrentDestinationSubLocation.transform.position;
                    return (Vector2D)(currentDestinationSubLocation - entityPos);
            }
            return Vector2D.Zero();
        }
    }
}
