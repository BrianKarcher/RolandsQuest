using RQ.Common.Actions;
using RQ.Model;
using RQ.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V2.States
{
    [AddComponentMenu("RQ/States/State/Run Actions")]
    public class RunActionsState : StateBase
    {
        //[SerializeField]
        private IAction[] _actions;

        //public override void Awake()
        //{
        //    base.Awake();
            
        //}

        //public override void Start()
        //{
        //    base.Start();
        //}

        public override void Init()
        {
            base.Init();
            if (!Application.isPlaying)
                return;
            _actions = GetComponents<IAction>();
            //foreach (var action in _actions)
            //{
            for (int i = 0; i < _actions.Length; i++)
            {
                var action = _actions[i];
                action.SetState(this);
                action.InitAction();
            }
        }

        public override void Enter()
        {
            base.Enter();
            if (_actions == null)
                return;
            //foreach (var action in _actions)
            //{
            for (int i = 0; i < _actions.Length; i++)
            {
                var action = _actions[i];
                action.Act(null);
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (_actions == null)
                return;
            //foreach (var action in _actions)
            //{
            for (int i = 0; i < _actions.Length; i++)
            {
                var action = _actions[i];
                action.ActExit(null);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_actions == null)
                return;
            //foreach (var action in _actions)
            //{
            for (int i = 0; i < _actions.Length; i++)
            {
                var action = _actions[i];
                action.FixedUpdate();
            }
        }

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            ActionSerializerHelper.SerializeActions(entitySerializedData, gameObject);
        }

        public override void Deserialize(EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            ActionSerializerHelper.DeserializeActions(entitySerializedData, gameObject);
        }

        public override void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        {
            base.DeserializeUniqueIds(entitySerializedData);
            ActionSerializerHelper.DeserializeActionUniqueIds(entitySerializedData, gameObject);
        }
    }
}
