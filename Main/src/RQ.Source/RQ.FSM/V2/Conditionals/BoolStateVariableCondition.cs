using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Bool State Variable")]
    public class BoolStateVariableCondition : StateTransitionConditionBase
    {
        public enum Variable
        {
            IsComplete = 0,
            SwitchToBattle = 1,
            IsStuck = 2,
            ChangeScene = 3
            //IsDamaged = 3
        }

        [SerializeField]
        private Variable _variable = Variable.IsComplete;

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

            var stateInfo = stateMachine.GetStateInfo();

            switch (_variable)
            {
                case Variable.IsComplete:
                    return stateInfo.IsComplete;
                case Variable.SwitchToBattle:
                    return stateInfo.SwitchToBattle;
                case Variable.IsStuck:
                    return stateInfo.IsStuck;
                case Variable.ChangeScene:
                    return stateInfo.ChangeScene;
                //case Variable.IsDamaged:
                //    return GetEntity() as IBaseGameEntity()
                    //return stateInfo.IsDamaged;
            }

            return false;
            //var isFinished = _behavior.Path.IsFinished;

            //if (isFinished)
            //{
            //    string hi = "hi8";
            //}

            //return isFinished;

            //return _entity.GetStateInfo().IsStuck;
            //return stateMachine.GetStateInfo().IsComplete;

            //return _entity.IsAnimationComplete;
        }
    }
}
