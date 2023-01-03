using System;

namespace RQ.FSM.V2.Conditionals
{
    //public class ConditionExpression<T>
    //    where T : IRQObject
    //{
    //    public StateTransitionConditionBase<T> Condition;
    //    public bool IsAnd;
    //}

    [Serializable]
    public class ConditionExpression
    {
        public string UniqueId;
        public StateTransitionConditionBase Condition;
        public StateTransitionConditionBaseConfig Condition2;
        public bool IsNot;

        public IStateTransitionCondition GetCondition()
        {
            var condition = Condition as IStateTransitionCondition;
            if (condition == null)
                condition = Condition2;
            return condition;
        }
    }
}
