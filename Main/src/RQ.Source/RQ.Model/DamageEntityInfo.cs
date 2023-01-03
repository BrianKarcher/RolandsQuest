using RQ.Entity.Components;
using RQ.Model.Enums;
using RQ.Model.Interfaces;
using System;
using UnityEngine;

namespace RQ.Entity.Common
{
    [Serializable]
    public class DamageEntityInfo
    {
        public float DamageAmount { get; set; }
        public string DamagedBy { get; set; }
        public CollisionActionType CollisionDamageType { get; set; }
        //[NonSerialized]
        public IComponentRepository DamagedByEntity { get; set; }
        public Vector2 DamageSourceLocation { get; set; }
        public Vector2 HitPosition { get; set; }
        public string Tag { get; set; }
        // TODO: Move into StateInfo
        public bool IsDamaged { get; set; }
        //[NonSerialized]
        public ICollisionComponent CollisionHit { get; set; }
        public string SkillUsed { get; set; }
    }
}
