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
    public class KillSelfAtom : AtomActionBase
    {
        [SerializeField]
        private float _delay = 0f;
        [SerializeField]
        private bool _killWhenAnimationCompletes = false;
        //public string AnimationType;
        //private AnimationComponent _animComponent;
        //private IComponentRepository _entity;
        //private bool _isRunning;
        //private long _killSelfIndex;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //_entity = entity;
            //_isRunning = true;
            if (_killWhenAnimationCompletes)
                _delay = GetClipLength();
            //SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId);
            var damageComponent = entity.Components.GetComponent<DamageComponent>();
            if (damageComponent != null)
            {
                var deathPrefab = damageComponent.GetDeathPrefab();
                if (deathPrefab != null)
                    GameObject.Instantiate(deathPrefab, entity.transform.position, Quaternion.identity);
            }

            MessageDispatcher2.Instance.DispatchMsg("StopMovement", 0f, entity.UniqueId, entity.UniqueId, null);
            //MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, entity.UniqueId, entity.UniqueId, null);
            MessageDispatcher2.Instance.DispatchMsg("GlobalEntityDied", 0f, entity.UniqueId, null,
                entity.UniqueId);
            entity.Destroy();
        }

        private float GetClipLength()
        {
            var animComponent = _entity.Components.GetComponent<AnimationComponent>();
            var spriteAnimator = animComponent.GetSpriteAnimator() as ISpriteAnimator;
            return spriteAnimator.GetCurrentClipLength();
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _killSelfIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
        //    {
        //        //var animation = _animComponent.Get
        //        if ((string)data.ExtraInfo != AnimationType)
        //            return;
        //        _isRunning = false;
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        //}

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
