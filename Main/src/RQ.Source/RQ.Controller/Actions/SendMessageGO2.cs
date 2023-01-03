using RQ.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Send Message GO 2")]
    public class SendMessageGO2 : ActionBase
    {
        [SerializeField]
        private bool _sendToGameController = false;
        [SerializeField]
        private bool _sendToUIManager = false;
        [SerializeField]
        private BaseObject _target = null;
        [SerializeField]
        private string eventName;
        [SerializeField]
        private Transform _data;
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
                var spriteBaseComponent = sendTarget?.GetComponent<IComponentRepository>();
                if (spriteBaseComponent != null)
                    target = spriteBaseComponent.UniqueId;
                else
                    target = _target.UniqueId;
            }

            MessageDispatcher2.Instance.DispatchMsg(eventName, 0f, this.UniqueId, target, _data);
        }
    }
}
