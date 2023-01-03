using RQ.Animation;
using RQ.Animation.V2;
using RQ.AnimationV2;
using RQ.Common.Config;
using RQ.Controller.SequencerEvents;
using RQ.Entity.Components;
using RQ.FSM.V2.States;
using RQ.Messaging;
using RQ.Model.Animation;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Animate/Direction Transition")]
    public class DirectionTransitionAnimate : ActionBase, IAnimatedObject
    {
        [SerializeField]
        private DirectionTransitionAnimateData[] _directionTransitionDatas;

        //[SerializeField]
        protected AnimationComponent _animationComponent;
        [SerializeField]
        protected RQBaseConfig _spriteAnimationsConfig;
        private ISpriteAnimationsConfig _iSpriteAnimationsConfig;

        public void Awake()
        {
            _iSpriteAnimationsConfig = _spriteAnimationsConfig as ISpriteAnimationsConfig;
        }

        public override void InitAction()
        {
            base.InitAction();
            if (_animationComponent == null)
            {
                var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                if (animationComponents != null)
                {
                    //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                    for (int i = 0; i < animationComponents.Count; i++)
                    {
                        var aniamtionComponent = animationComponents[i] as AnimationComponent;
                        if (aniamtionComponent.IsMain())
                        {
                            _animationComponent = aniamtionComponent;
                            break;
                        }
                    }
                    //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                }
            }
        }

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (_animationComponent == null)
                return;
            var animationData = _animationComponent.Data;
            DirectionTransitionAnimateData directionTransition = null;
            foreach (var tempTransition in _directionTransitionDatas)
            {
                if (tempTransition.NewDirection == animationData.FacingDirection &&
                tempTransition.OldDirection == animationData.PreviousFacingDirection)
                {
                    directionTransition = tempTransition;
                    break;
                }
            }
            //var directionTransition = _directionTransitionDatas.FirstOrDefault(i =>
            //    i.NewDirection == animationData.FacingDirection && 
            //    i.OldDirection == animationData.PreviousFacingDirection);

            // Could not find transition, mark it complete
            if (directionTransition == null)
            {
                _animationComponent.Data.IsAnimationComplete = true;
                //Complete();
                return;
            }
            _animationComponent.SetManualHFlip(true);
            _animationComponent.SetHFlip(directionTransition.HFlip);
            _animationComponent.Animate(directionTransition.AnimationType);

            _animationComponent.Data.IsAnimationComplete = false;
            var spriteAnimator = _animationComponent.GetSpriteAnimator() as ISpriteAnimator;
            var animationLength = spriteAnimator.GetCurrentClipLength();
            SendMessageToSelf(animationLength, Enums.Telegrams.AnimationComplete, UniqueId);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            _animationComponent.SetManualHFlip(false);
        }

        public RQ.AnimationV2.ISpriteRenderer GetSpriteRenderer()
        {
            //var state = GetComponent<StateBase>();
            //var stateMachine = state.StateMachine;
            //if (stateMachine == null)
            //{
            //    return null;
            //}

            //var entity = stateMachine.GetEntity();
            //if (entity == null)
            //{
            //    return null;
            //}

            //var baseGameEntity = EntityUIBase.GetEntity<IBaseGameEntity>(entity);
            //if (baseGameEntity == null)
            //{
            //    return null;
            //}

            //var entityUIBase = entity.GetComponent<EntityUIBase>();
            //if (entityUIBase == null)
            //    return null;

            //var componentRepository = stateMachine.GetComponentRepository();


            if (_animationComponent == null)
            {
                var entity = GetEntity();
                if (entity == null)
                {
                    var runActionsState = GetComponent<RunActionsState>();
                    //var stateMachine = runActionsState.StateMachine;
                    if (runActionsState != null)
                        entity = runActionsState.GetEntity();

                    //Debug.Log("Could not locate entity");
                    //return null;
                }
                if (entity == null)
                {
                    var playActionEvent = GetComponent<IPlayActionsEvent>();
                    if (playActionEvent != null)
                        entity = playActionEvent.AffectedObject.GetComponent<IComponentRepository>();
                }

                var animationComponents = entity.Components.GetComponents<AnimationComponent>();

                if (animationComponents == null)
                    return null;

                //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }

                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain() == true);
            }

            //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
            if (_animationComponent == null)
                return null;

            return _animationComponent.GetSpriteAnimator();
        }

        public List<SpriteAnimationType> GetStoredSpriteAnimations()
        {
            if (_spriteAnimationsConfig != null)
                return _iSpriteAnimationsConfig.GetStoredSpriteAnimations();

            if (_animationComponent == null)
            {
                var entity = GetEntity();
                if (entity == null)
                {
                    var runActionsState = GetComponent<RunActionsState>();
                    //var stateMachine = runActionsState.StateMachine;
                    if (runActionsState != null)
                        entity = runActionsState.GetEntity();

                    //Debug.Log("Could not locate entity");
                    //return null;
                }
                if (entity == null)
                {
                    var playActionEvent = GetComponent<IPlayActionsEvent>();
                    if (playActionEvent != null)
                        entity = playActionEvent.AffectedObject.GetComponent<IComponentRepository>();
                }

                var animationComponents = entity.Components.GetComponents<AnimationComponent>();

                if (animationComponents == null)
                    return null;

                //var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        _animationComponent = aniamtionComponent;
                        break;
                    }
                }

                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain() == true);
            }

            //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
            if (_animationComponent == null)
                return null;

            return _animationComponent.GetSpriteAnimator().GetStoredSpriteAnimations();
        }

        public AnimationComponent GetAnimationComponent()
        {
            return _animationComponent;
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;
            //var data = agent as ISprite;
            switch (telegram.Msg)
            {
                case Enums.Telegrams.AnimationComplete:
                    if ((string)telegram.ExtraInfo == UniqueId)
                    {
                        //var sprite = agent as ISprite;
                        _animationComponent.Data.IsAnimationComplete = true;
                    }
                    // TODO FIX THIS - The state transition table should be determing the next state
                    //data.GetFSM().RevertToPreviousState();
                    break;
            }
            return false;
        }
    }
}
