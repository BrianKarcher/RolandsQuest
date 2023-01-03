using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class AdjustDepthByYData
    {
        public float OldAdjustedZ { get; set; }
        public float OriginalZ { get; set; }
        public float FootPositionY { get; set; }
    }
}
