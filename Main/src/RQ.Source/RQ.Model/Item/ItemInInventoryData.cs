using System;
using Newtonsoft.Json;

namespace RQ.Model.Item
{
    [Serializable]
    public class ItemInInventoryData
    {
        public string UniqueId { get; set; }
        public string ItemUniqueId { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public IItemConfig ItemConfig { get; set; }

        public ItemInInventoryData Clone()
        {
            return new ItemInInventoryData()
            {
                UniqueId = UniqueId,
                ItemUniqueId = ItemUniqueId,
                Quantity = Quantity
            };
        }
    }
}
