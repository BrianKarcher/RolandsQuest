using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Timer")]
    public class TimerCondition : StateTransitionConditionBase
    {
        [SerializeField]
        private float _startTime = 0f;

        [SerializeField]
        private float _endTime = 0f;

        private long _stateCompleteId;
        private Action<Telegram2> _stateCompleteActionDelegate;

        // TODO Get rid of the SerizeField attribute, only temporary for testing purposes
        // TODO Have the Message Dispatcher do the timer instead
        //[SerializeField]
        //private float _time = 0f;

        //protected IRQObject _entity;

        public override void Start()
        {
            base.Start();
            if (_endTime == 0f)
                _endTime = _startTime;
            if (_stateCompleteActionDelegate == null)
            {
                _stateCompleteActionDelegate = (data) =>
                {
                    if ((string) data.ExtraInfo == this.UniqueId)
                        SetIsConditionSatisfied(true);
                };
            }
        }

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);

        //    //_entity = entity;
        //    //SetTimer();
        //}

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            var delay = GetDelay();
            var entity = GetEntity();
            //entity.StartListening("StateComplete", 
            //MessageDispatcher2.Instance.RemoveMessages("", entity.UniqueId);
            MessageDispatcher2.Instance.DispatchMsg("StateComplete", delay, this.UniqueId, entity.UniqueId, this.UniqueId);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            var entity = GetEntity();
            if (_stateCompleteActionDelegate == null)
            {
                _stateCompleteActionDelegate = (data) =>
                {
                    if ((string)data.ExtraInfo == this.UniqueId)
                        SetIsConditionSatisfied(true);
                };
            }
            _stateCompleteId = MessageDispatcher2.Instance.StartListening("StateComplete", entity.UniqueId, _stateCompleteActionDelegate);
            //entity.StartListening("StateComplete", this.UniqueId, _stateCompleteActionDelegate);
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = GetEntity();
            //entity.StopListening("StateComplete", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("StateComplete", entity.UniqueId, _stateCompleteId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return GetIsConditionSatisfied();
            //return Time.time > _stateInfo.StateStartTime + _time;
        }

        //public override void ConditionReset()
        //{
        //    base.ConditionReset();
        //    //SetTimer();
        //}

        //public void SetTimer()
        //{
        //    _time = ;
        //}

        public float GetDelay()
        {
            return UnityEngine.Random.Range(_startTime, _endTime);
        }

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.StateComplete:
        //            //Debug.LogWarning("Timer " + this.name + " complete");
        //            SetIsConditionSatisfied(true);
        //            break;
        //    }

        //    return false;
        //}
    }
}
