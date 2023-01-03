using RQ.Controller.ManageScene;
using RQ.Entity.StatesV2.Conditions;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Item/Use Selected Item")]
    public class UseSelectedItem : ActionBase
    {
        [SerializeField]
        private ManualCondition _itemUsed = null;

        [SerializeField]
        private ManualCondition _itemNotUsed = null;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            var item = InventoryController.Instance.SelectedItem;
            var isSuccess = InventoryController.Instance.UseItem(item);
            if (isSuccess)
                _itemUsed.SetComplete(true);
            else
                _itemNotUsed.SetComplete(true);

            InventoryController.Instance.SelectedItem = null;
        }
    }
}
