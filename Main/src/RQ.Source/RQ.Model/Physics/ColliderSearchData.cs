using RQ.Model.Interfaces;
using UnityEngine;

namespace RQ.Model.Physics
{
    public class ColliderSearchData
    {
        public string EntityUniqueId { get; set; }
        public Vector2 Point { get; set; }
        public ICollisionComponent CollisionComponent {get;set;}
    }
}
