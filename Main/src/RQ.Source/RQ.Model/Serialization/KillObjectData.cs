using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class KillObjectData
    {
        public string KillObjectUniqueId { get; set; }
        public List<string> KillObjectUniqueIds { get; set; }
    }
}
