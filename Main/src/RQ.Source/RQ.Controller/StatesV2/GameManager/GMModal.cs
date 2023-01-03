using RQ.Messaging;
using System;
using UnityEngine;
using RQ.FSM.V2;

namespace RQ.Controller.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Modal")]
    public class GMModal : StateBase
    {
        [SerializeField]
        private string _uiBeginMessage;
        [SerializeField]
        private string _uiEndMessage;
        [SerializeField]
        private string _broadcastEndMessage;
        [SerializeField]
        private string _text;

        public override void Enter()
        {
            base.Enter();
            if (!String.IsNullOrEmpty(_text))
                GameController.Instance.UIManager.SetModalText(_text);
            MessageDispatcher2.Instance.DispatchMsg("Pause", 0f, this.UniqueId, "Game Controller", null);
            if (!String.IsNullOrEmpty(_uiBeginMessage))
                MessageDispatcher2.Instance.DispatchMsg(_uiBeginMessage, 0f, this.UniqueId, 
                    "UI Manager", null);
        }

        public override void Exit()
        {
            base.Exit();
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, "Game Controller", null);
            if (!String.IsNullOrEmpty(_uiEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_uiEndMessage, 0f, this.UniqueId,
                    "UI Manager", null);
            if (!String.IsNullOrEmpty(_broadcastEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_broadcastEndMessage, 0.01f, this.UniqueId,
                    null, null);
        }
    }
}
