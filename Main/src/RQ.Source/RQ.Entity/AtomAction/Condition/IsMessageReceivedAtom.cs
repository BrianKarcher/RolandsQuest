using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.AtomAction.Condition
{
    public class IsMessageReceivedAtom : AtomActionBase
    {
        public string Message;
        public string Data;
        private long _messageId;
        public event EventHandler Received;
        private bool _executedAction = false;
        private GameObject _senderGameObject;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _executedAction = false;
            _senderGameObject = null;
        }

        public override void End()
        {
            base.End();
            //if (Message == "VictoryPose")
            //{
            //    Debug.LogError("(IsMessageReceivedAtom.End()) VictoryPose called");
            //    int i = 1;
            //}
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            //if (Message == "TitleScreen")
            //    Debug.LogError($"{entity.name} Listening to TitleScreen");
            _messageId = MessageDispatcher2.Instance.StartListening(Message, entity.UniqueId, (data) =>
            {
                if (Message != data.EventName)
                {
                    Debug.LogError($"(IsMessageReceivedAtom) '{Message}' '{data.EventName}' mismatch");
                }
                if (!String.IsNullOrEmpty(Data))
                {
                    string externData = data.ExtraInfo.ToString();
                    if (Data != externData)
                        return;
                }
                if (Received == null)
                {
                    Debug.LogError($"(IsMessageReceived) Received handler broken in {Message} {Data} for {entity.name}");
                }
                else
                {
                    var sender = data.ExtraInfo as GameObject;
                    _senderGameObject = sender;
                    //if (sender != null)
                    //{
                    //    _senderGameObject = sender;
                    //    //var senderEntity = EntityContainer._instance.GetEntity(sender);
                    //    //if (senderEntity != null)
                    //    //{
                    //    //    _senderGameObject = senderEntity.gameObject;
                    //    //}
                    //}
                    Received(null, EventArgs.Empty);
                }
                
                _executedAction = true;
                //_isRunning = false;
            });
        }

        public override void StopListening(IComponentRepository entity)
        {
            base.StopListening(entity);
            MessageDispatcher2.Instance.StopListening(Message, entity.UniqueId, _messageId);
        }

        public override AtomActionResults OnUpdate()
        {
            return _executedAction ? AtomActionResults.Success : AtomActionResults.Failure;
        }

        public GameObject GetSenderGameObject()
        {
            return _senderGameObject;
        }
    }
}
