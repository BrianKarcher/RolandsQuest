//using Sprites = RQ.Entity.Sprites;
using RQ.Controller.ManageScene;
using RQ.Model.Item;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Setup Inventory Panel")]
    public class SetupInventoryPanelState : PanelState
    {
        public ItemClass[] itemClasses;
        public override void Enter()
        {
            base.Enter();
            //_entity.InventoryGrid.ClearGrid();
            //var itemGridData = InventoryController.Instance.GetInventoryAsGrid(itemClasses);
            //_entity.InventoryGrid.PopulateGrid(itemGridData);
            //GetAllPanelButtons();
        }

        public override void Exit()
        {
            base.Exit();
            _entity.ClearPersistenceGrid();
            //_entity.PersistenceGrid.ClearGrid();
        }
    }
}
