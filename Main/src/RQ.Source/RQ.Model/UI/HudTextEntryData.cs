using UnityEngine;

namespace RQ.Model.UI
{
    public class HudTextEntryData
    {
        public object Data { get; set; }
        public Color Color { get; set; }
        public float Duration { get; set; }

        public HudTextEntryData() { }
        public HudTextEntryData(object data, Color color, float duration)
        {
            Data = data;
            Color = color;
            Duration = duration;
        }
    }
}
