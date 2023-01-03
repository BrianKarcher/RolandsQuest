using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RQ.Common.Config;
using UnityEngine;

namespace RQ.Model.Game_Data.Quest
{
    public class QuestConfig : RQBaseConfig
    {
        [SerializeField]
        private List<QuestEntryConfig> _questEntries;
        public List<QuestEntryConfig> QuestEntries
        {
            get => _questEntries;
            set => _questEntries = value;
        }

    }
}
