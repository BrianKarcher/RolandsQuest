namespace RQ.FSM.V2.Conditionals
{
    /// <summary>
    /// A condition on whether or not to change states
    /// </summary>
    //public interface IStateTransitionCondition<T>
    //    where T : IRQObject
    //{
    //    bool TestCondition(T entity, IStateMachine stateMachine);
    //}

    public interface IStateTransitionCondition
    {
        //void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo);
        bool TestCondition(IStateMachine stateMachine);
        //void ConditionReset();
        string Name { get; set; }
        //void SetStateMachine(StateMachine stateMachine);
        void ConditionEnter(IStateMachine stateMachine);
        void ConditionInit(IStateMachine stateMachine);
        void ConditionExit(IStateMachine stateMachine);
        void ConditionReset(IStateMachine stateMachine);
    }
}
