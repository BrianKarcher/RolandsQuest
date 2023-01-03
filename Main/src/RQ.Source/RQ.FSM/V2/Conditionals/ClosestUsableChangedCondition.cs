using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [AddComponentMenu("RQ/States/Conditions/UI/Closest Usable Changed")]
    public class ClosestUsableChangedCondition : StateTransitionConditionBase
    {
        //private bool _isTelegramReceived = false;

        //[SerializeField]
        //private Enums.Telegrams _telegram = Enums.Telegrams.AnimationComplete;

        private string _closestUsable;

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;
            if (GameDataController.Instance.Data == null)
                return false;
            //var currentObject = GameDataController.Instance.Data.UsableContainer.CurrentUsableObject;
            //if (_closestUsable != currentObject)
            //{
            //    _closestUsable = currentObject;
            //    base.SetIsConditionSatisfied(true);
            //}

            return GetIsConditionSatisfied();
        }

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            //_closestUsable = UsableContainerController.Instance.Data.UsableContainer.CurrentUsableObject;
        }
    }
}
