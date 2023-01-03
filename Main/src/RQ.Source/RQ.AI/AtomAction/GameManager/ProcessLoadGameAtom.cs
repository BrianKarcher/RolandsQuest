using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class ProcessLoadGameAtom : AtomActionBase
    {
        [SerializeField]
        private bool _fromMenu = false;
        [SerializeField]
        private string _fileName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            string fileName;

            if (GameController.Instance.UIManager.ClickedSaveSlotData != null)
            {
                var saveSlotData = GameController.Instance.UIManager.ClickedSaveSlotData;
                fileName = saveSlotData.FileName;
            }
            else
            {
                fileName = _fileName;
            }
            GameStateController.Instance.LoadGame(fileName);
        }

        public override void End()
        {
            GameController.Instance.UIManager.ClickedSaveSlotData = null;
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
