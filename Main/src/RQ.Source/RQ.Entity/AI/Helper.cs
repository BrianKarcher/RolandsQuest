using HutongGames.PlayMaker;

namespace RQ.AI.AtomAction
{
    public static class Helper
    {
        public static int LayerArrayToLayerMask(FsmInt[] layers, bool invert)
        {
            var layermask = 0;

            foreach (var layer in layers)
            {
                layermask |= 1 << layer.Value;
            }

            if (invert)
            {
                layermask = ~layermask;
            }

            // Unity 5.3 changed this Physics property name
            //public const int kDefaultRaycastLayers = -5;
            /*
            #if UNITY_PRE_5_3
                        return layermask == 0 ? Physics.kDefaultRaycastLayers : layermask;
            #else
                        return layermask == 0 ? Physics.DefaultRaycastLayers : layermask;
            #endif
            */
            // HACK just return the hardcoded value to avoid separate Unity 5.3 dll
            // TODO Revisit in future version
            //return layermask == 0 ? -5 : layermask;
            return layermask;
        }
    }
}
