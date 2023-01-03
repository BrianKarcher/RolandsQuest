using RQ.Animation.V2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation
{
    public interface ISpriteAnimator
    {
        float GetCurrentClipLength();
        void Pause();
        void StopAnim();
        void Resume();
        float GetClipTime();
        void SetSpriteAnimation(ISpriteAnimationsConfig spriteAnimationConfig);
        string GetIdByType(string type);
        bool RenderByName(string animationName, float time, bool setOffsetDelta = true, bool hFlip = false);
        event Action AnimComplete;
        Texture2D[] GetTextures();
    }
}
