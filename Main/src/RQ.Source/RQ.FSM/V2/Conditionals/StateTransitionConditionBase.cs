using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    //public abstract class StateTransitionConditionBase<T> : ScriptableObject, IStateTransitionCondition<T>
    //    where T : IRQObject
    //{
    //    public virtual bool TestCondition(T entity, IStateMachine stateMachine)
    //    {
    //        if (entity == null)
    //            throw new Exception("Entity is null in condition test");
    //        return true;
    //    }
    //}

    [ExecuteInEditMode]
    public abstract class StateTransitionConditionBase : MessagingObject, IStateTransitionCondition
    {
        //[UniqueIdentifier]
        //public string ID;

        //private IComponentRepository _entity;
        //private IStateMachine _stateMachine;
        protected string _stateMachineId;
        [SerializeField]
        private string _name;
        //[SerializeField]
        //private StateMachine _stateMachine;
        protected StateInfo _stateInfo;
        protected string _spriteBaseId;
        private IComponentRepository _componentRepo;
        protected IStateMachine _stateMachine;
        //protected StateMachine _stateMachine;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo,
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _spriteBaseId,
                msg, extraInfo, null, earlyTermination);
        }

        //public virtual void OnEnable()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        if (String.IsNullOrEmpty(this.ID))
        //            ID = Guid.NewGuid().ToString();
        //        UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
        //    }
        //}

        //public virtual void OnDestroy()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        UniqueIdRegistry.Deregister(this.ID);
        //    }
        //}

        //public virtual void Update()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.ID))
        //        {
        //            ID = Guid.NewGuid().ToString();
        //            UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
        //        }
        //    }
        //}

        public virtual void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            //_entity = entity;
            //_stateMachine = stateMachine;
            //_stateMachineId = stateMachine.UniqueId;
            _stateMachineId = stateMachineId;
            //_stateMachine = stateMachine;
            //var spriteBase = entity.GetComponent<IComponentRepository>();
            _spriteBaseId = entity.UniqueId;
            _stateInfo = stateInfo;
            _componentRepo = entity;
            //_stateInfo = stateMachine.GetStateInfo();
            //_state
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateMachineId,
            //    Telegrams.RequestStateInfo, null);
        }

        protected IComponentRepository GetEntity()
        {
            return _componentRepo;
        }

        //public virtual void SetStateMachine(StateMachine stateMachine)
        //{
        //    _stateMachine = stateMachine;
        //}

        //public void SetEntity(Transform entity, IStateMachine stateMachine)
        //{
        //    SetEntity(entity.GetComponent<IComponentRepository>(), stateMachine);
        //}

        //public virtual IComponentRepository GetEntity()
        //{
        //    return _entity;
        //}

        public virtual bool TestCondition(IStateMachine stateMachine)
        {
            //if (_entity == null)
            //    throw new Exception("Entity is null in condition test");
            return true;
        }

        public virtual void ConditionExit(IStateMachine stateMachine)
        {
            //SetIsConditionSatisfied(false);
        }

        public virtual void ConditionReset(IStateMachine stateMachine)
        {
            SetIsConditionSatisfied(false);
        }

        public virtual void ConditionEnter(IStateMachine stateMachine)
        { }

        public virtual void ConditionInit(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void SetIsConditionSatisfied(bool satisfied)
        {
            if (_stateInfo == null)
            {
                int i = 1;
                throw new Exception("StateInfo not located in " + this.UniqueId + " " + this.name);
            }
            if (satisfied)
            {
                int p = 2;
            }
            if (!_stateInfo.IsConditionSatisfied.ContainsKey(this.UniqueId))
                _stateInfo.IsConditionSatisfied.Add(this.UniqueId, satisfied);
            else
                _stateInfo.IsConditionSatisfied[this.UniqueId] = satisfied;
        }

        public virtual bool GetIsConditionSatisfied()
        {
            if (_stateInfo == null)
            {
                int i = 1;
                throw new Exception("StateInfo not located in " + this.UniqueId + " " + this.name);
            }
            if (!_stateInfo.IsConditionSatisfied.ContainsKey(this.UniqueId))
                return false;
            else
                return _stateInfo.IsConditionSatisfied[this.UniqueId];
        }

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    if (base.HandleMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.SetStateInfo:
        //            _stateInfo = (StateInfo)telegram.ExtraInfo;
        //            break;
        //    }
        //    return false;
        //}
    }
}
