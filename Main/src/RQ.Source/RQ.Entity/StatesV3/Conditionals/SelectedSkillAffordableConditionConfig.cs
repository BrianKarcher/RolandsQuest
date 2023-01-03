using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Item;
using System;

namespace RQ.Entity.StatesV3.Conditions
{
    [Obsolete]
    //[AddComponentMenu("RQ/States/Conditions/Animation Complete")]
    public class SelectedSkillAffordableConditionConfig : StateTransitionConditionBaseConfig
    {
        //protected AnimationComponent _animationComponent;

        //public override void ConditionInit(IStateMachine stateMachine)
        //{

        //}

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    //_entity = entity as IBaseGameEntity;
        //    //_animationComponent = entity.Components.GetComponent<AnimationComponent>();

        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            //return Utility.IsSelectedSkillAffordable();
            return true;
        }
    }
}
