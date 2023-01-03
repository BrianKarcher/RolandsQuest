using System;

namespace RQ.Model.UI
{
    public class SaveSlotData
    {
        public string FileName { get; set; }
        public string Chapter { get; set; }
        public bool IsAutoSave { get; set; }
        public bool IsNewSlot { get; set; }
        public int Level { get; set; }
        public int SaveCount { get; set; }
        public int Gold { get; set; }
        public string Time { get; set; }
        public DateTime Date { get; set; }
    }
}
