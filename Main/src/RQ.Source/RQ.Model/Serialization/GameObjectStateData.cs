using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class GameObjectStateData
    {
        public TransformData TransformData { get; set; }
        public bool enabled { get; set; }
        public Dictionary<string, object> ObjectData { get; set; }
    }
}
