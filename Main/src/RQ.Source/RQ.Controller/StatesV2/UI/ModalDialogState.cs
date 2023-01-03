using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Modal Dialog")]
    public class ModalDialogState : PanelState
    {
        [SerializeField]
        private string _modalText = string.Empty;

        [SerializeField]
        private bool _hasCancelButton = false;

        public override void Enter()
        {
            base.Enter();
            if (!String.IsNullOrEmpty(_modalText))
                GameController.Instance.UIManager.SetModalText(_modalText);
            _entity.SetupModal(_hasCancelButton);
            // Disable cancel button?
            //NGUITools.SetActive(GameController.Instance.UIManager.ModalCancelButton.gameObject,
            //    _hasCancelButton);
            ////UICamera.currentScheme = UICamera.ControlScheme.Controller;
            //UICamera.hoveredObject = GameController.Instance.UIManager.ModalOkButton.gameObject;
            //var saveLoadPanel = GameController._instance.UIManager.GetPanel(Panels.SaveLoad);
            //var buttons = GetPanelButtons(Panels.SaveLoad);
            //EnableButtons(buttons, false);
        }

        public override void Exit()
        {
            base.Exit();
            MessageDispatcher2.Instance.DispatchMsg("EndModal", 0f, this.UniqueId, "Game Controller", null);
        }
    }
}
