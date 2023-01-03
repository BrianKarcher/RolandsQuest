using RQ.Animation;
using RQ.Animation.V2;
using RQ.AnimationV2;
using RQ.Common.Config;
using RQ.Controller.SequencerEvents;
using RQ.Entity.Components;
using RQ.FSM.V2.States;
using RQ.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    // TODO Serialize THIS!!!!!!!!!!!!!!
    [AddComponentMenu("RQ/Action/Animate2")]
    public class Animate2 : ActionBase
    {
        [SerializeField]
        private string _animation = null;

        // TODO Turn this into a drop down
        [SerializeField]
        protected AnimationComponent _animationComponent;
        [SerializeField]
        protected RQBaseConfig _spriteAnimationsConfig;
        private ISpriteAnimationsConfig _iSpriteAnimationsConfig;
        private long _animationCompleteIndex;

        public override void InitAction()
        {
            base.InitAction();
            InitAnimationComponent();
        }

        public override void Awake()
        {
            base.Awake();
            InitAnimationComponent();
        }

        private void InitAnimationComponent()
        {
            _iSpriteAnimationsConfig = _spriteAnimationsConfig as ISpriteAnimationsConfig;
            if (_animationComponent == null)
            {
                var entity = GetRepo();
                if (entity == null)
                    return;
                var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                if (animationComponents != null)
                {
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
            if (!String.IsNullOrEmpty(_animation))
            {
                var animationId = _animationComponent.GetIdByType(_animation);
                _animationComponent.Animate(animationId);
            }
            _animationComponent.Data.IsAnimationComplete = false;
            var spriteAnimator = _animationComponent.GetSpriteAnimator() as ISpriteAnimator;
            var animationLength = spriteAnimator.GetCurrentClipLength();
            StartListening(GetEntity());
            SendMessageToSelf(animationLength, Enums.Telegrams.AnimationComplete, UniqueId);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            StopListening();

        }

        //public RQ.AnimationV2.ISpriteRenderer GetSpriteRenderer()
        //{
        //    //var state = GetComponent<StateBase>();
        //    //var stateMachine = state.StateMachine;
        //    //if (stateMachine == null)
        //    //{
        //    //    return null;
        //    //}

        //    //var entity = stateMachine.GetEntity();
        //    //if (entity == null)
        //    //{
        //    //    return null;
        //    //}

        //    //var baseGameEntity = EntityUIBase.GetEntity<IBaseGameEntity>(entity);
        //    //if (baseGameEntity == null)
        //    //{
        //    //    return null;
        //    //}

        //    //var entityUIBase = entity.GetComponent<EntityUIBase>();
        //    //if (entityUIBase == null)
        //    //    return null;

        //    //var componentRepository = stateMachine.GetComponentRepository();


        //    if (_animationComponent == null)
        //    {
        //        var entity = GetRepo();
        //        if (entity == null)
        //            return null;

        //        var animationComponents = entity.Components.GetComponents<AnimationComponent>();

        //        if (animationComponents == null)
        //            return null;

        //        _animationComponent = animationComponents.FirstOrDefault(i => i.IsMain() == true);
        //    }

        //    //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
        //    if (_animationComponent == null)
        //        return null;

        //    return _animationComponent.GetSpriteAnimator();
        //}

        private IComponentRepository GetRepo()
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
            return entity;
        }

        public List<SpriteAnimationType> GetStoredSpriteAnimations()
        {
            if (_iSpriteAnimationsConfig != null)
                return _iSpriteAnimationsConfig.GetStoredSpriteAnimations();

            if (_animationComponent == null)
            {
                var entity = GetRepo();
                //var entity = GetEntity();
                //if (entity == null)
                //{
                //    var runActionsState = GetComponent<RunActionsState>();
                //    //var stateMachine = runActionsState.StateMachine;
                //    if (runActionsState != null)
                //        entity = runActionsState.GetEntity();

                //    //Debug.Log("Could not locate entity");
                //    //return null;
                //}
                //if (entity == null)
                //{
                //    var playActionEvent = GetComponent<PlayActionsEvent>();
                //    if (playActionEvent != null)
                //        entity = playActionEvent.AffectedObject.GetComponent<IComponentRepository>();
                //}
                //if (entity == null)
                //    return null;

                var animationComponents = entity.Components.GetComponents<AnimationComponent>();

                if (animationComponents == null)
                    return null;

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

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;
        //    //var data = agent as ISprite;
        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.AnimationComplete:
        //            if ((string)telegram.ExtraInfo == UniqueId)
        //            {
        //                Log.Info("Animation complete message received for entity " + GetEntity().name);
        //                //var sprite = agent as ISprite;
        //                _animationComponent.Data.IsAnimationComplete = true;
        //            }
        //            // TODO FIX THIS - The state transition table should be determing the next state
        //            //data.GetFSM().RevertToPreviousState();
        //            break;
        //    }
        //    return false;
        //}

        public void StartListening(IComponentRepository entity)
        {
            _animationCompleteIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
            {
                if (string.IsNullOrEmpty(_animation))
                    _isRunning = false;
                _animationComponent.Data.IsAnimationComplete = true;
                //var animation = _animComponent.Get
                if ((string)data.ExtraInfo != _animation)
                    return;
                _isRunning = false;
            });
        }

        public void StopListening(IComponentRepository entity)
        {
            MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        }

        //public override void Serialize(EntitySerializedData entityData)
        //{
        //    base.Serialize(entityData);
        //    //Data.TransformData = transform.Serialize();
        //    AnimationData Data = new AnimationData();
        //    base.SerializeComponent(entityData, Data);

        //    Data.SpriteClipName = _animationComponent.SpriteAnimator2.GetClipName();
        //    Data.HFlip = _animationComponent.SpriteAnimator2.GetHFlip();
        //    Data.AnimationType = _animationType;
        //    var animConfig = _animationComponent.SpriteAnimator2.GetSpriteAnimationsConfig();
        //    //if (animConfig != null)
        //    //    Data.SpriteAnimationsConfigUniqueId = animConfig.UniqueId;
        //    var spriteAnimator = _animationComponent.SpriteAnimator2 as ISpriteAnimator;
        //    if (spriteAnimator != null)
        //        Data.SpriteClipTime = spriteAnimator.GetClipTime();            
        //}

        //public override void Deserialize(EntitySerializedData entityData)
        //{
        //    base.Deserialize(entityData);

        //    var Data = base.DeserializeComponent<AnimationData>(entityData);
        //    _animationType = Data.AnimationType;
        //    //transform.Deserialize(Data.TransformData);

        //    if (_animationComponent.SpriteAnimator2 == null)
        //        return;
        //    _animationComponent.SpriteAnimator2.ProcessDirectionChange(Data.FacingDirection);
        //    _animationComponent.SpriteAnimator2.SetHFlip(Data.HFlip);
        //    _animationComponent.SpriteAnimator2.ProcessHorizontalFlip();
        //    var spriteAnim = _animationComponent.SpriteAnimator2 as ISpriteAnimator;
        //    if (!string.IsNullOrEmpty(Data.SpriteAnimationsConfigUniqueId))
        //    {
        //        //MessageDispatcher2.Instance.DispatchMsg("GetConfig", 0f, this.UniqueId, "Game Controller", null);
        //        var config = ConfigsContainer.Instance.GetConfig<ISpriteAnimationsConfig>(Data.SpriteAnimationsConfigUniqueId);
        //        //var config = _configs[Data.SpriteAnimationsConfigUniqueId] as SpriteAnimationsConfig;
        //        _animationComponent.SpriteAnimator2.SetSpriteAnimationsConfig(config);

        //        if (spriteAnim != null)
        //        {
        //            //spriteAnim.SetSpriteAnimation(config.GetSpriteAnimation());
        //            _animationComponent.SpriteAnimator2.ProcessSpriteAnimations();
        //        }
        //    }

        //    if (spriteAnim != null)
        //    {
        //        spriteAnim.RenderByName(Data.SpriteClipName, Data.SpriteClipTime, Data.HFlip);
        //    }
        //    else
        //    {
        //        _animationComponent.SpriteAnimator2.Render(Data.SpriteClipName);
        //    }
        //}
    }
}
