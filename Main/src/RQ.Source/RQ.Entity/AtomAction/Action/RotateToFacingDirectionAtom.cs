using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class RotateToFacingDirectionAtom : AtomActionBase
    {
        private AnimationComponent _animComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _animComponent = entity.Components.GetComponent<AnimationComponent>();
            Tick();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void Tick()
        {
            var dest = (Vector2)_animComponent.GetFacingDirectionVector();
            MessageDispatcher2.Instance.DispatchMsg("RotateTo", 0f, _entity.UniqueId, _entity.UniqueId, dest);
        }
    }
}
