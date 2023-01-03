using RQ.Common.Config;
using System;
using UnityEngine;

namespace RQ.Model.Item
{
    [Serializable]
    public class StartingItem
    {
        [SerializeField]
        public RQBaseConfig Item;
        [SerializeField]
        public int Quantity;
    }
}
