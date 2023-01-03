using RQ.AI;
using RQ.Animation;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action.Animation
{
    [Serializable]
    public class SetMaterialAtom : AtomActionBase
    {
        public Material Material;
        private AnimationComponent _animComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animationComponents.Count; i++)
            {
                var aniamtionComponent = animationComponents[i] as AnimationComponent;
                if (aniamtionComponent.IsMain())
                {
                    _animComponent = aniamtionComponent;
                    break;
                }
            }
            //_animComponent = entity.Components.GetComponents<AnimationComponent>()
            //    .FirstOrDefault(i => i.IsMain());
            //Debug.LogError($"Setting material for {entity.name} to {Material.name}");
            _animComponent.GetSpriteAnimator().SetMaterial(Material);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public override void OnLateUpdate()
        {
            //_animComponent.GetSpriteAnimator().SetMaterial(Material);
            base.OnLateUpdate();
        }
    }
}
