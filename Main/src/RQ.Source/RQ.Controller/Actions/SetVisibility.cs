using RQ.Enums;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Set Visibility")]
    public class SetVisibility : ActionBase
    {
        [SerializeField]
        private bool _visibilityOnEnter;
        [SerializeField]
        private bool _visibilityOnExit;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                GetEntity().UniqueId, Telegrams.SetRender, _visibilityOnEnter);
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                GetEntity().UniqueId, Telegrams.SetRender, _visibilityOnExit);
        }
    }
}
