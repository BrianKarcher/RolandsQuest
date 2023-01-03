using System;
using UnityEngine;

namespace RQ.Model.Item
{
    [Serializable]
    public class DropItem
    {
        [SerializeField]
        public GameObject Item;
        //public RQBaseConfig Item;
        [SerializeField]
        public int Quantity;
        [SerializeField]
        public float Chance;
    }
}
