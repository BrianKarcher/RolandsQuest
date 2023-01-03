using RQ.Audio;
using RQ.Common.Components;
using RQ.Entity.Item;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Item;
using RQ.Model.Messaging;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using RQ.Model.Audio;
using UnityEngine;
using RQ.Common;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Item 2")]
    public class ItemComponent2 : ComponentPersistence<ItemComponent2>, IEventTrigger
    {
        [SerializeField]
        private List<ItemConfigAndQuantityData> _items = null;
        //[SerializeField]
        //private int _quantity = 1;
        [SerializeField]
        private bool _acquireOnTrigger = false;
        [SerializeField]
        [Tag]
        private string[] _tagsThatAcquire;
        private bool _triggered = false;

        private long _acquireItemId, _displayAcquireModalId, _triggerEnterId, _endModalId;

        private Action<Telegram2> _acquireItemDelegate, _displayAcquireModalDelegate, _triggerEnterDelegate, _endModalDelegate;
        //[SerializeField]
        //private StorySceneConfig _setStoryProgress;

        public event EventHandler EventEnded;

        public override void Awake()
        {
            base.Awake();
            _acquireItemDelegate = (data) =>
            {
                AcquireFirstItem();
            };
            _displayAcquireModalDelegate = (data) =>
            {
                DisplayAcquireModal();
            };
            _triggerEnterDelegate = (data) =>
            {
                if (!_acquireOnTrigger)
                    return;
                if (data.ExtraInfo != null)
                {
                    var collider2D = data.ExtraInfo as CollisionMessageData;
                    if (_tagsThatAcquire == null)
                        return;
                    var found = Array.IndexOf(_tagsThatAcquire, collider2D.OtherCollider.tag) > -1;
                    if (found)
                    {
                        //AcquireItem();
                        MessageDispatcher2.Instance.DispatchMsg("UseUsable", 0f, this.UniqueId, _componentRepository.UniqueId, _componentRepository.UniqueId);
                        //
                        return;
                    }
                    else
                        return;
                }
                //AcquireItem();
            };
            _endModalDelegate = (data) =>
            {
                Debug.Log("ItemComponent - EndModal called");
                if (!_triggered)
                    return;
                Debug.Log("ItemComponent - EventEnded");
                EventEnded(this, EventArgs.Empty);
            };
        }

        public override void StartListening()
        {
            base.StartListening();
            if (!Application.isPlaying)
                return;
            _acquireItemId = MessageDispatcher2.Instance.StartListening("AcquireItem", _componentRepository.UniqueId, 
                _acquireItemDelegate);
            //_componentRepository.StartListening("AcquireItem", this.UniqueId, );

            _displayAcquireModalId = MessageDispatcher2.Instance.StartListening("DisplayAcquireModal", _componentRepository.UniqueId,
                _displayAcquireModalDelegate);
            //_componentRepository.StartListening("DisplayAcquireModal", this.UniqueId, );
            _triggerEnterId = MessageDispatcher2.Instance.StartListening("TriggerEnter", _componentRepository.UniqueId,
                _triggerEnterDelegate);
            //_componentRepository.StartListening("TriggerEnter", this.UniqueId, );
            _endModalId = MessageDispatcher2.Instance.StartListening("EndModal", _componentRepository.UniqueId,
                _endModalDelegate);
            //_componentRepository.StartListening("EndModal", this.UniqueId, );
        }

        public void Trigger(Transform requester)
        {
            Debug.Log("Item event triggered");
            Acquire();
            //AcquireFirstItem();
            // Triggered items notify the receiver when the user exits the modal
            _triggered = true;
        }

        public override void StopListening()
        {
            base.StopListening();
            if (!Application.isPlaying)
                return;
            MessageDispatcher2.Instance.StopListening("AcquireItem", _componentRepository.UniqueId, _acquireItemId);
            //_componentRepository.StopListening("AcquireItem", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("DisplayAcquireModal", _componentRepository.UniqueId, _displayAcquireModalId);
            //_componentRepository.StopListening("DisplayAcquireModal", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TriggerEnter", _componentRepository.UniqueId, _triggerEnterId);
            //_componentRepository.StopListening("TriggerEnter", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("EndModal", _componentRepository.UniqueId, _endModalId);
            //_componentRepository.StopListening("EndModal", this.UniqueId);
        }

        protected virtual void AcquireFirstItem()
        {
            var firstItem = _items[0];
            AcquireItem(firstItem);
        }

        protected virtual void AcquireItem(ItemConfigAndQuantityData itemData)
        {
            if (itemData == null)
            {
                Debug.LogError("Item not set in " + _componentRepository.name);
                return;
            }
            //switch (item.Type)
            //{
            //    case ItemType.Gold:
            //        MessageDispatcher2.Instance.DispatchMsg("AddGold", 0f, this.UniqueId, "Inventory Controller",
            //            quantity);
            //        break;
            //    default:
            var itemAndQuantityData = new ItemAndQuantityData()
            {
                ItemConfig = itemData.ItemConfig,
                Quantity = itemData.Quantity
            };
            MessageDispatcher2.Instance.DispatchMsg("AddItem", 0f, this.UniqueId, "Inventory Controller",
                itemAndQuantityData);

            if (itemData.ItemConfig.AcquireSound.AudioClips.Count != 0)
            {
                var currentClipIndex = UnityEngine.Random.Range(0, itemData.ItemConfig.AcquireSound.AudioClips.Count);
                var playSoundData = new PlaySoundData()
                {
                    AudioClip = itemData.ItemConfig.AcquireSound.AudioClips[currentClipIndex],
                    PlaySound = itemData.ItemConfig.AcquireSound.PlaySound,
                    //PlayOnMusicTrack = itemData.ItemConfig.AcquireSound.PlayOnMusicTrack,
                    //PlayAsOneShot = itemData.ItemConfig.AcquireSound.PlayAsOneShot,
                    Volume = itemData.ItemConfig.AcquireSound.Volume
                };
                MessageDispatcher2.Instance.DispatchMsg("PlaySoundOnSoundEffectTrack", 0f, this.UniqueId, "Game Controller",
                    playSoundData);
                //AudioComponent.PlaySoundInfo(playSoundData, this._componentRepository);
            }
        }

        protected virtual void DisplayAcquireModal()
        {
            var firstItem = _items[0];
            DisplayAcquireModal(firstItem.ItemConfig, firstItem.Quantity);
        }

        protected void DisplayAcquireModal(ItemConfig item, int quantity)
        {
            if (item == null || string.IsNullOrEmpty(item.AcquireText))
                return;
            string itemAcquiredText = item == null ? "(null)" : string.Format(item.AcquireText, quantity);
            MessageDispatcher2.Instance.DispatchMsg("DisplayModal", 0f, this.UniqueId, "UI Manager", itemAcquiredText);
        }

        public void Acquire()
        {
            AcquireFirstItem();
            DisplayAcquireModal();
            _items.RemoveAt(0);
        }

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            //if (_item == null)
            //    return;
            var addItemData = SerializeData();
            base.SerializeComponent(entitySerializedData, addItemData);
        }

        protected virtual IEnumerable<ItemAndQuantityData> SerializeData()
        {
            List<ItemAndQuantityData> itemList = new List<ItemAndQuantityData>();
            foreach (var item in _items)
            {
                itemList.Add(new ItemAndQuantityData
                {
                    ItemConfig = item.ItemConfig,
                    Quantity = item.Quantity
                });
            }
            //var itemList = _items.Select(i => new ItemAndQuantityData
            //{
            //    ItemConfigUniqueId = i.ItemConfig.UniqueId,
            //    Quantity = i.Quantity
            //});
            //var addItemData = new ItemAndQuantityData()
            //{
            //    ItemConfigUniqueId = _item == null ? null : _item.UniqueId,
            //    Quantity = _quantity,
            //    //SetStoryProgressUniqueId = _setStoryProgress == null ? null : _setStoryProgress.UniqueId
            //};
            //return addItemData;
            return itemList;
        }

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var addItemData = base.DeserializeComponent<IEnumerable<ItemAndQuantityData>>(entitySerializedData);
        //    if (addItemData == null)
        //        return;
        //    DeserializeData(addItemData);
        //}

        //protected virtual void DeserializeData(IEnumerable<ItemAndQuantityData> addItemDatas)
        //{
        //    _items.Clear();
        //    foreach (var item in addItemDatas)
        //    {
        //        var itemData = new ItemConfigAndQuantityData();
        //        if (!string.IsNullOrEmpty(item.ItemConfigUniqueId))
        //            itemData.ItemConfig = ConfigsContainer.Instance.GetConfig<ItemConfig>(item.ItemConfigUniqueId);
        //        itemData.Quantity = item.Quantity;
        //        _items.Add(itemData);
        //    }
        //}

        //public void SetStoryProgress()
        //{
        //    if (_setStoryProgress == null)
        //        return;
        //    GameDataController.Instance.Data.SetStoryProgress(_setStoryProgress);
        //}
    }
}
