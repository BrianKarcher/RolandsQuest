using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.Data
{
    [Serializable]
    public class SmartFollowData
    {
        public List<Vector3> Path { get; set; }
    }
}
