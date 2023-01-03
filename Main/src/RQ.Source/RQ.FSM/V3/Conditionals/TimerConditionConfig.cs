using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V3.Conditionals
{
    //[AddComponentMenu("RQ/States/Conditions/Timer")]
    public class TimerConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private float _startTime = 0f;

        [SerializeField]
        private float _endTime = 0f;

        private Dictionary<string, long> _timesUpIds = new Dictionary<string, long>();
        private Action<Telegram2> _timesUpDelegate;
        //private string _messageKeyName;

        // TODO Get rid of the SerizeField attribute, only temporary for testing purposes
        // TODO Have the Message Dispatcher do the timer instead
        //[SerializeField]
        //private float _time = 0f;

        //protected IRQObject _entity;

        public override void SetEntity(IComponentRepository entity, string stateMachineId)
        {
            base.SetEntity(entity, stateMachineId);
            if (_endTime == 0f)
                _endTime = _startTime;
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
            var entity = stateMachine.GetComponentRepository();
            //Debug.LogError("Dispatching Times Up up for " + entity.name);
            MessageDispatcher2.Instance.DispatchMsg("TimesUp", delay, this.UniqueId, 
                entity.UniqueId, null);
            //MessageDispatcher.Instance.DispatchMsg(delay, this.UniqueId, this.UniqueId,
            //    Enums.Telegrams.StateComplete, null);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            //if (String.IsNullOrEmpty(_messageKeyName))
            //{
            //    _messageKeyName = entity.UniqueId;
            //}
            if (_timesUpDelegate == null)
            {
                _timesUpDelegate = (data) =>
                {
                    //Debug.LogError("Times up for " + entity.name);
                    SetIsConditionSatisfied(stateMachine, true);
                };
            }
            //entity.StartListening("TimesUp", this.UniqueId, );
            var id = MessageDispatcher2.Instance.StartListening("TimesUp", entity.UniqueId, _timesUpDelegate);
            _timesUpIds[entity.UniqueId] = id;
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            //entity.StopListening("TimesUp", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("TimesUp", entity.UniqueId, _timesUpIds[entity.UniqueId]);
            _timesUpIds.Remove(entity.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            return GetIsConditionSatisfied(stateMachine);
            //return Time.time > _stateInfo.StateStartTime + _time;
        }

        public float GetDelay()
        {
            return UnityEngine.Random.Range(_startTime, _endTime);
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
