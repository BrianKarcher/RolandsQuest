using RQ.Model.Enums;
using UnityEngine;
//using Sprites = RQ.Entity.Sprites;


namespace RQ.Entity.StatesV2
{
    //public enum SaveLoad
    //{
    //    Load = 0,
    //    Save = 1
    //}

    [AddComponentMenu("RQ/States/State/UI/Setup Persistence Panel")]
    public class SetupPersistencePanelState : PanelState
    {

        [SerializeField]
        private bool _newGameSlot = false;
        [SerializeField]
        private string _mainLabelText = null;
        [SerializeField]
        private SaveOrLoad _savePanelState;

        public override void Enter()
        {
            base.Enter();
            _entity.SetupPersistenceGrid(_mainLabelText, _savePanelState, _newGameSlot);
            //GameController.Instance.UIManager.PersistenceMainLabelText = _mainLabelText;
            //_entity.PersistenceGrid.ClearGrid();
            //_entity.PersistenceGrid.AddNewSaveSlot = _newGameSlot;
            //_entity.PersistenceGrid.SavePanelState = _savePanelState;
            //_entity.PersistenceGrid.PopulateGrid();
        }

        public override void Exit()
        {
            base.Exit();
            _entity.ClearPersistenceGrid();
            //_entity.PersistenceGrid.ClearGrid();
        }
    }
}
