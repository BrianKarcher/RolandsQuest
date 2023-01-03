using RQ.AI;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.Sequencer;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;
using UnityEngine;
using WellFired;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class uSequencerAtom : AtomActionBase
    {
        private int _sequencerCount;
        //private long _startCutsceneId;
        private long _startConversationId;
        private long _endConversationId;
        private long _sequenceCompleteId;

        public override void Start(IComponentRepository entity)
        {
            Debug.Log("(uSequencerAtom) Start called");
            base.Start(entity);
            _sequencerCount = 0;
            SetupSequence();
        }

        private void SetupSequence()
        {
            var currentSequence = GameController.Instance.UIManager.CurrentSequence;
            if (currentSequence == null)
                return;
            //currentSequence.PlaybackFinished = PlaybackFinished;
            currentSequence.OnSequenceComplete += CurrentSequence_OnSequenceComplete;
            Debug.LogError("(uSequencerAtom) Playing sequence " + currentSequence.name);
            //currentSequence.Play();
            _sequencerCount++;
            GameStateController.Instance.PlayCutscene = false;
        }

        private void CurrentSequence_OnSequenceComplete()
        {
            //_sequencerCount--;
            //Debug.LogWarning("Sequence Count = " + _sequencerCount);
            //if (_sequencerCount > 0)
            //    return;
            AllSequencesComplete();
        }

        //private void PlaybackFinished(USSequencer sequencer)
        //{

        //}

        private void AllSequencesComplete()
        {
            Debug.LogWarning("(uSequencerAtom) Sequence " + GameController.Instance.UIManager.CurrentSequence?.name + " Finished");

            GameDataController.Instance.Data.RunningSequenceUniqueId = null;
            GameController.Instance.UIManager.CurrentSequence = null;
            //Debug.LogWarning("SequenceState Calling StopCutscene");
            //MessageDispatcher2.Instance.DispatchMsg("StopCutscene", 0f, string.Empty, "Game Controller", null);
            // Broadcast to all
            MessageDispatcher2.Instance.DispatchMsg("StopCutscene", 0f, string.Empty, null, null);
            _isRunning = false;
            //var sequencerLink = sequencer.GetComponent<SequencerLink>();
            // SequenceComplete only gets called on the last sequencer to stop playing.
            //if (sequencerLink != null)
            //    sequencerLink.SequenceComplete();
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            //_startCutsceneId = MessageDispatcher2.Instance.StartListening("StartCutscene", entity.UniqueId, (data) =>
            //{
            //    SetupSequence();
            //});
            _startConversationId = MessageDispatcher2.Instance.StartListening("StartConversation", entity.UniqueId, (data) =>
            {
                var currentSequence = GameController.Instance.UIManager.CurrentSequence;
                currentSequence?.Pause();
            });
            _endConversationId = MessageDispatcher2.Instance.StartListening("EndConversation", entity.UniqueId, (data) =>
            {
                var currentSequence = GameController.Instance.UIManager.CurrentSequence;
                currentSequence?.Play();
            });
            _sequenceCompleteId = MessageDispatcher2.Instance.StartListening("SequenceComplete", entity.UniqueId, (data) =>
            {
                AllSequencesComplete();
            });
        }

        public override void StopListening(IComponentRepository entity)
        {
            base.StopListening(entity);
            //MessageDispatcher2.Instance.StopListening("StartCutscene", entity.UniqueId, _startCutsceneId);
            MessageDispatcher2.Instance.StopListening("StartConversation", entity.UniqueId, _startConversationId);
            MessageDispatcher2.Instance.StopListening("EndConversation", entity.UniqueId, _endConversationId);
            MessageDispatcher2.Instance.StopListening("SequenceComplete", entity.UniqueId, _sequenceCompleteId);
        }

        public override void End()
        {
            Debug.LogError("(uSequencer) Exiting State");
            var currentSequence = GameController.Instance.UIManager.CurrentSequence;
            if (currentSequence == null)
                return;
            //if (currentSequence.IsPlaying())
            //{
            //    currentSequence.Pause();
            //}
            //else
            //{
            //    GameController.Instance.UIManager.CurrentSequence = null;
            //}

            //currentSequence.PlaybackFinished = null;
            //if (currentSequence.IsPlaying())
            //    currentSequence.Stop();
            currentSequence.OnSequenceComplete -= CurrentSequence_OnSequenceComplete;
            GameController.Instance.UIManager.CurrentSequence = null;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
