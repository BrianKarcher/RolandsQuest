using RQ.AI;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Entity.AtomAction.Action.Physics
{
    [Serializable]
    public class LockOnTargetAtom : AtomActionBase
    {
        private IBasicPhysicsComponent _physicsComponent;
        private AIComponent _aiComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<IBasicPhysicsComponent>();
            _aiComponent = entity.Components.GetComponent<AIComponent>();
        }

        public override void End()
        {
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            //var physicsData = _physicsComponent.GetPhysicsData();
            if (_aiComponent.Target == null)
                return;
            var targetLocation = (Vector2)_aiComponent.Target.transform.position;

            _physicsComponent.SetWorldPos(targetLocation);
            //MessageDispatcher2.Instance.DispatchMsg("SetPos", 0f, _entity.UniqueId, _physicsComponent.UniqueId,
            //    targetLocation);
            MessageDispatcher2.Instance.DispatchMsg("CameraUpdate", 0f, _entity.UniqueId, _entity.UniqueId, null);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }
    }
}
