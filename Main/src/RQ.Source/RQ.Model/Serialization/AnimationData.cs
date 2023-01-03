using RQ.Common;
using System;
using UnityEngine;

namespace RQ.Model.Serialization
{
    public enum FacingDirectionEnum
    {
        ToVelocity = 0,
        ToTarget = 1,
        ToInputForce = 2
    }

    [Serializable]
    public class AnimationData
    {        
        [SerializeField]
        public DirectionMode FacingDirectionMode = DirectionMode.Automatic;
        [SerializeField]
        public Direction FacingDirection = Direction.Right;
        public Direction PreviousFacingDirection = Direction.None;

        [SerializeField]
        private bool _isAnimationComplete;
        public bool IsAnimationComplete { get { return _isAnimationComplete; } set { _isAnimationComplete = value; } }
        // Next two are used in Serialization
        public string SpriteClipName { get; set; }
        public string AnimationType { get; set; }
        public float SpriteClipTime { get; set; }
        public bool ManualHFlip { get; set; }
        public bool HFlip { get; set; }
        public string SpriteAnimationsConfigUniqueId { get; set; }
        [SerializeField]
        [AnimationTypeAttribute]
        public string AutoPlayAnimationType = null;
        [SerializeField]
        public bool PlayAutomatically = false;

        [SerializeField]
        public bool PlayDefaultOnEnable = false;
        public bool AutoUpdateFacingDirection = true;
        public FacingDirectionEnum AutoFace;
        //public bool IsAnimationComplete { get { return _isAnimationComplete; } set { _isAnimationComplete = value; } }
    }
}
