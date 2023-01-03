using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Is Stuck")]
    public class IsStuckCondition : StateTransitionConditionBase
    {
        //protected ISprite _entity;
        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        //public override void SetEntity(IRQObject entity, IStateMachine stateMachine)
        //{
        //    base.SetEntity(entity, stateMachine);
        //    //_entity = entity as ISprite;
        //    //_behavior = _entity.GetSteering().GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            //var isFinished = _behavior.Path.IsFinished;

            //if (isFinished)
            //{
            //    string hi = "hi8";
            //}

            //return isFinished;

            //return _entity.GetStateInfo().IsStuck;
            return stateMachine.GetStateInfo().IsStuck;

            //return _entity.IsAnimationComplete;
        }
    }
}
