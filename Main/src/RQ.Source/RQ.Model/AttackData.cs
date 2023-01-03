using RQ.Common;
using RQ.Model.Physics;
using System;
using UnityEngine;

namespace RQ.Model
{
    [Serializable]
    public class AttackData
    {
        [SerializeField]
        public float StrikeDelay = 0f;
        [SerializeField]
        public bool StopMovingDuringAttack = true;
        [SerializeField]
        public Vector2 Offset;
        [SerializeField]
        public Vector2 Size;
        [SerializeField]
        public float Angle = 0;
        [SerializeField]
        public float Distance = 0;
        [SerializeField]
        public float Damage = 0;
        [SerializeField]
        [Tag]
        public string[] TargetTags = null;
        [SerializeField]
        public bool SameLayer = true;
        [SerializeField]
        public SingleUnityLayer[] Layers;
        //[SerializeField]
        //public ItemConfig _skill;
        public AttackDirection AttackDirection;
    }

    public enum AttackDirection
    {
        FourWay = 0,
        EightWay = 1,
        Circle = 2
    }
}
