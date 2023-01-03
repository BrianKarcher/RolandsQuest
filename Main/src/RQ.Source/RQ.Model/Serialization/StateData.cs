using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class StateData
    {
        public string StateUniqueId;
        public Dictionary<string, object> DataObjects;
    }
}
