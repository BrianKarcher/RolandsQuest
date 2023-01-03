using RQ.Animation.V2;
using RQ.AnimationV2;
using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Extensions;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Serialization;
using RQ.Physics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation
{
    [AddComponentMenu("RQ/Components/Animation")]
    public class AnimationComponent : ComponentPersistence<AnimationComponent>, IAnimationComponent, IAnimatedObject
    {
        public AnimationData Data = new AnimationData();
        public RQ.AnimationV2.SpriteRendererBase2 SpriteAnimator2;
        public event Action<Direction, Direction> FacingDirectionChanged;
        [SerializeField]
        private bool _use8Directions = false;
        [SerializeField]
        private bool _isMain = true;
        private long _setFacingDirectionId;
        private long _velocityChangedId;
        private IAIComponent _aIComponent;
        private IBasicPhysicsComponent _physicsComponent;
        private MeshRenderer _meshRenderer;
        private long _setRenderId, _pauseAnimationId, _resumeAnimationId;
        //private Dictionary<string, IRQConfig> _configs;

        private Action<Telegram2> _pauseAnimationDelegate;
        private Action<Telegram2> _resumeAnimationDelegate;
        private Action<Telegram2> _setFacingDirectionDelegate;
        private Action<Telegram2> _velocityChangedDelegate;
        private Action<Telegram2> _setRenderDelegate;

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            //if (GameDataController.Instance.LoadingGame)
            //    return;            

            if (SpriteAnimator2 != null)
            {
                SpriteAnimator2.ProcessDirectionChange(Data.FacingDirection);
            }
            //if (SpriteAnimator2 == null)
            //    Debug.LogError($"{_componentRepository.name} does not contain a Sprite Animator");
            // Notify for the inital direction
            NotifyFacingDirectionChanged();
            if (Data.PlayAutomatically)
            {
                Animate(Data.AutoPlayAnimationType);
            }
        }

        public override void Awake()
        {
            base.Awake();
            //if (GameDataController.Instance.LoadingGame)
            //    return;
            Data.IsAnimationComplete = false;
            _pauseAnimationDelegate = (data) =>
            {
                var spriteAnimator = SpriteAnimator2;
                if (spriteAnimator != null)
                    spriteAnimator.Pause();
            };
            _resumeAnimationDelegate = (data) =>
            {
                var spriteAnimator = SpriteAnimator2;
                if (spriteAnimator != null)
                    spriteAnimator.Resume();
            };
            _setFacingDirectionDelegate = (data) =>
            {
                var direction = (Direction) data.ExtraInfo;
                SetFacingDirection(direction);
                ProcessDirectionChange(direction);
            };
            _velocityChangedDelegate = (data) =>
            {
                SetFacingDirection((Vector2D) data.ExtraInfo);
            };
            _setRenderDelegate = (data) =>
            {
                var render = (bool) data.ExtraInfo;
                SetRender(render);
            };
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            if (Data.PlayDefaultOnEnable)
                Animate(Data.AutoPlayAnimationType);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;
        }

        public override void StartListening()
        {
            base.StartListening();
            if (!Application.isPlaying)
                return;
            //_componentRepository.StartListening("SetRender", this.UniqueId, (data) =>
            //{
            //    var render = data.ExtraInfo.ToString() == "1";
            //    SetRender(render);
            //});
            _pauseAnimationId =
                MessageDispatcher2.Instance.StartListening("PauseAnimation", _componentRepository.UniqueId, _pauseAnimationDelegate);
            //_componentRepository.StartListening("PauseAnimation", this.UniqueId, _pauseAnimationDelegate);
            _resumeAnimationId =
                MessageDispatcher2.Instance.StartListening("ResumeAnimation", _componentRepository.UniqueId, _resumeAnimationDelegate);
            //_componentRepository.StartListening("ResumeAnimation", this.UniqueId, _resumeAnimationDelegate);
            _setFacingDirectionId = MessageDispatcher2.Instance.StartListening("SetFacingDirection", _componentRepository.UniqueId, _setFacingDirectionDelegate);
            _velocityChangedId = MessageDispatcher2.Instance.StartListening("VelocityChanged", _componentRepository.UniqueId, _velocityChangedDelegate);
            _setRenderId = MessageDispatcher2.Instance.StartListening("SetRender", _componentRepository.UniqueId, _setRenderDelegate);
            //MessageDispatcher2.Instance.StartListening("SetConfig", this.UniqueId, (data) =>
            //{
            //    _configs = data.ExtraInfo as Dictionary<string, IRQConfig>;
            //});
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("PauseAnimation", _componentRepository.UniqueId, _pauseAnimationId);
            //_componentRepository.StopListening("PauseAnimation", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("ResumeAnimation", _componentRepository.UniqueId, _resumeAnimationId);
            //_componentRepository.StopListening("ResumeAnimation", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetFacingDirection", _componentRepository.UniqueId, _setFacingDirectionId);
            MessageDispatcher2.Instance.StopListening("VelocityChanged", _componentRepository.UniqueId, _velocityChangedId);
            MessageDispatcher2.Instance.StopListening("SetRender", _componentRepository.UniqueId, _setRenderId);

            //MessageDispatcher2.Instance.StopListening("SetConfig", this.UniqueId);
        }

        public virtual string GetIdByType(string type)
        {
            var spriteRenderer = SpriteAnimator2;
            return spriteRenderer.GetIdByType(type);
            //SpriteAnimator2.RenderByName()
            //if (spriteRenderer != null)
            //{
            //    spriteRenderer.RenderByName()
            //    //SpriteAnimator2.Render(id, Data.FacingDirection);
            //}
        }

        public virtual void Animate(string id)
        {
            if (SpriteAnimator2 == null)
                return;

            SpriteAnimator2.Render(id, Data.FacingDirection);
            var spriteAnimator = SpriteAnimator2;
            var animationLength = spriteAnimator.GetCurrentClipLength();
            //SendMessageToSelf(animationLength, Enums.Telegrams.AnimationComplete, UniqueId);
            var animations = GetStoredSpriteAnimations();
            if (animations == null)
                return;
            for (int i = 0; i < animations.Count; i++)
            {
                var anim = animations[i];
                if (anim.ID == id)
                {
                    MessageDispatcher2.Instance.DispatchMsg("AnimationComplete", animationLength, this.UniqueId,
                        _componentRepository.UniqueId, anim.Type /*SpriteAnimator2.GetClipName()*/);
                    break;
                }
            }
        }

        public virtual void StopAnimation()
        {
            var anim = SpriteAnimator2;
            if (anim != null)
                anim.StopAnim();
        }

        public ISpriteRenderer GetSpriteAnimator()
        {
            return SpriteAnimator2 as ISpriteRenderer;
        }

        public Direction GetFacingDirection()
        {
            return Data.FacingDirection;
        }

        /// <summary>
        /// Sets the facing direction
        /// </summary>
        /// <param name="direction">Direction.</param>
        public virtual void SetFacingDirection(Direction facingDirection)
        {
            // Kick off the event
            if (FacingDirectionChanged != null && facingDirection != Data.FacingDirection)
                FacingDirectionChanged(facingDirection, Data.FacingDirection);
            //Debug.Log(_componentRepository.name + " setting facing direction to " + facingDirection);
            Data.FacingDirection = facingDirection;
            ProcessDirectionChange(facingDirection);
            if (SpriteAnimator2 != null)
                SpriteAnimator2.Render(Data.FacingDirection);
        }

        // Returns true if the direction changed
        public virtual bool SetFacingDirection(Vector2D velocity)
        {
            if (SpriteAnimator2 == null)
                return true;
            // Don't change directions if in Manual mode
            if (Data.FacingDirectionMode == DirectionMode.Manual)
                return true;
            if (velocity.isZero())
                return true;

            Direction newDirection;
            if (_use8Directions)
                newDirection = velocity.GetDirection8();
            else
                newDirection = velocity.GetDirection();
            // If the direction changed, set it
            if (newDirection != Direction.None && newDirection != Data.FacingDirection)
            {
                Data.PreviousFacingDirection = Data.FacingDirection;
                //Debug.Log("(" + _componentRepository.name + ") Setting direction to " + newDirection);
                //Debug.Log(_componentRepository.name + " setting facing direction to " + newDirection);
                Data.FacingDirection = newDirection;
                // It's a new direction, gotta render it!
                ProcessDirectionChange(newDirection);
                SpriteAnimator2.Render(Data.FacingDirection);
                if (FacingDirectionChanged != null)
                    FacingDirectionChanged(Data.PreviousFacingDirection, Data.FacingDirection);

                // If the object needs to physically rotate, rotate it to point to the new Direction
                SendMessageToSpriteBase(0f, Enums.Telegrams.RotateObjectToDirection, newDirection);
                NotifyFacingDirectionChanged();
                return true;
            }
            return false;
        }

        private void NotifyFacingDirectionChanged()
        {
            MessageDispatcher2.Instance.DispatchMsg("FacingDirectionChanged", 0f, this.UniqueId,
                _componentRepository.UniqueId, Data.FacingDirection);
        }

        public virtual void ProcessDirectionChange(Direction newDirection)
        {
            //Debug.Log(_componentRepository.name + " setting facing direction to " + newDirection);
            Data.FacingDirection = newDirection;
            if (SpriteAnimator2 == null)
                return;
            SpriteAnimator2.ProcessDirectionChange(newDirection);
        }

        public virtual Vector3 GetFacingDirectionVector()
        {
            return Data.FacingDirection.ToVector();
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.HeadingChanged:
                    SetFacingDirection((Vector2D)msg.ExtraInfo);
                    break;
                //case Enums.Telegrams.AltitudeChanged:
                //    if (SpriteAnimator2 != null)
                //        SpriteAnimator2.SetPosition((Vector2D)msg.ExtraInfo);
                //    break;
                case Enums.Telegrams.SetRender:
                    var render = (bool) msg.ExtraInfo;
                    SetRender(render);
                    break;
                case Enums.Telegrams.SetFacingDirection:
                    var direction = (Direction) msg.ExtraInfo;
                    SetFacingDirection(direction);
                    ProcessDirectionChange(direction);
                    break;
            }
            return false;
        }

        public void SetRender(bool render)
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            //Debug.LogError($"(AnimationComponent) Setting Render to {render}");
            _meshRenderer.enabled = render;
        }

        public void SetManualHFlip(bool manualHFlip)
        {
            Data.ManualHFlip = manualHFlip;
            SpriteAnimator2.SetManualHFlip(manualHFlip);
        }

        public void SetHFlip(bool hFlip)
        {
            Data.HFlip = hFlip;
            SpriteAnimator2.SetHFlip(hFlip);
            SpriteAnimator2.ProcessHorizontalFlip();
        }

        public void LateUpdate()
        {
            if (!Data.AutoUpdateFacingDirection)
                return;
            switch (Data.AutoFace)
            {
                case FacingDirectionEnum.ToTarget:
                    if (_aIComponent == null)
                        _aIComponent = GetComponentRepository().Components.GetComponent<IAIComponent>();
                    if (_aIComponent == null)
                        return;
                    var target = _aIComponent.GetTarget();
                    if (target == null)
                        return;
                    SetFacingDirection(target.transform.position - GetComponentRepository().transform.position);
                    break;
                case FacingDirectionEnum.ToVelocity:
                    if (_physicsComponent == null)
                        _physicsComponent = GetComponentRepository().Components.GetComponent<IBasicPhysicsComponent>();
                    if (_physicsComponent == null)
                        return;
                    SetFacingDirection(_physicsComponent.GetVelocity());
                    break;
                case FacingDirectionEnum.ToInputForce:
                    if (_physicsComponent == null)
                        _physicsComponent = GetComponentRepository().Components.GetComponent<IBasicPhysicsComponent>();
                    if (_physicsComponent == null)
                        return;
                    SetFacingDirection(_physicsComponent.GetPhysicsAffector("Input").Force);
                    break;
            }
        }

        /// <summary>
        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
        /// may populate different parts of this object
        /// </summary>
        /// <param name="entityData"></param>
        //public override void Serialize(EntitySerializedData entityData)
        //{
        //    base.Serialize(entityData);
        //    //Data.TransformData = transform.Serialize();
        //    if (SpriteAnimator2 == null)
        //        return;

        //    base.SerializeComponent(entityData, Data);
        //    //if (SpriteAnimator2 == null)
        //    //    Debug.LogError("SpriteAnimator2 is null in " + _componentRepository.name);
        //    Data.SpriteClipName = SpriteAnimator2.GetClipName();
        //    //Data.HFlip = SpriteAnimator2.GetSprite().FlipX;
        //    Data.HFlip = SpriteAnimator2.GetHFlip();
        //    var animConfig = SpriteAnimator2.GetSpriteAnimationsConfig();
        //    if (animConfig != null)
        //        Data.SpriteAnimationsConfigUniqueId = animConfig.GetUniqueId();
        //    if (SpriteAnimator2 is ISpriteAnimator)
        //    {
        //        Data.SpriteClipTime = (SpriteAnimator2 as ISpriteAnimator).GetClipTime();
        //    }
        //}

        //public override void Deserialize(EntitySerializedData entityData)
        //{
        //    base.Deserialize(entityData);
        //    if (SpriteAnimator2 == null)
        //        return;
        //    if (entityData.UniqueId == "d3f8fcfa-fef4-4165-aff0-908c95dd685e")
        //    {
        //        int i = 1;
        //    }

        //    Data = base.DeserializeComponent<AnimationData>(entityData);
        //    //transform.Deserialize(Data.TransformData);

        //    //if (SpriteAnimator2 == null)
        //    //    return;
        //    SpriteAnimator2.ProcessDirectionChange(Data.FacingDirection);
        //    //SpriteAnimator2.GetSprite().FlipX = Data.HFlip;

        //    SpriteAnimator2.SetHFlip(Data.HFlip);
        //    SpriteAnimator2.ProcessHorizontalFlip();
        //    if (!string.IsNullOrEmpty(Data.SpriteAnimationsConfigUniqueId))
        //    {
        //        //MessageDispatcher2.Instance.DispatchMsg("GetConfig", 0f, this.UniqueId, "Game Controller", null);
        //        var config = ConfigsContainer.Instance.GetConfig<ISpriteAnimationsConfig>(Data.SpriteAnimationsConfigUniqueId);
        //        //var config = _configs[Data.SpriteAnimationsConfigUniqueId] as SpriteAnimationsConfig;
        //        SpriteAnimator2.SetSpriteAnimationsConfig(config);
        //        var spriteAnim = SpriteAnimator2 as ISpriteAnimator;
        //        if (spriteAnim != null)
        //        {
        //            spriteAnim.SetSpriteAnimation(config);
        //            SpriteAnimator2.ProcessSpriteAnimations();
        //        }
        //    }
            
        //    if (SpriteAnimator2 is ISpriteAnimator)
        //    {
        //        (SpriteAnimator2 as ISpriteAnimator).RenderByName(Data.SpriteClipName, Data.SpriteClipTime, Data.HFlip);
        //    }
        //    else
        //    {
        //        SpriteAnimator2.Render(Data.SpriteClipName);
        //    }
        //}

        public bool IsMain()
        {
            return _isMain;
        }

        public AnimationComponent GetAnimationComponent()
        {
            return this;
        }

        public RQ.AnimationV2.ISpriteRenderer GetSpriteRenderer()
        {
            return GetSpriteAnimator();
        }

        public List<SpriteAnimationType> GetStoredSpriteAnimations()
        {
            return GetSpriteAnimator().GetStoredSpriteAnimations();
        }

        public string GetUniqueIdForType(string type)
        {
            var animConfig = SpriteAnimator2.GetSpriteAnimationsConfig();
            var spriteAnimations = animConfig.GetStoredSpriteAnimations();
            //if (spriteAnimations == null || spriteAnimations.Count == 0)
            SpriteAnimationType animType = null;
            for (int i = 0; i < spriteAnimations.Count; i++)
            {
                var anim = spriteAnimations[i];
                if (anim.Type == type)
                {
                    animType = anim;
                    break;
                }
            }

            //var anim = spriteAnimations.FirstOrDefault(i => i.Type == type);
            if (animType == null)
                throw new Exception("Could not locate animation type " + type + " for " + name);
            return animType.ID;
        }
    }
}
