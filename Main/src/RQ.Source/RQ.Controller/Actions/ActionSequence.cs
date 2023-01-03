using RQ.Controller.Actions.Conditionals;
using RQ.Extensions;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Serialization;
using RQ.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Action Sequence")]
    public class ActionSequence : MessagingObject, IActionSequence
    {
        //[UniqueIdentifier]
        //public string UniqueId;
        //public string Name;
        [SerializeField]
        private string _actionSequenceType;
        public ActionController ActionController;
        [SerializeField]
        private bool _runOnce = true;
        public bool RunOnce { get { return _runOnce; } }
        private bool _hasRun = false;

        private IList<ConditionalBase> _conditionals;
        private IList<ActionBase> _actions;

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;
            _conditionals = GetComponents<ConditionalBase>();
            _actions = GetComponents<ActionBase>();
            //if (_actions != null)
            //{
            //    _actions = _actions
            //}
            if (ActionController != null)
            {
                //Debug.LogWarning("Registering action sequence " + this.name);
                ActionController.RegisterActionManager(this);
            }
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            foreach (var condition in _conditionals)
            {
                condition.InitCondition();
            }
        }

        public bool Check()
        {
            if (RunOnce && _hasRun)
                return false;
            var isSuccess = true;
            for (var i = 0; i < _conditionals.Count; i++)
            {
                var conditional = _conditionals[i];
                // If any conditionals fail, the check fails.  This is an AND operation
                if (!conditional.Check())
                {
                    isSuccess = false;
                    break;
                }
            }
            return isSuccess;
        }

        public void Run()
        {
            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    if (!action.isActiveAndEnabled)
                        continue;
                    action.Act(null);
                }
                //var activeActions = _actions.Where(i => i.isActiveAndEnabled).ToList();
                //for (var i = 0; i < activeActions.Count; i++)
                //{
                //    var action = activeActions[i];
                //    action.Act(null);
                //}
                _hasRun = true;
            }
        }

        public bool CheckAndRun()
        {
            if (Check())
            {
                Run();
                return true;
            }
            return false;
        }

        public void Serialize(EntitySerializedData entitySerializedData)
        {
            var actionSequenceData = new ActionSequenceData()
            {
                HasRun = _hasRun
            };

            entitySerializedData.SerializeComponent(this, actionSequenceData);
            foreach (var conditional in _conditionals)
            {
                conditional.Serialize(entitySerializedData);
            }
            foreach (var action in _actions)
            {
                action.Serialize(entitySerializedData);
            }
            
        }

        public void Deserialize(EntitySerializedData entitySerializedData)
        {
            var data = entitySerializedData.DeserializeComponent<ActionSequenceData>(this);
            _hasRun = data.HasRun;
            foreach (var conditional in _conditionals)
            {
                conditional.Deserialize(entitySerializedData);
            }
            foreach (var action in _actions)
            {
                action.Deserialize(entitySerializedData);
            }
        }

        public string GetActionSequenceType()
        {
            return _actionSequenceType;
        }
    }
}
