using System;
using RQ.Common.Components;
using RQ.Model.Interfaces;
using RQ.Serialization;
using System.Collections.Generic;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Action Controller")]
    public class ActionController : ComponentPersistence<ActionController>, IActionController
    {
        //public string Name;

        //private IList<ConditionalBase> _conditionals;
        //private IList<ActionBase> _actions;

        //private IList<ActionSequence> _actionSequences;
        [SerializeField]
        private bool _runAutomatically = false;
        public List<ActionSequence> StartSceneActions;
        [SerializeField]
        private List<ActionSequence> RunningActions;
        public List<ActionSequence> EndSceneActions;
        private Dictionary<string, IList<ActionSequence>> _actionSequences;
        private bool _isFirstUpdate = true;
        //[SerializeField]
        //private List<ActionSequence> BossDiedActions;

        private long _checkAndRunActionSequencesId, _checkAndRunActionSequencesId2;
        private Action<Telegram2> _checkAndRunActionSequencesDelegate;

        public override void Awake()
        {
            base.Awake();
            //GameController._instance.ActionController = this;
            //_actionManagers = new List<ActionManager>();
            //_conditionals = GetComponents<ConditionalBase>();
            //_actions = GetComponents<ActionBase>();
            _checkAndRunActionSequencesDelegate = (data) =>
            {
                var actionSequenceType = (string) data.ExtraInfo;
                CheckAndRunActionSequences(actionSequenceType);
            };
        }

        public override void Start()
        {
            base.Start();
            if (_runAutomatically)
                CheckAndRunActionSequences("Start");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_runAutomatically)
                CheckAndRunActionSequences("End");
        }

        public override void Update()
        {
            base.Update();
            if (_isFirstUpdate)
            {
                if (_runAutomatically)
                    CheckAndRunActionSequences("FirstUpdate");
                _isFirstUpdate = false;
            }
        }

        // Only instantiate this list once. Might want to pool this.
        private List<ActionSequence> actionsToRemove = new List<ActionSequence>();

        // TODO May want to put this in a coroutine so it runs less often
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            actionsToRemove.Clear();

            foreach (var action in RunningActions)
            {
                bool ran = action.CheckAndRun();
                if (ran && action.RunOnce)
                    actionsToRemove.Add(action);
            }
            foreach (var action in actionsToRemove)
            {
                RunningActions.Remove(action);
            }
        }

        public override void StartListening()
        {
            base.StartListening();
            if (!Application.isPlaying)
                return;
            _checkAndRunActionSequencesId = MessageDispatcher2.Instance.StartListening("CheckAndRunActionSequences", _componentRepository.UniqueId,
                _checkAndRunActionSequencesDelegate);
            _checkAndRunActionSequencesId2 = MessageDispatcher2.Instance.StartListening("CheckAndRunActionSequences", this.UniqueId,
                _checkAndRunActionSequencesDelegate);
            //_componentRepository.StartListening("CheckAndRunActionSequences", this.UniqueId, , true);
        }

        public override void StopListening()
        {
            base.StopListening();
            if (!Application.isPlaying)
                return;
            MessageDispatcher2.Instance.StopListening("CheckAndRunActionSequences", _componentRepository.UniqueId, _checkAndRunActionSequencesId);
            MessageDispatcher2.Instance.StopListening("CheckAndRunActionSequences", this.UniqueId, _checkAndRunActionSequencesId2);
            //_componentRepository.StopListening("CheckAndRunActionSequences", this.UniqueId, true);
        }

        public void RegisterActionManager(ActionSequence actionSequence)
        {
            if (StartSceneActions.Contains(actionSequence))
                return;
            if (EndSceneActions.Contains(actionSequence))
                return;
            if (RunningActions.Contains(actionSequence))
                return;
            if (_actionSequences == null)
            {
                _actionSequences = new Dictionary<string, IList<ActionSequence>>();
            }
            IList<ActionSequence> actionSequences;
            var actionSequenceType = actionSequence.GetActionSequenceType();
            if (!_actionSequences.TryGetValue(actionSequenceType, out actionSequences))
            {
                actionSequences = new List<ActionSequence>();
                _actionSequences.Add(actionSequenceType, actionSequences);
            }
            Debug.LogWarning("Registering action sequence " + actionSequence.name);
            actionSequences.Add(actionSequence);
        }

        public void RunStartSceneActions()
        {
            if (!isActiveAndEnabled)
                return;
            for (var i = 0; i < StartSceneActions.Count; i++)
            {
                StartSceneActions[i].CheckAndRun();
            }
        }

        public void RunEndSceneActions()
        {
            if (!isActiveAndEnabled)
                return;
            for (var i = 0; i < EndSceneActions.Count; i++)
            {
                EndSceneActions[i].CheckAndRun();
            }
        }

        public bool CheckAndRunActionSequences(string actionSequenceType)
        {
            IList<ActionSequence> actionSequences;
            if (_actionSequences == null)
                return false;
            if (!_actionSequences.TryGetValue(actionSequenceType, out actionSequences))
                return false;
            if (actionSequences == null)
                return false;
            bool anyRunning = false;
            foreach (var actionSequence in actionSequences)
            {
                if (actionSequence.CheckAndRun())
                    anyRunning = true;
            }
            return anyRunning;
        }

        public ActionSequence GetActionSequence(string actionSequenceType, string uniqueId)
        {
            foreach (var sequence in _actionSequences[actionSequenceType])
            {
                if (sequence.UniqueId == UniqueId)
                    return sequence;
            }
            //return _actionSequences[actionSequenceType].FirstOrDefault(i => i.UniqueId == uniqueId);
            return null;
        }

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            Debug.LogWarning("Serialize called on " + this.name);
            if (_actionSequences != null)
            {
                foreach (var sequenceTypes in _actionSequences)
                {
                    foreach (var sequence in sequenceTypes.Value)
                    {
                        sequence.Serialize(entitySerializedData);
                    }
                }
            }
            foreach (var sequence in StartSceneActions)
                sequence.Serialize(entitySerializedData);
            foreach (var sequence in EndSceneActions)
                sequence.Serialize(entitySerializedData);
            foreach (var sequence in RunningActions)
                sequence.Serialize(entitySerializedData);
        }

        public override void Deserialize(EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            Debug.LogWarning("Deserialize called on " + this.name);
            if (_actionSequences != null)
            {
                foreach (var sequenceTypes in _actionSequences)
                {
                    foreach (var sequence in sequenceTypes.Value)
                    {
                        sequence.Deserialize(entitySerializedData);
                    }
                }
            }
            foreach (var sequence in StartSceneActions)
                sequence.Deserialize(entitySerializedData);
            foreach (var sequence in EndSceneActions)
                sequence.Deserialize(entitySerializedData);
            foreach (var sequence in RunningActions)
                sequence.Deserialize(entitySerializedData);
        }
    }
}
