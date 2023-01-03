using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V2
{
    /// <summary>
    /// A single record in the State Transition table
    /// </summary>
    //public class StateTransitionRecord<T>
    //    where T : IRQObject
    //{
    //    public StateBase CurrentState;

    //    /// <summary>
    //    /// List of conditions to satisfy the transition. The bool determines whether it is a NOT condition
    //    /// </summary>
    //    public List<ConditionExpression<T>> Condition;

    //    public ConditionType Type;

    //    public StateBase TargetState;
    //}

    [Serializable]
    public class StateTransitionRecord
    {
        [SerializeField]
        private string _name;
        public StateBase CurrentState;
        public string UniqueId;

        /// <summary>
        /// List of conditions to satisfy the transition. The bool determines whether it is a NOT condition
        /// </summary>
        public List<ConditionExpression> Conditions;

        public ConditionType Type;
        public bool PreviousState;

        public StateBase TargetState;

        public StateBase TargetStateSelected;

        public bool IsActive = true;

        public string Name { get { return _name; } set { _name = value; } }
    }
}
