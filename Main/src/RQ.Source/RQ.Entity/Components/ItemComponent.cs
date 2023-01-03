using RQ.Common.Components;
using RQ.Messaging;
using RQ.Model.Item;
using RQ.Model.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using RQ.Model.Messaging;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Item")]
    public class ItemComponent : ComponentPersistence<ItemComponent>
    {
        [SerializeField]
        private ItemConfig _item = null;
        [SerializeField]
        private int _quantity = 1;
        [SerializeField]
        private bool _acquireOnTrigger = false;
        [SerializeField]
        private List<string> _tagsThatAcquire;

        private long _acquireItemId, _displayAcquireModalId, _triggerEnterId;

        private Action<Telegram2> _acquireItemDelegate, _displayAcquireModalDelegate, _triggerEnterDelegate;
        //[SerializeField]
        //private StorySceneConfig _setStoryProgress;

        public override void Awake()
        {
            base.Awake();
            _acquireItemDelegate = (data) =>
            {
                AcquireItem();
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
                    if (_tagsThatAcquire.Contains(collider2D.OtherCollider.tag))
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
        }

        protected virtual void AcquireItem()
        {
            AcquireItem(_item, _quantity);
        }

        protected virtual void AcquireItem(ItemConfig item, int quantity)
        {
            if (item == null)
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

            ItemAndQuantityData itemAndQuantityData = new ItemAndQuantityData();
            itemAndQuantityData.ItemConfig = item;
            itemAndQuantityData.Quantity = quantity;

            MessageDispatcher2.Instance.DispatchMsg("AddItem", 0f, this.UniqueId, "Inventory Controller",
                itemAndQuantityData);

            PlayItemAcquireSound(item);
        }

        private void PlayItemAcquireSound(ItemConfig item)
        {
            if (item.AcquireSound.AudioClips.Count == 0)
                return;
            var currentClipIndex = UnityEngine.Random.Range(0, item.AcquireSound.AudioClips.Count);
            var soundInfo = new PlaySoundData()
            {
                AudioClip = item.AcquireSound.AudioClips[currentClipIndex],
                PlaySound = item.AcquireSound.PlaySound,
                //PlayOnMusicTrack = item.AcquireSound.PlayOnMusicTrack,
                //PlayAsOneShot = item.AcquireSound.PlayAsOneShot,
                Volume = item.AcquireSound.Volume
            };
            MessageDispatcher2.Instance.DispatchMsg("PlaySoundOnSoundEffectTrack", 0f, this.UniqueId, "Game Controller",
                soundInfo);
        }

        protected virtual void DisplayAcquireModal()
        {
            DisplayAcquireModal(_item, _quantity);
        }

        protected void DisplayAcquireModal(ItemConfig item, int quantity)
        {
            if (item == null || string.IsNullOrEmpty(item.AcquireText))
                return;
            string itemAcquiredText = item == null ? "(null)" : string.Format(item.AcquireText, quantity);
            MessageDispatcher2.Instance.DispatchMsg("DisplayModal", 0f, this.UniqueId, "UI Manager", itemAcquiredText);
        }

        //public void Acquire()
        //{
        //    AcquireItem();
        //    DisplayAcquireModal();
        //}

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    //if (_item == null)
        //    //    return;
        //    var addItemData = SerializeData();
        //    base.SerializeComponent(entitySerializedData, addItemData);
        //}

        //protected virtual ItemAndQuantityData SerializeData()
        //{
        //    var addItemData = new ItemAndQuantityData()
        //    {
        //        ItemConfigUniqueId = _item == null ? null : _item.UniqueId,
        //        Quantity = _quantity,
        //        //SetStoryProgressUniqueId = _setStoryProgress == null ? null : _setStoryProgress.UniqueId
        //    };
        //    return addItemData;
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var addItemData = base.DeserializeComponent<ItemAndQuantityData>(entitySerializedData);
        //    //if (addItemData == null)
        //    //    return;
        //    DeserializeData(addItemData);
        //}

        //protected virtual void DeserializeData(ItemAndQuantityData addItemData)
        //{
        //    if (!string.IsNullOrEmpty(addItemData.ItemConfigUniqueId))
        //        _item = GameDataController.Instance.GetGameConfig().GetAsset<ItemConfig>(addItemData.ItemConfigUniqueId);
        //    //_item = ConfigsContainer.Instance.GetConfig<ItemConfig>(addItemData.ItemConfigUniqueId);
        //    //if (!string.IsNullOrEmpty(addItemData.SetStoryProgressUniqueId))
        //    //    _setStoryProgress = ConfigsContainer.Instance.GetConfig<StorySceneConfig>(addItemData.SetStoryProgressUniqueId);
        //    _quantity = addItemData.Quantity;
        //}

        //public void SetStoryProgress()
        //{
        //    if (_setStoryProgress == null)
        //        return;
        //    GameDataController.Instance.Data.SetStoryProgress(_setStoryProgress);
        //}
    }
}
