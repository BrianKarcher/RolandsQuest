using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [Obsolete]
    [AddComponentMenu("RQ/States/Conditions/Message Received")]
    public class MessageReceivedCondition : StateTransitionConditionBase
    {
        //private bool _isTelegramReceived = false;

        [SerializeField]
        private Enums.Telegrams _telegram = Enums.Telegrams.AnimationComplete;
        [SerializeField]
        private string _data = null;
        private IStateMachine _stateMachine;
        private bool _isEnabled = false;
        //protected IRQEntity _entity;

        //public virtual void SetEntity(IRQObject entity)
        //{
        //    _entity = entity as ISprite;
        //}

        //[SerializeField]
        //private Button _button;

        //public void OnEnable()
        //{
        //    InputManager.Instance.AddEntity(this);
        //}

        //public void OnDisable()
        //{
        //    InputManager.Instance.RemoveEntity(this);
        //}

        public override void SetEntity(Entity.Components.IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            //if (Name == "Unpause")
            //    Debug.LogWarning("SetEntity called in Unpause");
            base.SetEntity(entity, stateMachineId, stateInfo);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            //if (Name == "Unpause")
            //    Debug.LogWarning("ConditionInit called in Unpause");
            base.ConditionInit(stateMachine);
            _stateMachine = stateMachine;
            if (_stateMachine == null)
                Debug.LogError("State Machine is null in " + GetEntity().name);
        }

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            _isEnabled = true;
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            _isEnabled = false;
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            return GetIsConditionSatisfied();
        }

        //public void LateUpdate()
        //{
        //    //_isButtonPressed = false;
        //}

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            //var data = agent as ISprite;

            if (telegram.Msg != _telegram)
                return false;
            if (!String.IsNullOrEmpty(_data) && _data != (string)telegram.ExtraInfo)
                return false;

            if (!_isEnabled)
                return false;
            SetIsConditionSatisfied(true);

            if (_stateMachine == null)
                Debug.LogError("State Machine is null in " + GetEntity().name + " " + Name);
            // Set up the next state right away if need be, no need to wait a frame
            _stateMachine.CalculateNextState();
            return false;
        }
    }
}
