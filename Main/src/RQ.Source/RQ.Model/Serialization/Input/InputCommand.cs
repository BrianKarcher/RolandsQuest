using UnityEngine;

namespace RQ.Model.Serialization.Input
{
    public class InputCommand
    {
        public InputAction InputAction { get; set; }
        public KeyCode KeyCode { get; set; }
        public bool IsAxis { get; set; }
        public string AxisName { get; set; }
        public bool IsAxisDirectionPositive { get; set; }
        //public InputType InputType { get; set; }

        public void ClearKeys()
        {
            KeyCode = KeyCode.None;
            IsAxis = false;
            AxisName = null;
        }
    }
}
