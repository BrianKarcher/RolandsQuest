using RQ.Common;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Serialization;
using RQ.Serialization;
using System;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Send Message 2")]
    public class SendMessage2 : ActionBase
    {
        [Obsolete]
        [SerializeField]
        private bool _sendToGameController = false;
        [Obsolete]
        [SerializeField]
        private bool _sendToUIManager = false;
        [SerializeField]
        private BaseObject _target = null;
        [Obsolete]
        [SerializeField]
        private string eventName;
        [Obsolete]
        [SerializeField]
        private string _data = null;
        [SerializeField]
        private SendMessageData MessageData;
        //public AudioClip AudioClip;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            string target = null;
            if (MessageData.EventName == "StartCutscene")
            {
                int i = 0;
            }
            if (MessageData.SendToGameController)
                target = "Game Controller";
            else if (MessageData.SendToUIManager)
                target = "UI Manager";
            else if (MessageData.SendToMainCharacter)
                target = EntityContainer._instance.GetMainCharacter().UniqueId;
            else if (MessageData.SendToAll)
            {
                target = null;
            }
            else if (MessageData.SendToSelf)
            {
                var sendTarget = _target == null ? GetEntity() as BaseObject : _target;
                var spriteBaseComponent = sendTarget?.GetComponent<IComponentRepository>();
                if (spriteBaseComponent != null)
                    target = spriteBaseComponent.UniqueId;
                else
                    target = _target.UniqueId;
            }
            else
            {
                // If no target, send it to yourself
                var sendTarget = _target == null ? GetEntity() as BaseObject : _target;
                var spriteBaseComponent = sendTarget?.GetComponent<IComponentRepository>();
                if (spriteBaseComponent != null)
                    target = spriteBaseComponent.UniqueId;
                else
                    target = _target.UniqueId;
            }
            //if (MessageData.EventName == "EndCutscene")
            //{
            //    //Debug.LogError("SendMessage: EndCutscene Called");
            //}
            MessageDispatcher2.Instance.DispatchMsg(MessageData.EventName, 0f, this.UniqueId, 
                target, MessageData.Data);
        }

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            //var killObjectData = new SendMessageData();
            if (_target != null)
            {
                var repo = _target.GetComponent<IComponentRepository>();
                MessageData.TargetUniqueId = repo.UniqueId;
            }
            base.SerializeComponent(entitySerializedData, MessageData);
        }

        public override void Deserialize(EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            MessageData = base.DeserializeComponent<SendMessageData>(entitySerializedData);
            if (MessageData == null)
                return;
            if (!string.IsNullOrEmpty(MessageData.TargetUniqueId))
            {
                // TODO Change this to a deep search, right now it only searches for the Component Repo's.
                _target = EntityContainer._instance.GetEntity(MessageData.TargetUniqueId) as BaseObject;
            }
        }
    }
}
