using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Entity.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Conversation")]
    public class ConversationState : StateBase
    {
        [SerializeField]
        private string _uiBeginMessage;
        [SerializeField]
        private string _uiEndMessage;
        [SerializeField]
        private string _broadcastEndMessage;

        public override void Enter()
        {
            //Debug.LogWarning("Entering GM Conversation state");
            MessageDispatcher2.Instance.DispatchMsg("Pause", 0f, string.Empty, "Game Controller", null);
            if (!string.IsNullOrEmpty(_uiBeginMessage))
            {
                //Debug.LogWarning("Sending message " + _uiBeginMessage);
                MessageDispatcher2.Instance.DispatchMsg(_uiBeginMessage, 0f, string.Empty, "UI Manager", null);
            }
            //MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, "Game Controller", Enums.Telegrams.Pause,
            //    null);
        }

        public override void Exit()
        {
            //Debug.LogWarning("Exiting GM Conversation state");
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, "Game Controller", null);
            if (!string.IsNullOrEmpty(_uiEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_uiEndMessage, 0f, string.Empty, "UI Manager", null);
            // this Broadcast can potentially cause a state change, so wait a frame until we are fully in the next
            // state before sending out the message
            if (!string.IsNullOrEmpty(_broadcastEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_broadcastEndMessage, 0.01f, string.Empty, null, null);
            //MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, "Game Controller", Enums.Telegrams.Unpause,
            //    null);
        }
    }
}
