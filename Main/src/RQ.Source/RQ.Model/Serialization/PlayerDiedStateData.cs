using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class PlayerDiedStateData
    {
        public bool NotifyGameController { get; set; }

        public string ActionControllerUniqueId { get; set; }
    }
}
