using RQ.Animation;
using RQ.Animation.V2;
using RQ.Common;
using RQ.Common.Config;
using RQ.Messaging;
using System;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Animator")]
    public class AnimatorState : Active, IAnimatedObject //<ISprite>
    {
        //private AnimatorState() { }

        //public AnimationType AnimationType;

        [SerializeField]
        [AnimationTypeAttribute]
        private string _animationType = null;

        [SerializeField]
        protected AnimationComponent _animationComponent;
        [SerializeField]
        protected RQBaseConfig _spriteAnimationsConfig;
        //private ISpriteAnimationsConfig _iSpriteAnimationsConfig;
        private Action<Telegram2> _animationCompleteMessageDelegate;

        protected AnimationComponent AnimationComponent
        {
            get
            {
                if (_animationComponent == null)
                {
                    var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                    if (animationComponents != null)
                    {
                        //foreach (var baseObject in animationComponents)
                        for (int i = 0; i < animationComponents.Count; i++)
                        {
                            var baseObject = animationComponents[i];
                            var animationComponent = baseObject as AnimationComponent;
                            if (animationComponent == null)
                                continue;
                            if (animationComponent.IsMain())
                            {
                                _animationComponent = animationComponent;
                                break;
                            }
                        }
                        //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                    }
                }
                return _animationComponent;
            }
        }

        public override void Enter()
        {
            base.Enter();
            //if (_componentRepository.name.Contains("BombExplosion"))
            //{
            //    Log.Info(GetInstanceID() + "Animator State Enter (BombExplosion)");
            //    int i = 1;
            //}
            //Log.Info(GetInstanceID() + "Entering Animator state for entity " + _spriteBaseComponent.name);
            StartAnimation();
        }

        public override void Awake()
        {
            base.Awake();
            //_iSpriteAnimationsConfig = _spriteAnimationsConfig as ISpriteAnimationsConfig;
            // Caching the delegate
            _animationCompleteMessageDelegate = AnimationCompleteMessage;
        }

        protected virtual void StartAnimation()
        {
            if (AnimationComponent == null)
                return;
            if (!String.IsNullOrEmpty(_animationType))
            {
                AnimationComponent.Animate(_animationType);
                AnimationComponent.Data.IsAnimationComplete = false;
                var animationLength = GetClipLength();
                if (animationLength != 0f)
                    MessageDispatcher2.Instance.DispatchMsg("AnimationComplete", animationLength, this.UniqueId, this.UniqueId, this.UniqueId);

            }
            //SendMessageToSelf(animationLength, Enums.Telegrams.AnimationComplete, UniqueId);
        }

        protected float GetClipLength()
        {
            var spriteAnimator = GetSpriteAnimator();
            if (spriteAnimator == null)
                return 0f;
            var animationLength = spriteAnimator.GetCurrentClipLength();
            return animationLength;
        }

        protected ISpriteAnimator GetSpriteAnimator()
        {
            var spriteAnimator = _animationComponent.GetSpriteAnimator() as ISpriteAnimator;            
            return spriteAnimator;
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("AnimationComplete", this.UniqueId, _animationCompleteMessageDelegate);
        }

        public void AnimationCompleteMessage(Telegram2 data)
        {
            //if (_componentRepository.name.Contains("BombExplosion"))
            //{
            //    Log.Info(GetInstanceID() + "Animator State AnimationComplete (BombExplosion)");
            //    int i = 1;
            //}
            if ((string)data.ExtraInfo != UniqueId)
                return;
            //{
            if (AnimationComponent.Data == null)
            {
                throw new Exception("(" + GetEntity().GetName() + ") Animation Data is null");
            }
            //Log.Info("Animation complete message received for entity " + _spriteBaseComponent.name);
            //var sprite = agent as ISprite;
            AnimationComponent.Data.IsAnimationComplete = true;
            //}
            // TODO FIX THIS - The state transition table should be determing the next state
            //data.GetFSM().RevertToPreviousState();
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("AnimationComplete", this.UniqueId, -1);
        }

        //public override void Exit()
        //{
        //    base.Exit();
        //    //Log.Info(GetInstanceID() + "Exiting Animator state for entity " + _spriteBaseComponent.name);
        //    //if (_componentRepository.name.Contains("BombExplosion"))
        //    //{
        //    //    Log.Info(GetInstanceID() + "Animator State Exit (BombExplosion)");
        //    //    int i = 1;
        //    //}
        //    //MessageDispatcher2.Instance.RemoveMessages("AnimationComplete", this.UniqueId);
        //    //MessageDispatcher2.Instance.RemoveMessagesForReceiverId(this.UniqueId);
        //    // Remove the message in case we exit early
        //    //MessageDispatcher.Instance.RemoveFromQueue(i => i.ReceiverId == this.UniqueId &&
        //    //    i.Msg == Enums.Telegrams.AnimationComplete && ((string)i.ExtraInfo) == this.UniqueId);
        //    //agent.Animator.RenderPrevious();
        //}

        //public override void Update(IRQObject agent)
        //{

        //}

        //public override void FixedUpdate(IRQObject agent)
        //{

        //}

        public RQ.AnimationV2.ISpriteRenderer GetSpriteRenderer()
        {
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

            if (_animationComponent == null)
            {
                var entity = GetEntity();
                if (entity == null)
                {
                    //var stateMachine = StateMachine;
                    if (StateMachine == null)
                    {
                        //Debug.Log("Could not locate State Machine");
                        return null;
                    }
                    var componentRepository = StateMachine.GetComponentRepository();

                    if (componentRepository == null)
                        return null;
                }
                var animationComponents = entity.Components.GetComponents<AnimationComponent>();
                if (animationComponents != null)
                {
                    foreach (var component in animationComponents)
                    {
                        var animComponent = component as AnimationComponent;
                        if (animComponent.IsMain())
                        {
                            _animationComponent = animComponent;
                            break;
                        }
                    }
                    //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                }
                //_animationComponent = entity.Components.GetComponents<AnimationComponent>().FirstOrDefault(i => i.IsMain());
            }

            //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
            if (_animationComponent == null)
                return null;

            return _animationComponent.GetSpriteAnimator();
        }

        //public List<SpriteAnimationType> GetStoredSpriteAnimations()
        //{
        //    if (_iSpriteAnimationsConfig != null)
        //        return _iSpriteAnimationsConfig.GetStoredSpriteAnimations();

        //    if (_animationComponent == null)
        //    {
        //        var entity = GetEntity();
        //        if (entity == null)
        //        {
        //            //var stateMachine = StateMachine;
        //            if (StateMachine == null)
        //            {
        //                //Debug.Log("Could not locate State Machine");
        //                return null;
        //            }
        //            var componentRepository = StateMachine.GetComponentRepository();

        //            if (componentRepository == null)
        //                return null;
        //        }
        //        _animationComponent = entity.Components.GetComponents<AnimationComponent>().FirstOrDefault(i => i.IsMain());
        //    }

        //    //var baseGameEntity = entityUIBase.GetEntity() as IBaseGameEntity;
        //    if (_animationComponent == null)
        //        return null;

        //    return _animationComponent.GetSpriteAnimator().GetStoredSpriteAnimations();
        //}

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
                    
        //            break;
        //    }
        //    return false;
        //}

        //public override void Serialize(StateData stateData)
        //{
        //    base.Serialize(stateData);
        //    var animatorStateData = new AnimatorStateData();
        //    animatorStateData.AnimationType = _animationType;
        //    if (_animationComponent != null)
        //        animatorStateData.AnimationComponentUniqueId = _animationComponent.UniqueId;
        //    //base.SerializeComponent(entitySerializedData, animatorStateData);
        //    stateData.DataObjects.Add("AnimatorState", animatorStateData);
        //}

        //public override void Deserialize(StateData stateData)
        //{
        //    base.Deserialize(stateData);
        //    object tempanimatorStateData;
        //    if (!stateData.DataObjects.TryGetValue("AnimatorState", out tempanimatorStateData))
        //        return;
        //    //AnimatorStateData animatorStateData = (AnimatorStateData)tempanimatorStateData;

        //    //var animatorStateData = base.DeserializeComponent<AnimatorStateData>(entitySerializedData);
        //    var animatorStateData = Persistence.DeserializeObject<AnimatorStateData>(tempanimatorStateData);
        //    _animationType = animatorStateData.AnimationType;
        //    _animationComponent = GetEntity().Components.GetComponents<AnimationComponent>()
        //        .FirstOrDefault(i => i.UniqueId == animatorStateData.AnimationComponentUniqueId);
        //}
    }
}
