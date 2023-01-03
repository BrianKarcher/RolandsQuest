using RQ.Animation;
using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    [Serializable]
    public class DamagedAtom : AtomActionBase
    {
        private DamageEntityInfo _damageInfo;
        private PhysicsComponent _physicsComponent;
        private DamageComponent _damageComponent;
        private AnimationComponent _animationComponent;
        public bool UseDamageColor = true;
        private long _damageBounceCompleteIndex;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _damageComponent = entity.Components.GetComponent<DamageComponent>();
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animationComponents.Count; i++)
            {
                var aniamtionComponent = animationComponents[i] as AnimationComponent;
                if (aniamtionComponent.IsMain())
                {
                    _animationComponent = aniamtionComponent;
                    break;
                }
            }
            //_animationComponent = entity.Components.GetComponents<AnimationComponent>().Where(i => i.IsMain()).First();

            MessageDispatcher.Instance.DispatchMsg(0f, _damageComponent.UniqueId, _damageComponent.UniqueId,
                Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);

            var inputAffector = _physicsComponent.GetPhysicsAffector("Input");
            inputAffector.Force = Vector2.zero;

            if (_damageComponent.UseDamageColor && UseDamageColor)
            {
                _animationComponent?.GetSpriteAnimator()?.SetColor("Damage", _damageComponent.DamageColor);
            }
        }

        public override void End()
        {
            base.End();
            _damageInfo.IsDamaged = false;
            if (_damageComponent.UseDamageColor && UseDamageColor)
            {
                _animationComponent?.GetSpriteAnimator()?.RemoveColor("Damage");
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
