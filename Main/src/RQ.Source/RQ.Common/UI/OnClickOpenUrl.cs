using RQ.Messaging;
using UnityEngine;

namespace RQ.Common.UI
{
    [AddComponentMenu("RQ/UI/On Click Open Url")]
    public class OnClickOpenUrl : MessagingObject
    {
        [SerializeField]
        private string _url;

        public void OnClick()
        {
            Application.OpenURL(_url);
            // Broadcast to all
            //MessageDispatcher2.Instance.DispatchMsg("ButtonClicked", 0f, this.UniqueId, null, _button);
        }
    }
}
