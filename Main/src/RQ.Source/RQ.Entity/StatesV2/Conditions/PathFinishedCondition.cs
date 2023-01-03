using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Path Finished")]
    public class PathFinishedCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        private RQ.Physics.SteeringBehaviors.FollowPath _behavior;
        private SteeringBehaviorManager _steering;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            //_entity = entity as ISprite;
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.GetSteering, null, (steering) => _steering = (SteeringBehaviorManager)steering);

            _behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        }

        //public override bool HandleMessage(Messaging.Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.SetSteering:
        //            _steering = (SteeringBehaviorManager)telegram.ExtraInfo;
        //            break;
        //    }

        //    return false;
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            var isFinished = _behavior.Path.IsFinished;

            if (isFinished)
            {
                //string hi = "hi8";
            }

            return isFinished;

            //return _entity.IsAnimationComplete;
        }
    }
}
