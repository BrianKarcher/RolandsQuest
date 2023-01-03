using UnityEngine;

namespace RQ.Common.Components
{
    // Point used to position a sprite and set its facing direction
    [AddComponentMenu("RQ/Components/Location")]
    public class Location : BaseObject
    {
        /// <summary>
        /// Facing direction when location is reached
        /// </summary>
        [SerializeField]
        private Direction _direction;

        public Direction Direction { get { return _direction; } set { _direction = value; } }

        [SerializeField]
        private Transform _parent;        
    }
}
