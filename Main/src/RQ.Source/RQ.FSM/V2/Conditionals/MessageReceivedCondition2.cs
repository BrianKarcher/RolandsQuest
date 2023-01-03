using RQ.Entity.Components;
using System;
using RQ.Messaging;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [Obsolete]
    [AddComponentMenu("RQ/States/Conditions/Message Received 2")]
    public class MessageReceivedCondition2 : StateTransitionConditionBase
    {
        //private bool _isTelegramReceived = false;

        [SerializeField]
        private string eventName;
        [SerializeField]
        private string _data = null;
        //private IStateMachine _stateMachine;
        private long _messageId;

        public Action<Telegram2> _messageDelegate;


        public override void Awake()
        {
            base.Awake();
            _messageDelegate = (data) =>
            {
                if (!String.IsNullOrEmpty(_data) && _data != (string) data.ExtraInfo)
                    return;
                SetIsConditionSatisfied(true);
                if (_stateMachine == null)
                    Debug.LogError("State Machine is null in " + GetEntity().name + " " + name);
                // Set up the next state right away if need be, no need to wait a frame
                _stateMachine.CalculateNextState();
            };
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");
            _stateMachine = stateMachine;
            if (_stateMachine == null)
                Debug.LogError("State Machine is null in " + GetEntity().name);

            _messageId = MessageDispatcher2.Instance.StartListening(eventName, entity.UniqueId, _messageDelegate);
            //entity.StartListening(eventName, this.UniqueId, );
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");
            MessageDispatcher2.Instance.StopListening(eventName, entity.UniqueId, _messageId);
            //entity.StopListening(eventName, this.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return GetIsConditionSatisfied();
        }

        //public override void OnEnable()
        //{
        //    if (!Application.isPlaying)
        //        return;
        //    base.OnEnable();
        //    //var entity = GetEntity();
        //    //if (entity == null)
        //    //    throw new Exception("Message Received Condition - Entity is empty");
        //}

        //public override void StartListening()
        //{
        //    base.StartListening();
        //    GetEntity().StartListening(eventName, this.UniqueId, (data) =>
        //    {
        //        if (!String.IsNullOrEmpty(_data) && _data != (string)data.ExtraInfo)
        //            return;
        //        Log.Info("Event " + eventName + " received in MessageReceivedCondition2");
        //        SetIsConditionSatisfied(true);
        //    });
        //}

        //public override void StopListening()
        //{
        //    base.StopListening();
        //    GetEntity().StopListening(eventName, this.UniqueId);
        //}

        //public override void OnDisable()
        //{
        //    if (!Application.isPlaying)
        //        return;
        //    base.OnDisable();
        //    //GetEntity().StopListening(eventName, this.UniqueId);
        //}

        //public override bool TestCondition(IStateMachine stateMachine)
        //{
        //    if (!base.TestCondition(stateMachine))
        //        return false;

        //    return GetIsConditionSatisfied();
        //}
    }
}
