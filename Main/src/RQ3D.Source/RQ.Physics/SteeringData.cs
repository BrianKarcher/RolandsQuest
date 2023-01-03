using System;
using UnityEngine;

namespace RQ.Physics
{
    [Serializable]
    public class SteeringData
    {
        [SerializeField]
        private float FeelerOffset;

        /// <summary>
        /// Used for obstacle avoidance
        /// </summary>
        [SerializeField]
        private float BoundingRadius;
    }
}
