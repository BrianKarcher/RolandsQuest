using System;
using RQ.Common.Controllers;
using RQ.Controller.Sequencer;
using RQ.FSM.V2;
using RQ.Messaging;
using UnityEngine;
using WellFired;

namespace RQ.Entity.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Sequencer")]
    public class SequencerState : StateBase
    {
        [SerializeField]
        private string _uiBeginMessage;
        [SerializeField]
        private string _uiEndMessage;
        [SerializeField]
        private string _broadcastEndMessage;
        private int _sequencerCount;

        private long _startCutsceneId, _startConversationId, _endConversationId;
        private Action<Telegram2> _startCutsceneDelegate, _startConversationDelegate, _endConversationDelegate;

        public override void Awake()
        {
            base.Awake();
            _startCutsceneDelegate = (data) =>
            {
                SetupSequence();
            };
            _startConversationDelegate = (data) =>
            {
                var currentSequence = GameController.Instance.UIManager.CurrentSequence;
                currentSequence.Pause();
            };
            _endConversationDelegate = (data) =>
            {
                var currentSequence = GameController.Instance.UIManager.CurrentSequence;
                currentSequence.Play();
            };
        }

        public override void Enter()
        {
            //Debug.LogWarning("Entering SequenceState");
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, _spriteBase.UniqueId, null);
            if (!string.IsNullOrEmpty(_uiBeginMessage))
                MessageDispatcher2.Instance.DispatchMsg(_uiBeginMessage, 0f, string.Empty, "UI Manager", null);
            _sequencerCount = 0;
            SetupSequence();
        }

        private void SetupSequence()
        {
            var currentSequence = GameController.Instance.UIManager.CurrentSequence;
            if (currentSequence == null)
                return;
            //currentSequence.PlaybackFinished = PlaybackFinished;
            Debug.Log("SequencerState: Playing sequence " + currentSequence.name);
            currentSequence.Play();
            _sequencerCount++;
            GameStateController.Instance.PlayCutscene = false;
        }

        private void PlaybackFinished(USSequencer sequencer)
        {
            //Debug.LogWarning("Sequence " + sequencer.name + " Finished");
            _sequencerCount--;
            //Debug.LogWarning("Sequence Count = " + _sequencerCount);
            if (_sequencerCount > 0)
                return;
            GameDataController.Instance.Data.RunningSequenceUniqueId = null;
            GameController.Instance.UIManager.CurrentSequence = null;
            //Debug.LogWarning("SequenceState Calling StopCutscene");
            MessageDispatcher2.Instance.DispatchMsg("StopCutscene", 0f, this.UniqueId, "Game Controller", null);
            Complete();
            var sequencerLink = sequencer.GetComponent<SequencerLink>();
            // SequenceComplete only gets called on the last sequencer to stop playing.
            if (sequencerLink != null)
                sequencerLink.SequenceComplete();
        }

        public override void StartListening()
        {
            base.StartListening();
            _startCutsceneId =
                MessageDispatcher2.Instance.StartListening("StartCutscene", _componentRepository.UniqueId, _startCutsceneDelegate);
            //_componentRepository.StartListening("StartCutscene", this.UniqueId, _startCutsceneDelegate);
            _startConversationId =
                MessageDispatcher2.Instance.StartListening("StartConversation", _componentRepository.UniqueId,
                    _startConversationDelegate);
            //_componentRepository.StartListening("StartConversation", this.UniqueId, _startConversationDelegate);
            _endConversationId =
                MessageDispatcher2.Instance.StartListening("EndConversation", _componentRepository.UniqueId, _endConversationDelegate);
            //_componentRepository.StartListening("EndConversation", this.UniqueId, _endConversationDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("StartCutscene", _componentRepository.UniqueId, _startCutsceneId);
            //_componentRepository.StopListening("StartCutscene", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("StartConversation", _componentRepository.UniqueId, _startConversationId);
            //_componentRepository.StopListening("StartConversation", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("EndConversation", _componentRepository.UniqueId, _endConversationId);
            //_componentRepository.StopListening("EndConversation", this.UniqueId);
        }

        public override void Exit()
        {
            //Debug.LogWarning("Exiting SequenceState");
            //MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, "Game Controller", Enums.Telegrams.Unpause,
            //    null);
            if (!string.IsNullOrEmpty(_uiEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_uiEndMessage, 0f, string.Empty, "UI Manager", null);
            var currentSequence = GameController.Instance.UIManager.CurrentSequence;
            if (currentSequence == null)
                return;
            //if (currentSequence.IsPlaying)
            //{
            //    currentSequence.Pause();
            //}
            //else
            //{
            //    GameController.Instance.UIManager.CurrentSequence = null;
            //}
            //currentSequence.PlaybackFinished = null;
            //MessageDispatcher2.Instance.RemoveMessages("StartCutscene", this.UniqueId);
            if (!string.IsNullOrEmpty(_broadcastEndMessage))
                MessageDispatcher2.Instance.DispatchMsg(_broadcastEndMessage, 0f, string.Empty, null, null);
        }
    }
}
