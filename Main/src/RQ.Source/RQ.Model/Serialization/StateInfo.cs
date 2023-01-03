using RQ.Model.Serialization;
using System;
using System.Collections.Generic;

namespace RQ.FSM.V2
{
    [Serializable]
    public class StateInfo
    {
        public bool IsComplete;
        public bool IsStuck;
        public bool SwitchToBattle;
        public float StateStartTime;
        public bool ChangeScene;
        public string CurrentStateUniqueId;
        public string StateMachineUniqueId;
        public string StateMachineName;
        /// <summary>
        /// The key is the Unique Id of the condition, and the bool is if it is satisfied
        /// </summary>
        public Dictionary<string, bool> IsConditionSatisfied;
        public List<StateData> StateData;
        public string InitialStateUniqueId { get; set; }
        //public bool IsStateComplete { get; set; }
        //public float StateVelocity { get; set; }

        public StateInfo()
        {
            IsConditionSatisfied = new Dictionary<string, bool>();
        }
    }
}
