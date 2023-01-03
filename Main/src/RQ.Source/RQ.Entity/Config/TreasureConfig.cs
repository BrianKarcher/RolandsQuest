using RQ.Common.Config;
using RQ.Model.Item;
using UnityEngine;

namespace RQ.Entity.Assets
{
    public class TreasureConfig : RQBaseConfig
    {
        [SerializeField]
        private int _treasureId = -1;
        [SerializeField]
        private ItemConfig _item = null;
        [SerializeField]
        private int _quantity = 1;

        public int GetTreasureId()
        {
            return _treasureId;
        }

        public void SetTreasureId(int treasureId)
        {
            _treasureId = treasureId;
        }

        public ItemConfig GetItem()
        {
            return _item;
        }

        public int GetQuantity()
        {
            return _quantity;
        }
    }
}
