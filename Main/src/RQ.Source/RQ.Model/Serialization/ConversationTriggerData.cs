
using System;
namespace RQ.Model.Serialization
{
    [Serializable]
    public class ConversationTriggerData
    {
        public string ActorUniqueId { get; set; }
        public string ConversantUniqueId { get; set; }
    }
}
