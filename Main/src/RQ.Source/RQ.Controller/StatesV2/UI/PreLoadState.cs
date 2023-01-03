//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Pre Load")]
    public class PreLoadState : StateBase
    {
        [SerializeField]
        private bool _fromMenu = false;
        [SerializeField]
        private string _fileName;

        public override void Enter()
        {
            base.Enter();
            //string fileName;
            //if (_fromMenu)
            //{
            //    var saveSlot = GameController.Instance.UIManager.ClickedSaveSlotData;
            //    fileName = saveSlot.FileName;
            //}
            //else
            //{
            //    fileName = _fileName;
            //}
            //GameStateController.Instance.LoadGameFileName = fileName;
            //base.Complete();
        }

        public override void Exit()
        {
            base.Exit();
            //GameController.Instance.UIManager.ClickedSaveSlotData = null;
        }
    }
}
