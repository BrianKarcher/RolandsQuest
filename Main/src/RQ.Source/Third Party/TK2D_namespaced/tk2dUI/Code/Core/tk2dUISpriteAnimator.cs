using UnityEngine;
using System.Collections;

namespace tk2d
{
    [AddComponentMenu("2D Toolkit/UI/Core/tk2dUISpriteAnimator")]
    public class tk2dUISpriteAnimator : tk2dSpriteAnimator
    {
        public override void LateUpdate()
        {
            UpdateAnimation(tk2dUITime.deltaTime);
        }
    }
}
