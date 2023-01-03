using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using System;
using RQ.Controller.Contianers;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Current Usable Available")]
    public class CurrentUsableAvailableCondition : StateTransitionConditionBase
    {
        //protected UsableComponent _usableComponent;

        //public override void SetEntity(IComponentRepository entity, string stateMachineId)
        //{
        //    base.SetEntity(entity, stateMachineId);
        //    //_usableComponent = entity.Components.GetComponent<UsableComponent>();            
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            var usableContainer = UsableContainerController.Instance.UsableContainer;
            return !String.IsNullOrEmpty(usableContainer.GetCurrentUsable());
            //var currentUsable = usableContainer.GetCurrentUsable();
            //return currentUsable == _spriteBaseId;

            //return 

            //bool isInRange = false;

            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _usableComponent.UniqueId,
            //    Telegrams.IsInRange, null, (value) => isInRange = (bool)value);

            //return isInRange;

            //var isFinished = _behavior.Path.IsFinished;

            //if (isFinished)
            //{
            //    //string hi = "hi8";
            //}

            //return isFinished;

            //return _entity.IsAnimationComplete;
        }
    }
}
