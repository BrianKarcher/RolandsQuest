using RQ.Common;
using RQ.Common.Container;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Serialization;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Send Message")]
    public class SendMessage : ActionBase
    {
        [SerializeField]
        private bool _sendToGameController = false;
        [SerializeField]
        private bool _sendToUIManager = false;
        [SerializeField]
        private BaseObject _target = null;
        [SerializeField]
        private Telegrams _telegram;
        [SerializeField]
        private string _data = null;
        //public AudioClip AudioClip;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            string target = null;
            if (_sendToGameController)
                target = "Game Controller";
            else if (_sendToUIManager)
                target = "UI Manager";
            else
            {
                // If no target, send it to yourself
                var sendTarget = _target == null ? GetEntity() as BaseObject : _target;
                var spriteBaseComponent = sendTarget?.GetComponent<SpriteBaseComponent>();
                if (spriteBaseComponent != null)
                    target = spriteBaseComponent.UniqueId;
                else
                    target = _target.UniqueId;
            }

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, target, _telegram, _data);
        }

        public override void Serialize(EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            var MessageData = new SendMessageData()
            {
                SendToGameController = _sendToGameController,
                Data = _data,
                SendToUIManager = _sendToUIManager
            };
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
            var MessageData = base.DeserializeComponent<SendMessageData>(entitySerializedData);
            if (MessageData == null)
                return;
            _sendToGameController = MessageData.SendToGameController;
            _data = MessageData.Data;
            _sendToUIManager = MessageData.SendToUIManager;
            if (!string.IsNullOrEmpty(MessageData.TargetUniqueId))
            {
                // TODO Change this to a deep search, right now it only searches for the Component Repo's.
                _target = EntityContainer._instance.GetEntity(MessageData.TargetUniqueId) as BaseObject;
            }
        }
    }
}
