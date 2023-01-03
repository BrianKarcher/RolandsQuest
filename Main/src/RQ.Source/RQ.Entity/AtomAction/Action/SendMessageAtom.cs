using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Messaging;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SendMessageAtom : AtomActionBase
    {
        public string Message;
        public string[] TargetUniqueIds;
        public string collideTag;
        
        public bool _sendToSelf = false;
        public bool _sendToAll = false;
        public bool _sendToMainCharacter;
        public bool _sendToUsableController;
        public bool _sendToGameController;
        public bool _sendToUIManager;
        public SendMessageTime SendMessageTime;
        public bool _finishOnFirstMessageSent = true;

        //public bool _sendOnFirstUpdate = false;
        private System.Action DebugEvent;

        private long _messageId;
        private bool _processedFirstUpdate = false;
        private bool _messageSent = false;

        //public override reset

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _messageSent = false;
            _processedFirstUpdate = false;
            if (Message == "VictoryPose")
            {
                DebugEvent?.Invoke();
                Debug.LogError("SendMessage VictoryPose called");
                int i = 1;
            }
            if (SendMessageTime == SendMessageTime.Immediately)
            {
                SendMessage(entity);
            }
        }

        public override void End()
        {
            base.End();
            if (SendMessageTime == SendMessageTime.OnExit)
            {
                SendMessage(_entity);
            }
        }

        private void SendMessage(IComponentRepository entity)
        {
            if (Message == "TitleScreen")
            {
                Debug.LogError($"Sending TitleScreen message from {entity.name}");
            }
            if (_sendToAll)
            {
                // null dispatches to all who are listening
                Process(null);
            }
            else if (_sendToSelf)
            {
                Process(entity.UniqueId);
            }
            else if (_sendToMainCharacter)
            {
                var mainCharacter = base.GetTarget(Model.Enums.ActionTarget.MainCharacter);
                if (mainCharacter == null)
                    Debug.LogError($"No Main Character to send message to.");
                else
                    Process(mainCharacter.UniqueId);
            }
            else if (_sendToUsableController)
            {
                Process("Usable Controller");
            }
            else if (_sendToUIManager)
            {
                Process("UI Manager");
            }
            else if (_sendToGameController)
            {
                Process("Game Controller");
            }
            else
            {
                for (int i = 0; i < TargetUniqueIds.Length; i++)
                {
                    Process(TargetUniqueIds[i]);
                }
            }
            _messageSent = true;
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            if (Message == "DisperseHelixPull")
                Debug.Log("Listening to DisperseHelixPull");
            if (SendMessageTime != SendMessageTime.TriggerEnter)
                return;
            _messageId = MessageDispatcher2.Instance.StartListening("TriggerEnter", entity.UniqueId, (data) =>
            {

                var collisionData = (CollisionMessageData)data.ExtraInfo;
                if (collisionData.OtherCollider.tag != collideTag)
                    return;
                var otherCollisionComponent = collisionData.OtherCollider.GetComponent<CollisionComponent>();
                if (otherCollisionComponent == null)
                    return;
                var repo = otherCollisionComponent.GetComponentRepository();
                if (repo == null)
                    return;
                Debug.Log("Caught trigger for " + repo.UniqueId);
                // Send the message to both the requested recipients and also the target we hit.
                // TODO May want to change this with a flag on whether to send to target we hit or not.
                SendMessage(_entity);
                Process(repo.UniqueId);
                //Received(null, EventArgs.Empty);
                //_isRunning = false;
            });
        }

        public override void StopListening(IComponentRepository entity)
        {
            base.StopListening(entity);
            if (SendMessageTime != SendMessageTime.TriggerEnter)
                return;
            MessageDispatcher2.Instance.StopListening("TriggerEnter", entity.UniqueId, _messageId);
        }

        public void Process(string targetUniqueId)
        {
            if (Message == "TitleScreen")
            {
                Debug.LogError($"Sending TitleScreen message to {targetUniqueId}");
            }
            MessageDispatcher2.Instance.DispatchMsg(Message, 0f, _entity.UniqueId, targetUniqueId, null);            
        }

        public override AtomActionResults OnUpdate()
        {
            if (!_processedFirstUpdate && SendMessageTime == SendMessageTime.FirstUpdate)
            {
                SendMessage(_entity);
                _processedFirstUpdate = true;
            }

            return AtomActionResults.Success;
        }

        public bool IsFinished()
        {
            // Never finishes
            if (!_finishOnFirstMessageSent)
                return false;
            return _messageSent;
        }

        public void SetDebugEvent(System.Action debugEvent)
        {
            DebugEvent = debugEvent;
        }
    }
}
