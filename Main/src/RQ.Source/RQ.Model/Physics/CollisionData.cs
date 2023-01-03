using RQ.Common;
using RQ.Physics.Collision;
using System;
using UnityEngine;

namespace RQ.Physics.Data
{
    [Serializable]
    public class CollisionData
    {
        [SerializeField]
        public CollisionLevel Level = CollisionLevel.Same;
        [SerializeField]
        [Tag]
        public string[] Tags;

        //[SerializeField]
        //public LevelLayer _levelLayer = LevelLayer.LevelOne;

        [SerializeField]
        public bool ReceivesDamage = true;
        [SerializeField]
        public bool DamageCollider = false;
        [SerializeField]
        public bool DeflectingCollider = false;
        [SerializeField]
        public bool CurrentlyDeflecting = false;
        [SerializeField]
        public bool IsEnabled = true;
        public Vector2D ColliderOffset { get; set; }
        public Vector2D ColliderSize { get; set; }
        public float ColliderRadius { get; set; }
        public bool IsColliderTrigger { get; set; }

        // Gets maintained by the Active state
        //[NonSerialized]
        //public HashSet<string> NearbyEntities { get; set; }    
    
        //public CollisionData()
        //{
        //    //if (NearbyEntities == null)
        //    //    NearbyEntities = new HashSet<string>();
        //}
    }
}
