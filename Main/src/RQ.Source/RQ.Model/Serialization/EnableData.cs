using System;
using UnityEngine;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class EnableData
    {
        [HideInInspector]
        public string TargetUniqueId = null;
    }
}
