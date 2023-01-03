using RQ.AI;
using RQ.Entity.Components;
using RQ.Messaging;
using System;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class WaitForAnimationCompletionAtom : IAtomAction
    {
        /// <summary>
        /// Leave empty to Finish on completion of any animation type
        /// </summary>
        public string AnimationType;
        private AnimationComponent _animComponent;
        private IComponentRepository _entity;
        private bool _isRunning;
        private long _animationCompleteIndex;

        public void Start(IComponentRepository entity)
        {
            _entity = entity;
            _isRunning = true;
            StartListening(entity);
        }

        public void StartListening(IComponentRepository entity)
        {
            _animationCompleteIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
            {
                if (string.IsNullOrEmpty(AnimationType))
                    _isRunning = false;
                //var animation = _animComponent.Get
                if ((string)data.ExtraInfo != AnimationType)
                    return;
                _isRunning = false;
            });
        }

        public void StopListening(IComponentRepository entity)
        {
            MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        }

        public void End()
        {
            StopListening(_entity);
        }

        public AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
