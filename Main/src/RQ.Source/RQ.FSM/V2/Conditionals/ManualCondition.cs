using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Manual")]
    public class ManualCondition : StateTransitionConditionBase
    {
        public void SetComplete(bool satisfied)
        {
            SetIsConditionSatisfied(satisfied);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return GetIsConditionSatisfied();
        }
    }
}
