using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using RQ.Common;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetVisibilityAtom : AtomActionBase
    {
        public bool VisibilityOnEnter;
        public bool VisibilityOnExit;
        private GameObject _entityToAffect;
        private IList<IBaseObject> _animationComponents;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _animationComponents = entity.Components.GetComponents<AnimationComponent>();

            if (_animationComponents == null)
                return;

            //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < _animationComponents.Count; i++)
            {
                var aniamtionComponent = _animationComponents[i] as AnimationComponent;
                aniamtionComponent?.SetRender(VisibilityOnEnter);
            }
            //var visible = VisibilityOnEnter ? "1" : "0";
            //Debug.LogError($"Setting visibility to {VisibilityOnEnter}");
            //MessageDispatcher2.Instance.DispatchMsg("SetRender", 0f, string.Empty, entity.UniqueId, VisibilityOnEnter);
        }

        public override void End()
        {
            if (_animationComponents == null)
                return;

            //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < _animationComponents.Count; i++)
            {
                var aniamtionComponent = _animationComponents[i] as AnimationComponent;
                aniamtionComponent?.SetRender(VisibilityOnExit);
            }
            //var visible = VisibilityOnExit ? "1" : "0";
            //Debug.LogError($"Setting visibility to {VisibilityOnExit}");
            //MessageDispatcher2.Instance.DispatchMsg("SetRender", 0f, string.Empty, _entity.UniqueId, VisibilityOnExit);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public void SetEntityToAffect(GameObject entity)
        {
            _entityToAffect = entity;
        }
    }
}
