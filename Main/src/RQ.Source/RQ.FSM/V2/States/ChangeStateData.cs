using System;

namespace RQ.FSM.V2.States
{
    [Serializable]
    public class ChangeStateData
    {
        public StateMachine StateMachine;
        public StateBase State;
    }
}
