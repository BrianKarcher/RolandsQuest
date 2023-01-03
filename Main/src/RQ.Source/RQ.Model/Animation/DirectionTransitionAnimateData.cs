using RQ.Common;
using System;

namespace RQ.Model.Animation
{
    [Serializable]
    public class DirectionTransitionAnimateData
    {
        [AnimationTypeAttribute]
        public string AnimationType = null;
        public Direction OldDirection = Direction.None;
        public Direction NewDirection = Direction.None;
        public bool HFlip = false;
    }
}
