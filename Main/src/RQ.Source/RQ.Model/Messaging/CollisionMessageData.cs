using RQ.Model.Interfaces;
using System;
using UnityEngine;

namespace RQ.Model.Messaging
{
    [Serializable]
    public class CollisionMessageData
    {
        //public Collider2D TriggerCollider { get; set; }
        public Collider OtherCollider { get; set; }
        //public Collision2D CollisionCollider { get; set; }
        public Collision CollisionCollider { get; set; }
        public string CollisionComponentUniqueId { get; set; }
        public string ThisTag { get; set; }
        //public Vector2D HitPosition { get; set; }
        public Vector3 HitPosition { get; set; }
        //public bool? AreWeADamageCollider { get; set; }
        public ICollisionComponent OurCollisionComponent { get; set; }
    }
}
