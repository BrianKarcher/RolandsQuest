using RQ.Animation;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Direction Changed")]
    public class DirectionChangedCondition : StateTransitionConditionBase
    {
        private Direction _currentDirection;
        //private Direction _currentDirection = Direction.None;
        private AnimationComponent _animationComponent;

        // TODO Get rid of the SerizeField attribute, only temporary for testing purposes
        // TODO Have the Message Dispatcher do the timer instead
        //[SerializeField]
        //private float _time = 0f;

        //protected IRQObject _entity;

        //public override void Start()
        //{
        //    base.Start();
        //}

        // TODO Set the Animation Component in Awake!
        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            var animationComponents = entity.Components.GetComponents<AnimationComponent>();
            if (animationComponents != null)
            {
                for (int i = 0; i < animationComponents.Count; i++)
                //foreach (var component in animationComponents)
                {
                    var animComponent = animationComponents[i] as AnimationComponent;
                    if (animComponent.IsMain())
                    {
                        _animationComponent = animComponent;
                        break;
                    }
                }
                //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
            }
            //_animationComponent = entity.Components.GetComponents<AnimationComponent>()
            //    .FirstOrDefault(i => i.IsMain());
            _currentDirection = _animationComponent.Data.FacingDirection;
            //_animationComponent.FacingDirectionChanged += DirectionChanged;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (_animationComponent == null)
                return;
            //_animationComponent.FacingDirectionChanged += DirectionChanged;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_animationComponent == null)
                return;
            //_animationComponent.FacingDirectionChanged -= DirectionChanged;
        }

        public void DirectionChanged(Direction oldDir, Direction newDir)
        {
            base.SetIsConditionSatisfied(true);
        }

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);

        //    //_entity = entity;
        //    //SetTimer();
        //}

        //public override void ConditionEnter()
        //{
        //    base.ConditionEnter();
        //    var delay = GetDelay();
        //    MessageDispatcher.Instance.DispatchMsg(delay, this.UniqueId, this.UniqueId,
        //        Enums.Telegrams.StateComplete, null);
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return _currentDirection != _animationComponent.Data.FacingDirection;
            //return GetIsConditionSatisfied();
            //return Time.time > _stateInfo.StateStartTime + _time;
        }

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            _currentDirection = _animationComponent.Data.FacingDirection;
            //SetTimer();
        }

        //public void SetTimer()
        //{
        //    _time = ;
        //}

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.StateComplete:
        //            SetIsConditionSatisfied(true);
        //            break;
        //    }

        //    return false;
        //}
    }
}
