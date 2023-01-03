using System;
using UnityEngine;

namespace RQ.Model
{
    [Serializable]
    public class TweenToColorInfo
    {
        [SerializeField]
        public Color Color;
        [SerializeField]
        public bool Active = true;
        [SerializeField]
        public float Duration = .25f;
        [SerializeField]
        public float Delay = .01f;

        public TweenToColorInfo()
        { }

        public TweenToColorInfo(Color color, float delay, float duration)
        {
            Delay = delay;
            Duration = duration;
            Color = color;
        }
    }
}
