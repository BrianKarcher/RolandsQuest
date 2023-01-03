using RQ.AnimationV2;
using System.Collections.Generic;

namespace RQ.Animation.V2
{
    public interface ISpriteAnimationsConfig
    {
        string GetUniqueId();
        List<SpriteAnimationType> GetStoredSpriteAnimations();
    }
}
