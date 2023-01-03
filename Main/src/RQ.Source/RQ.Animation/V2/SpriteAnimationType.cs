using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AnimationV2
{
    [Serializable]
    public class SpriteAnimationType
    {
        [SerializeField]
        private string _type;
        public string Type { get { return _type; } set { _type = value; } }

        [SerializeField]
        private string _id;
        public string ID { get { return _id; } set { _id = value; } }

        [SerializeField]
        private List<SpriteAnimation> _spriteAnimations = new List<SpriteAnimation>();
        public List<SpriteAnimation> SpriteAnimations { get { return _spriteAnimations; } set { _spriteAnimations = value; } }

        //public string ID;
        //[SerializeField]
        //private string _type;
        //public string Type { get { return _type; } set { _type = value; } }
        //[SerializeField]
        //private Direction _direction;
        //public Direction Direction { get { return _direction; } set { _direction = value; } }
        //[SerializeField]
        //private string _animationName;
        //public string AnimationName { get { return _animationName; } set { _animationName = value; } }
        //[SerializeField]
        //private int _clipId;
        //public int ClipId { get { return _clipId; } set { _clipId = value; } }
    }
}
