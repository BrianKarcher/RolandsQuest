using RQ.Animation;
using RQ.Animation.V2;
using RQ.Common.Config;
using RQ.Physics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AnimationV2
{
    public abstract class SpriteRendererBase2 : MonoBehaviour, ISpriteRenderer
    {
        // TODO see if I can order this so I can do binary searches
        [SerializeField]
        [Obsolete]
        private List<SpriteAnimationType> _storedSpriteAnimations = new List<SpriteAnimationType>();

        [SerializeField]
        protected RQBaseConfig _spriteAnimationsConfig;
        private ISpriteAnimationsConfig _iSpriteAnimationsConfig;

        public abstract void StopAnim();
        public abstract void Pause();
        public abstract void Resume();
        public abstract float GetCurrentClipLength();
        public event Action AnimComplete;

        [SerializeField]
        private bool _manualHFlip = false;
        [SerializeField]
        private bool _hFlip = false;

        // Dictionaries are not serialized by Unity, this will be created in Awake
        private Dictionary<string, List<SpriteAnimation>> _spriteAnimations;
        //private List<SpriteAnimation> spriteAnimations;

        [SerializeField]
        private bool _directionRotate = false;
        public bool DirectionRotate { get { return _directionRotate; } set { _directionRotate = value; } }
        //public AnchorOffset2 AnchorOffset;

        [HideInInspector]
        public Vector2D OrigSpritePosition;
        //[HideInInspector]
        //public Vector2D AnchorOffsetPos;
        [HideInInspector]
        public Vector2D SpriteOffsetPos;

        //protected IBaseGameEntity _sprite;
        //protected string _spriteName;
        protected string _previousRender;
        //protected Transform _transform;
        //protected tk2dBaseSprite _baseSprite;
        //private Direction _direction;

        public Direction Direction { get; set; }
        //[SerializeField]
        protected string _currentID;
        //protected AnimationType _animationType;

        protected string _currentRender;

        //public event Action<Direction, Direction> FacingDirectionChanged;
        //public abstract void Awake(/*IBaseGameEntity sprite*/);
        //{
        //    Awake(sprite, sprite.GetEntityUI().GetTransform());
        //}

        public virtual void Awake(/*IBaseGameEntity sprite, */Transform transform)
        {
            _spriteAnimations = new Dictionary<string,List<SpriteAnimation>>();

            ProcessSpriteAnimations();
            //this._transform = transform;
            OrigSpritePosition = GetPosition();
            SetISpriteAnimationsConfig();
            
            
            //_sprite = sprite;
            //_spriteName = _sprite.GetName();
            //if (AnchorOffset != null)
            //    AnchorOffset.ProcessAwake(_sprite);
            //if (AnchorOffset != null)
            //    AnchorOffset.Init();
            
        }

        public void ProcessSpriteAnimations()
        {
            List<SpriteAnimationType> spriteTypes = _iSpriteAnimationsConfig != null ?
                _iSpriteAnimationsConfig.GetStoredSpriteAnimations() : _storedSpriteAnimations;

            if (spriteTypes == null)
                return;
            // It may be null if the object is disabled, this Awake is not called yet.
            if (_spriteAnimations == null)
                _spriteAnimations = new Dictionary<string, List<SpriteAnimation>>();
            _spriteAnimations.Clear();
            foreach (var kvp in spriteTypes)
            {
                _spriteAnimations.Add(kvp.ID, kvp.SpriteAnimations);
            }
        }

        public string GetIdByType(string type)
        {
            List<SpriteAnimationType> spriteTypes = _iSpriteAnimationsConfig != null ?
            _iSpriteAnimationsConfig.GetStoredSpriteAnimations() : _storedSpriteAnimations;
            foreach (var spriteType in spriteTypes)
            {
                if (spriteType.Type == type)
                {
                    return spriteType.ID;
                }
            }
            //var spriteType = spriteTypes.FirstOrDefault(i => i.Type == type);
            //if (spriteType == null)
            //    return null;
            //return spriteType.ID;
            return null;
        }

        //public virtual void Start()
        //{

        //}

        //protected void SetBaseSprite(tk2dBaseSprite sprite)
        //{
        //    _baseSprite = sprite;
        //}

        public abstract string GetClipName();

        public virtual Vector2D GetOrigSpritePosition()
        {
            return OrigSpritePosition;
        }

        public abstract void SetColor(string name, Color color);

        public abstract void RemoveColor(string name);

        public void SetMaterial(Material material)
        {
            var textures = (this as ISpriteAnimator).GetTextures();
            var rendererComponent = GetComponent<Renderer>();
            var materials = rendererComponent.materials;
            materials[0] = material;
            rendererComponent.materials = materials;
            var meshRenderer = GetComponent<Renderer>();
            meshRenderer.material = material;
            meshRenderer.material.SetTexture("_MainTex", textures[0]);
            //anim.Sprite.
        }

        public virtual void ProcessDirectionChange(Direction direction)
        {
            SetDirection(direction);
            //ProcessRotation(direction);
            //Direction = direction;
            // Determine whether to flip the scale
            if (_directionRotate)
                return;

            if (!_manualHFlip)
            {
                CalculateHorizontalFlip(direction);
                ProcessHorizontalFlip();
            }

            // Sometimes the direction changes but the animation stays the same, so reapply the offset
            //SetOffset(_currentRender, true);
        }

        public void SetManualHFlip(bool manualHFlip)
        {
            _manualHFlip = manualHFlip;
        }

        public void SetHFlip(bool hFlip)
        {
            _hFlip = hFlip;
            //_baseSprite.FlipX = _hFlip;
        }

        public bool GetHFlip()
        {
            return _hFlip;
        }

        //public tk2dBaseSprite GetSprite()
        //{
        //    return _baseSprite;
        //}

        public void CalculateHorizontalFlip(Direction direction)
        {
            if (direction == Direction.Left || direction == RQ.Direction.UpLeft ||
                direction == RQ.Direction.DownLeft)
            {
                // Left is special, we are flipping the sprite so we just reverse the x scale
                _hFlip = true;
            }
            else
            {
                // For all other directions, the scale should be positive, so reverse it if it is negative
                _hFlip = false;
            }
        }

        public void ProcessHorizontalFlip()
        {
            //_baseSprite.FlipX = _hFlip;
            if (_hFlip)
            {
                // Left is special, we are flipping the sprite so we just reverse the x scale
                if (transform.localScale.x > 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // For all other directions, the scale should be positive, so reverse it if it is negative
                if (transform.localScale.x < 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        protected void InvokeAnimComplete()
        {
            AnimComplete?.Invoke();
        }

        public virtual void ProcessRotation(Direction newDirection)
        {
            if (_directionRotate)
            {
                transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                    newDirection.GetDirectionAngle());
            }
        }

        public virtual void SetDirection(Direction direction)
        {
            Direction = direction;
            ProcessRotation(direction);
        }

        // TODO: Put this into an extension method
        //public virtual float GetDirectionAngle(Direction direction)
        //{
        //    switch (direction)
        //    {
        //        case RQ.Direction.Right:
        //            return 0f;
        //        case RQ.Direction.Up:
        //            return 90f;
        //        case RQ.Direction.Left:
        //            return 180f;
        //        default:
        //            return 270f;
        //    }
        //}

        //public virtual void SetOffset(Offset offset, bool applyOffsetToSprite = true)
        //{
        //    //string name = //SpriteManager.transform.parent == null ? SpriteManager.transform.name : SpriteManager.transform.parent.name;
        //    //Log.Info(_spriteName + " SpriteAnimator.SetOffset offset = " + offset.ToSafeString() + " applyOffsetToSprite = " + applyOffsetToSprite);
        //    if (AnchorOffset != null)
        //        AnchorOffset.SetAnchorOffset(offset, applyOffsetToSprite);
        //}

        //public virtual void SetOffset(string name, bool setSpriteDelta = true)
        //{
        //    //Log.Info(_spriteName + " SpriteAnimator3.SetOffset " + name);
        //    if (AnchorOffset != null)
        //    {
        //        Direction direction = _sprite.GetDirection();
        //        AnchorOffset.SetAnchorOffset(direction, name, setSpriteDelta);
        //    }
        //    //_anchorOffset.SetAnchorOffset(_direction, name);
        //}

        //public virtual Offset GetOffset()
        //{
        //    if (AnchorOffset != null)
        //        return AnchorOffset.GetAnchorOffset();

        //    return null;
        //}

        public Vector3 GetPosition()
        {
            return transform.localPosition;
        }

        protected Vector2D ProcessPosition()
        {
            return OrigSpritePosition + SpriteOffsetPos /*+ AnchorOffsetPos*/;
        }

        public void SetPosition(Vector3 localPosition)
        {
            //string name = string.Empty; //SpriteManager.name;
            //string name = _spriteName;
            //string name = SpriteManager.transform.parent == null ? SpriteManager.transform.name : SpriteManager.transform.parent.name;
            //Log.Info(name + " SpriteAnimator.SetPosition localPosition = " + localPosition.ToString("F4"));
            transform.localPosition = localPosition;
        }

        public void SetPosition(Vector2D localPosition)
        {
            //string name = string.Empty; //SpriteManager.name;
            //string name = _spriteName;
            //string name = SpriteManager.transform.parent == null ? SpriteManager.transform.name : SpriteManager.transform.parent.name;
            //Log.Info(name + " SpriteAnimator.SetPosition localPosition = " + localPosition.ToString("F4"));
            transform.localPosition = localPosition.ToVector3(
                transform.localPosition.z);
        }

        //public void SetAnchorOffsetPos(Vector2D pos)
        //{
        //    AnchorOffsetPos = pos;
        //    SetPosition(ProcessPosition());
        //}

        public void SetSpriteOffsetPos(Vector2D pos)
        {
            SpriteOffsetPos = pos;
            SetPosition(ProcessPosition());
        }

        public bool RenderPrevious()
        {
            if (_previousRender == null)
                return false;
            return Render(_previousRender);
        }

        public SpriteAnimation GetAnimation(string id, Direction direction)
        {
            if (String.IsNullOrEmpty(id))
                return null;

            if (_spriteAnimations == null || _spriteAnimations.Count == 0)
            {
                //Debug.LogError(this.name + " Could not locate Sprite Animations");
                ProcessSpriteAnimations();
                if (_spriteAnimations.Count == 0)
                {
                    Debug.LogError($"{this.name} Could not locate Sprite Animations");
                    return null;
                }
                //return null;
            }

            //if (!_spriteAnimations.Any())
            //{
            //    return null;
            //}
            List<SpriteAnimation> animations;
            _spriteAnimations.TryGetValue(id, out animations);
            if (animations == null)
            {
                Debug.LogError("No animations found in " + this.transform.parent + " for animation " + id);
            }
            for (int i = 0; i < animations.Count; i++)
            {
                var spriteAnimation = animations[i];
                if (spriteAnimation.Direction == direction)
                {
                    return spriteAnimation;
                }
            }
            // Could not find an animation type and direction match, attempt to find an "any" direction for
            // this animation type
            foreach (var animation in animations)
            {
                if (animation.Direction == RQ.Direction.Any)
                    return animation;
            }
            //var anyDirectionAnimation = animations.FirstOrDefault(i => i.Direction == RQ.Direction.Any);
            //if (anyDirectionAnimation != null)
            //{
            //    return anyDirectionAnimation;
            //}

            return null;
        }

        public string GetAnimationID()
        {
            return _currentID;
        }

        public List<SpriteAnimation> GetSpriteAnimations(string id)
        {
            return _spriteAnimations[id];
        }

        public void SetSpriteAnimationsConfig(ISpriteAnimationsConfig spriteAnimationsConfig)
        {
            _iSpriteAnimationsConfig = spriteAnimationsConfig;
            //_spriteAnimationsConfig = spriteAnimationsConfig;
        }

        public ISpriteAnimationsConfig GetSpriteAnimationsConfig()
        {
            return _iSpriteAnimationsConfig;
        }

        public List<SpriteAnimationType> GetStoredSpriteAnimations()
        {
            if (_iSpriteAnimationsConfig == null)
                SetISpriteAnimationsConfig();
            if (_iSpriteAnimationsConfig != null)
                return _iSpriteAnimationsConfig.GetStoredSpriteAnimations();
            return _storedSpriteAnimations;
        }

        private void SetISpriteAnimationsConfig()
        {
            _iSpriteAnimationsConfig = _spriteAnimationsConfig as ISpriteAnimationsConfig;
        }

        public List<SpriteAnimationType> GetThisStoredSpriteAnimations()
        {
            //if (_spriteAnimationsConfig != null)
            //    return _spriteAnimationsConfig.GetStoredSpriteAnimations();
            return _storedSpriteAnimations;
        }

        public abstract bool RenderByName(SpriteAnimation animation);
        public virtual bool Render(string id)
        {
            //_animationType = animationType;
            return Render(id, Direction);
        }
        public virtual bool Render(Direction direction)
        {
            //_animationType = animationType;
            return Render(_currentID, direction);
        }
        public abstract bool Render(string id, Direction direction);
        // Obsolete
        public abstract string[] GetAllClipNames();
        //public abstract tk2dBaseSprite GetBaseSprite();
        [Obsolete]
        public abstract int GetClipIdByName(string name);
        //public abstract float GetClipLength(int clipId);
    }
}
