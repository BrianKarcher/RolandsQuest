using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.Model.Item;
using RQ.Common.Controllers;
using RQ.Entity.Skill;

namespace RQ.AI.Action
{
    [Serializable]
    public class CanUseMoldAtom : AtomActionBase
    {
        public ItemConfig Mold;
        private bool _canUse;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _canUse = false;

            if (GameDataController.Instance.CurrentMold.GetUniqueId() != Mold.UniqueId)
                return;

            var moldConfig = GameDataController.Instance.CurrentMold as MoldConfig;
            // Check each shard
            for (int i = 0; i < moldConfig.ShardConfigs.Length; i++)
            {
                var shardConfigAndQuantityCost = moldConfig.ShardConfigs[i];
                var shardsInInventory = GameDataController.Instance.Data.Inventory.GetItem(shardConfigAndQuantityCost.ItemConfig.UniqueId);
                if (shardsInInventory == null)
                    continue;
                if (shardsInInventory.Quantity < shardConfigAndQuantityCost.Quantity)
                {
                    _canUse = false;
                    return;
                }
            }
            _canUse = true;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public bool GetCanUse()
        {
            return _canUse;
        }
    }
}
