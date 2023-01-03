using System;
using UnityEngine;

namespace RQ.AnimationV2
{
    [Serializable]
    public class SpriteAnimation
    {
        //public string ID;
        //[SerializeField]
        //private string _type;
        //public string Type { get { return _type; } set { _type = value; } }
        [SerializeField]
        private Direction _direction;
        public Direction Direction { get { return _direction; } set { _direction = value; } }
        [SerializeField]
        private string _animationName;
        public string AnimationName { get { return _animationName; } set { _animationName = value; } }
        [SerializeField]
        private int _clipId;
        public int ClipId { get { return _clipId; } set { _clipId = value; } }
        public bool hFlip = false;
    }
}
