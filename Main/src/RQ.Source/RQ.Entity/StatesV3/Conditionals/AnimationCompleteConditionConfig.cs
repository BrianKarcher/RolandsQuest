using RQ.Animation;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Animation Complete")]
    public class AnimationCompleteConditionConfig : StateTransitionConditionBaseConfig
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
            // TODO Make this event-based to speed it up.
            var animationComponents = stateMachine.GetComponentRepository().Components.GetComponents<AnimationComponent>();
            if (animationComponents != null)
            {
                for (int i = 0; i < animationComponents.Count; i++)
                {
                    var aniamtionComponent = animationComponents[i] as AnimationComponent;
                    if (aniamtionComponent.IsMain())
                    {
                        return aniamtionComponent.Data.IsAnimationComplete;
                    }
                }
                //var animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
            }
            
            return false;
        }
    }
}
