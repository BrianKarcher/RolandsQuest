using RQ.Animation;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Animation Complete")]
    public class AnimationCompleteCondition : StateTransitionConditionBase
    {
        protected AnimationComponent _animationComponent;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            //_entity = entity as IBaseGameEntity;
            //_animationComponent = entity.Components.GetComponent<AnimationComponent>();
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animationComponents.Count; i++)
            {
                var animationComponent = animationComponents[i] as AnimationComponent;
                if (animationComponent.IsMain())
                {
                    _animationComponent = animationComponent;
                    break;
                }
            }
            //if (animationComponents != null)
            //{
            //    _animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
            //}
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            return _animationComponent.Data.IsAnimationComplete;
        }
    }
}
