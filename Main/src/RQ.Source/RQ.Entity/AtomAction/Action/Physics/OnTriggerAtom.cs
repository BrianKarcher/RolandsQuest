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
    public class OnTriggerAtom : AtomActionBase
    {
        public string triggerType;
        public string collideTag;
        private long _messageId;
        private long _messageExitId;
        private Action<GameObject> Collided;
        private Action<GameObject> Exited;

        //public SendMessageAtom()
        //{
        //    TargetUniqueIds = new List<string>();
        //}

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            _messageId = MessageDispatcher2.Instance.StartListening("TriggerEnter", entity.UniqueId, (data) =>
            {
                if (triggerType != "enter")
                    return;
                var collisionData = (CollisionMessageData)data.ExtraInfo;
                if (collisionData == null || collisionData.OtherCollider == null)
                    return;
                Debug.Log($"Collision between {collisionData.OtherCollider.tag} and {collideTag}");
                if (collisionData.OtherCollider.tag != collideTag)
                    return;
                Collided(collisionData.OtherCollider.gameObject);
            });
            _messageExitId = MessageDispatcher2.Instance.StartListening("TriggerExit", entity.UniqueId, (data) =>
            {
                if (triggerType != "exit")
                    return;
                var collisionData = (CollisionMessageData)data.ExtraInfo;
                if (collisionData.OtherCollider.tag != collideTag)
                    return;
                Exited(collisionData.OtherCollider.gameObject);
            });
        }

        public override void StopListening(IComponentRepository entity)
        {
            base.StopListening(entity);
            MessageDispatcher2.Instance.StopListening("TriggerEnter", entity.UniqueId, _messageId);
            MessageDispatcher2.Instance.StopListening("TriggerExit", entity.UniqueId, _messageExitId);
        }

        public void Process(string targetUniqueId)
        {
            //MessageDispatcher2.Instance.DispatchMsg(Message, 0f, _entity.UniqueId, targetUniqueId, null);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public void SetCollided(Action<GameObject> collided)
        {
            Collided = collided;
        }

        public void SetExited(Action<GameObject> exited)
        {
            Exited = exited;
        }
    }
}
