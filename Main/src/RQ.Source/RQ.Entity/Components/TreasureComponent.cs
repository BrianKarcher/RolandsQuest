using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Treasure")]
    public class TreasureComponent : ItemComponent
    {
        //private bool _firstRun = true;

        //public override void Awake()
        //{
        //    base.Awake();
        //    if (!Application.isPlaying)
        //        return;
        //    //if (GameDataController.Instance.Data.TreasureChestsOpened.Contains(_treasureId))
        //}

        //public override void Update()
        //{
        //    base.Update();
        //    if (!Application.isPlaying)
        //        return;
        //    // TODO Place this into a coroutine so it only runs once
        //    if (!_firstRun)
        //        return;
        //    if (_treasureConfig == null)
        //        return;
        //    var treasureId = _treasureConfig.GetTreasureId();
        //    if (GameDataController.Instance.Data.TreasureChestsOpened[treasureId])
        //        MessageDispatcher2.Instance.DispatchMsg("QuickOpen", 0f, this.UniqueId, _componentRepository.UniqueId, null);
        //    _firstRun = false;
        //}

        //protected override void AcquireItem()
        //{
        //    base.AcquireItem(_item.GetItem(), _item.GetQuantity());
        //    //base.AcquireItem();
        //    //if (!GameDataController.Instance.Data.TreasureChestsOpened.Contains(_treasureId))
        //    GameDataController.Instance.Data.TreasureChestsOpened[_treasureConfig.GetTreasureId()] = true;
        //    //GameDataController.Instance.Data.TreasureChestsOpened.Add(_treasureId);
        //}

        //protected override void DisplayAcquireModal()
        //{
        //    base.DisplayAcquireModal(_treasureConfig.GetItem(), _treasureConfig.GetQuantity());
        //}

        //protected override ItemAndQuantityData SerializeData()
        //{
        //    var data = base.SerializeData();
        //    data.TreasureConfigUniqueId = _treasureConfig.UniqueId;
        //    return data;
        //}

        //protected override void DeserializeData(ItemAndQuantityData addItemData)
        //{
        //    base.DeserializeData(addItemData);
        //    _treasureConfig = GameDataController.Instance.GetGameConfig().GetAsset<TreasureConfig>(addItemData.TreasureConfigUniqueId);
        //    //_treasureConfig = ConfigsContainer.Instance.GetConfig<TreasureConfig>(addItemData.TreasureConfigUniqueId);
        //    //_treasureId = addItemData.TreasureId;
        //}
    }
}
