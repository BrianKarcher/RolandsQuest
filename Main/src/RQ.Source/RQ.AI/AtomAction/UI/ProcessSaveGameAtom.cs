using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Serialization;
using System;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    public class ProcessSaveGameAtom : AtomActionBase
    {
        //protected IUIManager _entity;

        public bool FromMenu = false;
        public string FileName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //_entity = entity.GetComponent<IUIManager>();
            if (_entity == null)
                throw new Exception("FSM - UIManager not found.");

            string fileName;

            if (FromMenu)
            {
                var saveSlotData = GameController.Instance.UIManager.ClickedSaveSlotData;
                if (saveSlotData.IsNewSlot)
                    fileName = Persistence.CalculateFileName();
                else
                    fileName = saveSlotData.FileName;
            }
            else
            {
                fileName = FileName;
            }
            GameStateController.Instance.SaveGame(fileName);
        }

        public override void End()
        {
            base.End();
            GameController.Instance.UIManager.ClickedSaveSlotData = null;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
