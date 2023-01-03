using System;
using System.Collections.Generic;
using RQ.Messaging;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    //[AddComponentMenu("RQ/States/Conditions/Message Received 2")]
    public class MessageReceivedConditionConfig3 : StateTransitionConditionBaseConfig
    {
        //private bool _isTelegramReceived = false;

        [SerializeField]
        private string eventName;
        [SerializeField]
        private string _data = null;

        private Dictionary<string, long> _messageIds = new Dictionary<string, long>();
        private Action<Telegram2> _messageDelegate;
        //private string _messageKeyName;

        //public override void ConditionEnter(IStateMachine stateMachine) 
        //{
        //    base.ConditionEnter(stateMachine);
        //    //Listen(stateMachine);
        //}

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            //if (String.IsNullOrEmpty(_messageKeyName))
            //{
            //    var entity = stateMachine.GetComponentRepository();
            //    _messageKeyName = eventName + entity.UniqueId;
            //}
            if (_messageDelegate == null)
            {
                _messageDelegate = (data) =>
                {
                    if (!String.IsNullOrEmpty(_data) && _data != (string) data.ExtraInfo)
                        return;
                    if (eventName == "StartConversation")
                    {
                        Debug.LogError("MessageReceivedCondition3 - StartConversation");
                    }
                    if (eventName == "EndCutscene")
                    {
                        Debug.LogError("MessageReceivedCondition3 - EndCutscene");
                    }
                    //Debug.Log("Event " + eventName + " received in MessageReceivedConditionConfig3");
                    SetIsConditionSatisfied(stateMachine, true);
                    // Set up the next state right away if need be, no need to wait a frame
                    stateMachine.CalculateNextState();
                };
            }
            Listen(stateMachine);
        }

        public void Listen(IStateMachine stateMachine)
        {
            var entity = stateMachine.GetComponentRepository();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");

            if (eventName == "TitleScreen")
                Debug.LogWarningFormat("Listening to {0}", eventName + " for " + entity.name);

            var id = MessageDispatcher2.Instance.StartListening(eventName, entity.UniqueId, _messageDelegate);
            _messageIds[eventName + entity.UniqueId] = id;

            // TODO This is HORRIBLE! - Need to switch to instantiated Conditions ASAP!
            //entity.StartListening(eventName, this.UniqueId, _messageDelegate);
        }

        public override void ConditionExit(IStateMachine stateMachine) 
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            if (entity == null)
                throw new Exception("Message Received Condition - Entity is empty");
            //if (eventName == "StartConversation")
            //    Debug.LogWarningFormat("Stopping Listening to {0}", eventName);
            //if (eventName == "TitleScreen")
            //    Debug.LogWarningFormat("Stopping Listening to {0}", eventName + " for " + entity.name);
            MessageDispatcher2.Instance.StopListening(eventName, entity.UniqueId, _messageIds[eventName + entity.UniqueId]);
            _messageIds.Remove(eventName + entity.UniqueId);

            //entity.StopListening(eventName, this.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return GetIsConditionSatisfied(stateMachine);
        }
    }
}
