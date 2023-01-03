using RQ.Controller.ManageScene;
using RQ.Model.Item;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Item/Add Item to Inventory")]
    public class AddItemToInventory : ActionBase
    {
        [SerializeField]
        private ItemConfig _item = null;
        [SerializeField]
        private int _quantity = 1;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            InventoryController.Instance.AddItemToInventory(_item, _quantity);
            //GameController._instance.AudioSource.PlayOneShot(AudioClip);
        }
    }
}
