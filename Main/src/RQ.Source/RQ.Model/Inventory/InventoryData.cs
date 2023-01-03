using RQ.Model.Item;
using System;
using System.Collections.Generic;

namespace RQ.Model.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public Dictionary<string, ItemInInventoryData> Items { get; set; }
        public int Gold { get; set; }

        public InventoryData()
        {
            Items = new Dictionary<string, ItemInInventoryData>();
        }

        public ItemInInventoryData GetItem(string uniqueId)
        {
            if (Items.TryGetValue(uniqueId, out var item))
                return item;
            else
                return null;
        }

        public int GetItemQuantity(string uniqueId)
        {
            if (Items.TryGetValue(uniqueId, out var item))
                return item.Quantity;
            else
                return 0;
        }

        public void DeleteItem(ItemInInventoryData item)
        {
            Items.Remove(item.ItemUniqueId);
        }

        public InventoryData Clone()
        {
            var newInventoryData = new InventoryData();
            newInventoryData.Gold = Gold;
            newInventoryData.Items = new Dictionary<string, ItemInInventoryData>();
            foreach (var item in Items)
            {
                newInventoryData.Items[item.Key] = item.Value.Clone();
            }
            return newInventoryData;
        }
    }
}
