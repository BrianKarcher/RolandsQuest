using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Is Complete")]
    public class IsCompleteCondition : StateTransitionConditionBaseConfig
    {
        public override bool TestCondition(IStateMachine stateMachine)
        {
            return stateMachine.GetStateInfo().IsComplete;
        }
    }
}
