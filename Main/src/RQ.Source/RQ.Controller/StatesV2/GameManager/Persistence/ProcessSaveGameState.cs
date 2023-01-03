using UnityEngine;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Serialization;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Process Save Game")]
    public class ProcessSaveGameState : StateBase
    {
        [SerializeField]
        private bool _fromMenu = false;
        [SerializeField]
        private string _fileName;

        public override void Enter()
        {
            base.Enter();
            string fileName;

            if (_fromMenu)
            {
                var saveSlotData = GameController.Instance.UIManager.ClickedSaveSlotData;
                if (saveSlotData.IsNewSlot)
                    fileName = Persistence.CalculateFileName();
                else
                    fileName = saveSlotData.FileName;
            }
            else
            {
                fileName = _fileName;
            }
            GameStateController.Instance.SaveGame(fileName);
            Complete();
        }

        public override void Exit()
        {
            base.Exit();
            GameController.Instance.UIManager.ClickedSaveSlotData = null;
        }
    }
}
