using System;
using UnityEngine;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class SendMessageData
    {
        [SerializeField]
        public bool SendToGameController = false;
        [SerializeField]
        public bool SendToUIManager = false;
        [SerializeField]
        public bool SendToMainCharacter = false;
        [SerializeField]
        public bool SendToAll = false;

        public bool SendToSelf = true;
        [HideInInspector]
        public string TargetUniqueId = null;
        [SerializeField]
        public string EventName;
        [SerializeField]
        public string Data = null;
    }
}
