using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Animation Complete")]
    public class AttackCountConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private int _attackCountTrigger;
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
            var damageComponent = stateMachine.GetComponentRepository().Components.GetComponent<DamageComponent>();
            if (damageComponent != null)
            {
                var attackCount = damageComponent.GetDamageData().AttackCount;
                if (attackCount == 0 && _attackCountTrigger != 0)
                    return false;
                return attackCount % _attackCountTrigger == 0;
                //var animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                //return animationComponent.Data.IsAnimationComplete;
            }

            // TODO Make this event-based to speed it up.
            //var animationComponents = stateMachine.GetComponentRepository().Components.GetComponents<AnimationComponent>();
            //if (animationComponents != null)
            //{
            //    var animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
            //    return animationComponent.Data.IsAnimationComplete;
            //}
            
            return false;
        }
    }
}
