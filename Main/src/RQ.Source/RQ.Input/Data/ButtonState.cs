using System;

namespace RQ.Input.Data
{
    [Serializable]
    public class ButtonState
    {
        public bool IsButtonDown { get; set; }
        public bool IsButtonPressed { get; set; }
        public int ButtonPressCount { get; set; }
    }
}
