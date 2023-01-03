using RQ.AI;
using RQ.Common;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class AnimationAtom : IAtomAction
    {
        //[SerializeField]
        ////[UniqueIdentifier]
        //public string _uniqueId;
        public string _animComponentName;
        [SerializeField]
        [AnimationTypeAttribute]
        //[AIAnimationTypeAttribute]
        public string _animation = null;
        private bool _byType = true;
        public bool WaitForAnimationCompletion = true;
        public bool StopOnExit = false;
        //[SerializeField]
        private AnimationComponent _animationComponent;
        private long _animationCompleteIndex;
        private bool _isRunning;
        private IComponentRepository _entity;

        private System.Action _spriteAnimator_AnimCompleteDelegate = null;
        //public AnimationAtom()
        //{
        //    if (string.IsNullOrEmpty(_uniqueId))
        //        _uniqueId = Guid.NewGuid().ToString();
        //}

        public void Start(IComponentRepository entity)
        {
            _entity = entity;
            //if (_animation.Contains("Dust"))
            //{
            //    int i = 1;
            //}
            GetAnimComponent(entity);

            //Log.Info("Entering Animator Atom Act for entity " + entity.name);
            string animationId = null;
            if (!String.IsNullOrEmpty(_animation))
            {
                 animationId = _byType ? _animationComponent.GetIdByType(_animation) : _animation;
                _animationComponent.Animate(animationId);
                _isRunning = true;
            }
            _animationComponent.Data.IsAnimationComplete = false;
            var spriteAnimator = _animationComponent.GetSpriteAnimator();
            if (spriteAnimator == null)
                return;
            if (_spriteAnimator_AnimCompleteDelegate == null)
                _spriteAnimator_AnimCompleteDelegate = SpriteAnimator_AnimComplete;
            spriteAnimator.AnimComplete += _spriteAnimator_AnimCompleteDelegate;

            StartListening(entity);
            if (!WaitForAnimationCompletion || animationId == null)
                _isRunning = false;
        }

        private void GetAnimComponent(IComponentRepository entity)
        {
            if (!String.IsNullOrEmpty(_animComponentName))
            {
                _animationComponent = entity.Components.GetComponent<AnimationComponent>(_animComponentName);
                if (_animationComponent != null)
                    return;
            }
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            if (animationComponents != null)
            {
                for (int i = 0; i < animationComponents.Count; i++)
                    //foreach (var component in animationComponents)
                {
                    var animComponent = animationComponents[i] as AnimationComponent;
                    if (animComponent.IsMain())
                    {
                        _animationComponent = animComponent;
                        break;
                    }
                }
                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
            }
        }

        private void SpriteAnimator_AnimComplete()
        {
            _isRunning = false;
        }

        public void StartListening(IComponentRepository entity)
        {
            //_animationCompleteIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
            //{
            //    _isRunning = false;
            //});
            //MessageDispatcher2.Instance.DispatchMsg("AnimationComplete", animationLength, this._uniqueId, this._uniqueId, _uniqueId);

        }

        public void StopListening(IComponentRepository entity)
        {
            //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
            //MessageDispatcher2.Instance.DispatchMsg("AnimationComplete", animationLength, this._uniqueId, this._uniqueId, _uniqueId);

        }

        public void End()
        {
            StopListening(_entity);
            var spriteAnimator = _animationComponent.GetSpriteAnimator();
            if (spriteAnimator == null)
                return;
            spriteAnimator.AnimComplete -= _spriteAnimator_AnimCompleteDelegate;
            if (StopOnExit)
                _animationComponent.StopAnimation();
        }

        public AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
