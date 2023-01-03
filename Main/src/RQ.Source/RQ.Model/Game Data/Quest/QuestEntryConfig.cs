using System;
using UnityEngine;

namespace RQ.Model.Game_Data.Quest
{
    [Serializable]
    public class QuestEntryConfig
    {
        [SerializeField]
        private int _index;
        public int Index { get => _index; set => _index = value; }

        [SerializeField]
        private int _id;
        public int Id { get => _id; set => _id = value; }

        [SerializeField]
        private string _uniqueId;
        public string UniqueId
        {
            get => _uniqueId;
            set => _uniqueId = value;
        }

        [SerializeField]
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}
