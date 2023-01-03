using System.Collections.Generic;

namespace RQ.Model.Game_Data.Quest
{
    public class QuestData
    {
        public List<QuestEntryData> QuestEntryDatas { get; set; }

        public string QuestEntryConfigUniqueId { get; set; }

        //public int CurrentIndex { get; set; }

        //public bool IsComplete { get; set; }
        public QuestStatus QuestStatus { get; set; }
    }
}
