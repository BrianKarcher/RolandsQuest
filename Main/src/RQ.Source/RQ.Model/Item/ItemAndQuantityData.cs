using System;

namespace RQ.Model.Item
{
    [Serializable]
    public struct ItemAndQuantityData
    {
        public IItemConfig ItemConfig;

        public int Quantity;
        //public int TreasureId { get; set; }
        //public string TreasureConfigUniqueId;

        //public string SetStoryProgressUniqueId { get; set; }
    }
}
