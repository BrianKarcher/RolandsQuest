using RQ.Model.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.Data
{
    [Serializable]
    public class DamageData
    {
        public bool DamageTargetOnCollision = true;
        public bool DamageTargetOnTrigger = false;
        public float DamageOnTouch = 1f;
        public bool TakesDamage = true;
        public bool TakesSkillDamage = true;
        public bool Vulnerable = true;
        public CollisionActionType CollisionDamageType = CollisionActionType.Normal;
        [HideInInspector]
        public HashSet<string> EntityDeathNotification { get; set; }
        public string AttackedBySkill { get; set; }
        public int AttackCount { get; set; }
        //public List<string> DamageNotify { get; set; }
        //public DamageData()
        //{
        //    DamageNotify = new List<string>();
        //}
    }
}
