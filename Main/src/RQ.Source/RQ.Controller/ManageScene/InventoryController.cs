using RQ.Common.Controllers;
using RQ.Controller.Actions;
using RQ.Controller.UI.Grid.Data;
using RQ.Messaging;
using RQ.Model.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.ManageScene
{
    public class InventoryController : MessagingObject
    {
        public override string UniqueId { get { return "Inventory Controller"; } set { } }

        //private Dictionary<string, ItemConfig> _itemConfigs = new Dictionary<string,ItemConfig>();
        [SerializeField]
        private List<ItemConfigAction> _itemConfigActions = new List<ItemConfigAction>();

        //private IList<ItemConfig> _itemConfigs;

        //[SerializeField]
        //private ManualCondition _itemClickedCondition;

        //public ManualCondition ItemClickedCondition { get { return _itemClickedCondition; } }

        private static InventoryController _instance;
        [HideInInspector]
        public static InventoryController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<InventoryController>();
                    //_instance._itemConfigs
                }
                return _instance;
            }
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("AddItem", this.UniqueId, (data) =>
                {
                    var itemdata = (ItemAndQuantityData)data.ExtraInfo;
                    var item = itemdata.ItemConfig;
                    //var item = FindItem(itemdata.ItemConfig);
                    //if (item.ItemClass == ItemClass.CreationOrb)
                    //{
                    //    GameDataController.Instance.Data.EntityStats.CreationOrbCount += item.Value;
                    //}
                    //else if (item.ItemClass == ItemClass.JusticeOrb)
                    //{
                    //    GameDataController.Instance.Data.EntityStats.JusticeOrbCount += item.Value;
                    //}
                    //else if (item.ItemClass == ItemClass.SpiritOrb)
                    //{
                    //    GameDataController.Instance.Data.EntityStats.SpiritOrbCount += item.Value;
                    //}
                    //else if (item.ItemClass == ItemClass.JusticeOrb)
                    //{
                    //    GameDataController.Instance.Data.EntityStats.JusticeOrbCount += item.Value;
                    //}
                    switch (item.Type)
                    {
                        case ItemType.Gold:
                            //MessageDispatcher2.Instance.DispatchMsg("AddGold", 0f, this.UniqueId, "Inventory Controller",
                            //    quantity);
                            GameDataController.Instance.Data.Inventory.Gold += itemdata.Quantity;
                            MessageDispatcher2.Instance.DispatchMsg("SetGold", 0f, this.UniqueId, "UI Manager", GameDataController.Instance.Data.Inventory.Gold);
                            break;
                        default:
                            var itemAndQuantityData = new ItemAndQuantityData()
                            {
                                ItemConfig = item,
                                Quantity = item.Value
                            };
                            AddItemToInventory((ItemConfig)item, itemdata.Quantity);
                            break;
                    }
                    Debug.Log("(InventoryController) Updating the Stats in the HUD.");
                    MessageDispatcher2.Instance.DispatchMsg("UpdateStatsInHud", 0f, this.UniqueId, "UI Manager", null);
                });
            MessageDispatcher2.Instance.StartListening("AddGold", this.UniqueId, (data) =>
            {
                var gold = (int)data.ExtraInfo;
                GameDataController.Instance.Data.Inventory.Gold += gold;
                MessageDispatcher2.Instance.DispatchMsg("SetGold", 0f, this.UniqueId, "UI Manager", GameDataController.Instance.Data.Inventory.Gold);
            });
            MessageDispatcher2.Instance.StartListening("Deserialize", this.UniqueId, (data) =>
                {
                    //GameDataController.Instance.Data.Inventory.Items
                    MessageDispatcher2.Instance.DispatchMsg("SetGold", 0f, this.UniqueId, "UI Manager", GameDataController.Instance.Data.Inventory.Gold);
                });
            MessageDispatcher2.Instance.StartListening("ListenToItemUse", this.UniqueId, (data) =>
                {
                    var itemUniqueId = (string)data.ExtraInfo;
                    List<string> listeners;
                    if (!GameDataController.Instance.Data.ItemUseEvents.TryGetValue(itemUniqueId, out listeners))
                    {
                        listeners = new List<string>();
                        GameDataController.Instance.Data.ItemUseEvents.Add(itemUniqueId, listeners);
                    }
                    listeners.Add(data.SenderId);
                    //GameDataController.Instance.Data.ItemUseEvents.Add()
                });
            MessageDispatcher2.Instance.StartListening("RemoveItemUseListener", this.UniqueId, (data) =>
                {
                    var itemUniqueId = (string)data.ExtraInfo;
                    List<string> listeners;
                    if (GameDataController.Instance.Data == null)
                        return;
                    if (!GameDataController.Instance.Data.ItemUseEvents.TryGetValue(itemUniqueId, out listeners))
                        return;
                    listeners.Remove(data.SenderId);
                });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("AddItem", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("AddGold", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("Deserialize", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("ListenToItemUse", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("RemoveItemUseListener", this.UniqueId, -1);
        }

        // TODO Find a better way to do this
        //private ItemConfig FindItem(string uniqueId)
        //{
        //    //var itemConfigAction = _itemConfigActions.FirstOrDefault(i => i.Config.UniqueId == uniqueId);
        //    //if (itemConfigAction != null)
        //    //    return itemConfigAction.Config;
        //    //return null;
        //    //return ConfigsContainer.Instance.GetConfig<ItemConfig>(uniqueId);
        //    return GameDataController.Instance.GetGameConfig().GetAsset<ItemConfig>(uniqueId);
        //}       

        public ItemInInventoryData SelectedItem { get; set; }

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;
        }

        public void AddItemToInventory(ItemConfig itemConfig, int quantity)
        {
            var itemsList = GameDataController.Instance.Data.Inventory.Items;
            ItemInInventoryData item;
            if (!itemsList.TryGetValue(itemConfig.UniqueId, out item))
            {
                // Item does not exist in inventory
                item = new ItemInInventoryData();
                //var item = new ItemInInventoryData();
                item.UniqueId = Guid.NewGuid().ToString();
                item.ItemUniqueId = itemConfig.UniqueId;
                item.Quantity = quantity;
                itemsList.Add(item.ItemUniqueId, item);
                item.ItemConfig = itemConfig;
            }
            else
            {
                // Item already exists in inventory, just increase quantity
                item.Quantity += quantity;
            }
        }

        public ItemInInventoryData GetItem(string uniqueId)
        {
            return GameDataController.Instance.Data.Inventory.Items[uniqueId];
        }

        //private void AddItemConfig(ItemConfig itemConfig)
        //{
        //    //_itemConfigs.Add(itemConfig.UniqueId, itemConfig);
        //    _itemConfigs.Add(itemConfig);
        //}

        public List<ItemGridData> GetInventoryAsGrid(params ItemClass[] itemClasses)
        {
            var itemGridDatas = new List<ItemGridData>();
            var data = GameDataController.Instance.Data;
            if (data == null)
            {
                Debug.LogError("No Game Data");
                return null;
            }
            var inventory = data.Inventory;
            if (inventory == null)
            {
                Debug.LogError("No Game Data Inventory");
                return null;
            }
            var items = GameDataController.Instance.Data.Inventory.Items;
            foreach (var item in items)
            {
                var itemConfig = item.Value.ItemConfig;
                var hasItemClass = Array.IndexOf(itemClasses, itemConfig.ItemClass) > -1;
                //if (!itemClasses.Contains(itemConfig.ItemClass))
                if (!hasItemClass)
                    continue;
                var itemGridData = new ItemGridData();
                itemGridData.Data = item.Value;
                //itemGridData.Config = _itemConfigs[item.ItemUniqueId];
                itemGridData.Config = itemConfig;//_itemConfigActions.FirstOrDefault(i => i.Config.UniqueId == item.ItemUniqueId).Config;
                itemGridDatas.Add(itemGridData);
            }

            return itemGridDatas;
        }

        public bool UseItem(ItemInInventoryData item)
        {
            if (item.Quantity < 1)
                return false;

            if (RunListeners(item))
                return false;

            if (!ProcessItem(item))
                return false;

            item.Quantity--;

            if (item.Quantity == 0)
                DeleteItemFromInventory(item);
            return true;
        }

        public bool RunListeners(ItemInInventoryData item)
        {
            List<string> listeners;
            if (!GameDataController.Instance.Data.ItemUseEvents.TryGetValue(item.ItemUniqueId, out listeners))
                return false;
            if (listeners.Count == 0)
                return false;
            foreach (var listener in listeners)
            {
                MessageDispatcher2.Instance.DispatchMsg("ItemUsed", 0f, this.UniqueId, listener, item.ItemUniqueId);
            }
            return true;
        }

        public bool ProcessItem(ItemInInventoryData item)
        {
            ItemConfigAction itemConfig = null;
            foreach (var tempItemConfig in _itemConfigActions)
            {
                if (tempItemConfig.Config.UniqueId == item.ItemUniqueId)
                {
                    itemConfig = tempItemConfig;
                    break;
                }
            }
            //var itemConfig = _itemConfigActions.FirstOrDefault(i => i.Config.UniqueId == item.ItemUniqueId);
            if (itemConfig == null || itemConfig.ActionSequence == null)
                return false;
            var actionSequence = itemConfig.ActionSequence.GetComponent<ActionSequence>();
            return actionSequence.CheckAndRun();
        }

        public void DeleteItemFromInventory(ItemInInventoryData item)
        {
            GameDataController.Instance.Data.Inventory.DeleteItem(item);
        }

        //public override bool HandleMessage(Telegram msg)
        //{
        //    if (base.HandleMessage(msg))
        //        return true;

        //    switch (msg.Msg)
        //    {
        //        //case Enums.Telegrams.AddContainerItem:
        //        //    AddItemConfig(msg.ExtraInfo as ItemConfig);
        //        //    break;
        //        //case Enums.Telegrams.GetContainerItem:
        //        //    var uniqueId = (string)msg.ExtraInfo;
        //        //    //msg.Act(_itemConfigs[uniqueId]);
        //        //    msg.Act(_itemConfigs.FirstOrDefault(i => i.UniqueId == uniqueId));
        //        //    break;
        //    }

        //    return false;
        //}
    }
}
