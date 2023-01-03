using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Common.UI
{
    [AddComponentMenu("RQ/UI/On Click Handler")]
    public class OnClickHandler : MessagingObject
    {
        [SerializeField]
        private string _button;

        private Action _clickAction;

        public virtual void OnClick()
        {
            _clickAction?.Invoke();
            // Broadcast to all
            MessageDispatcher2.Instance.DispatchMsg("ButtonClicked", 0f, this.UniqueId, null, _button);
        }

        public void SetClickAction(Action clickAction)
        {
            _clickAction = clickAction;
        }
    }
}
