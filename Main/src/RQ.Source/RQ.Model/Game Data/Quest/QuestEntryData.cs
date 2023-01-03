using System;

namespace RQ.Model.Game_Data.Quest
{
    [Serializable]
    public class QuestEntryData
    {
        //[SerializeField]
        public string QuestEntryConfigUniqueId { get; set; }

        //public int Index { get; set; }

        public int Id { get; set; }

        public QuestStatus Status { get; set; }
        //public bool IsComplete { get; set; }

        [NonSerializedAttribute]
        private QuestEntryConfig _questEntryConfig;
        public QuestEntryConfig QuestEntryConfig
        {
            get => _questEntryConfig;
            set => _questEntryConfig = value;
        }
    }
}
