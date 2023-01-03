using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class EntitiesDiedData
    {
        public List<string> EntitiesUniqueIds { get; set; }
    }
}
