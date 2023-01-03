using RQ.Common.Controllers;
using RQ.Model.Item;

namespace RQ.Entity.Common
{
    public static class Utility
    {
        public static bool IsSelectedSkillAffordable()
        {
            var selectedSkillUniqueId = GameDataController.Instance.Data.SelectedSkill;
            if (string.IsNullOrEmpty(selectedSkillUniqueId))
                return false;
            //var skillConfig = ConfigsContainer.Instance.GetConfig<SkillConfig>(selectedSkillUniqueId);
            var skillConfig = GameDataController.Instance.GetGameConfig().GetAsset<SkillConfig>(selectedSkillUniqueId);
            var sp = GameDataController.Instance.Data.CurrentEntityStats.CurrentSP;
            if (sp < skillConfig.Value)
                return false;
            // Check each orbs count to make sure player can afford it
            foreach (var orbCost in skillConfig.Orbs)
            {
                if (orbCost.Cost == 0)
                    continue;
                ItemInInventoryData playerOrb;
                if (!GameDataController.Instance.Data.Inventory.Items.TryGetValue(orbCost.Orb.UniqueId, out playerOrb))
                    return false;
                if (playerOrb.Quantity < orbCost.Cost)
                    return false;
                //var playerOrb = GameDataController.Instance.Data.Inventory.Items
                //orbCost.
            }
            return true;
        }
    }
}
