using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class SequencerLinkData
    {
        public string UniqueId { get; set; }
        public IEnumerable<SequencerLinkItemData> SequencerLinkItemDatas { get; set; }
    }

    [Serializable]
    public class SequencerLinkItemData
    {
        public string AffectedObjectUniqueId { get; set; }
        //public string AffectedObjectPath { get; set; }
        public int Index { get; set; }
    }
}
