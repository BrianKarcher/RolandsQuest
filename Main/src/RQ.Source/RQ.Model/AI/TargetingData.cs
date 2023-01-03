using RQ.Common;
using RQ.Controller.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.AI
{
    [Serializable]
    public class TargetingData
    {
        public Transform Waypoints;
        public GoToType GoToType;
        public Transform Target;
        public List<Transform> Targets;
        [Tag]
        public string[] TargetTags;
    }
}
