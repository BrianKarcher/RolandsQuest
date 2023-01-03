using RQ.Model.Item;
using System;

namespace RQ.Entity.Item
{
    [Serializable]
    public class ItemConfigAndQuantityData
    {
        public ItemConfig ItemConfig;
        public int Quantity;
    }
}
