using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Enums;
//using RQ.Entities.Common;
using RQ.Messaging;
using RQ.Model.Serialization;
using System;
using UnityEngine;

namespace RQ.FSM.V2
{
    [ExecuteInEditMode]
    // Each State will be an asset
    public abstract class StateBase : ComponentPersistence<StateBase>, IState// where T : class, IRQObject
    //public abstract class StateBase : MessagingObject, IState// where T : class, IRQObject
    {
        //[UniqueIdentifier]
        //public string ID;
        // Hierarchical state machine
        [SerializeField]
        private StateMachine _stateMachine;

        private bool _isActive = false;
        protected bool _isSetup = false;

        public string Name { get; private set; }

        protected IComponentRepository _spriteBase;

        public override void Awake()
        {
            base.Awake();
            Name = name;
        }

        //protected string _spriteBaseId;

        //private bool _isInitialized = false;

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


        public IStateMachine StateMachine { get { return _stateMachine; } set { _stateMachine = (StateMachine)value; } }

        private IStateMachine _childStateMachine;

        //public override void Awake()
        //{
        //    base.Awake();


        //    //if (String.IsNullOrEmpty(RQName))
        //    //    RQName = name + " State";
        //}

        public override void Init()
        {
            base.Init();
            if (_childStateMachine == null)
                _childStateMachine = GetComponent<IStateMachine>();
            SetupStateMachine();
            if (StateMachine == null)
                throw new Exception("State Machine null in " + name);
            var spriteEntity = StateMachine.GetEntity();
            if (spriteEntity == null)
                throw new Exception("Entity null in " + name);
            _spriteBase = spriteEntity.GetComponent<IComponentRepository>();
            //_isInitialized = true;
            //SetComponentRepository();
            //RegisterComponentInRepo();
        }

        private void SetupStateMachine()
        {
            if (StateMachine == null)
                StateMachine = GetComponentInParent<StateMachine>();
        }

        //public override void Start()
        //{
        //    base.Start();
        //    if (!Application.isPlaying)
        //        return;

        //    var entity = _stateMachine.GetEntity();

        //    SetEntity(entity);
        //}

        //public T Entity { get; set; }

        //public virtual void SetEntity(Transform entity) 
        //{

        //}

        public IComponentRepository GetEntity()
        {
            if (StateMachine == null)
                return null;
            if (_spriteBase == null)
                _spriteBase = StateMachine.GetEntity().GetComponent<IComponentRepository>();
            return _spriteBase;
        }

        public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo,
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _spriteBase.UniqueId,
                msg, extraInfo, null, earlyTermination);
        }

        public void SendMessageToSelf(float delay, Telegrams msg, object extraInfo,
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, this.UniqueId,
                msg, extraInfo, null, earlyTermination);
        }

        public virtual void Enter()
        {
            _isActive = true;
            SetupState();
            //if (!_isInitialized)
            //{
            //    if (StateMachine == null)
            //        throw new Exception("Cannot locate State Machine in " + name);
            //    SetEntity(StateMachine.GetEntity());
            //}
            //MessageDispatcher.Instance.RegisterMessageHandler(this);
        }

        public virtual void SetupState()
        {
            _isSetup = true;
        }

        public virtual void Exit()
        {
            _isActive = false;
            // It is possible for Exit to be called before Enter is ever called
            // For example, if a state change is requested at scene startup, Exit
            // gets called right away
            //if (!_isInitialized)
            //    SetEntity(StateMachine.GetEntity());
            //MessageDispatcher.Instance.UnregisterMessageHandler(this);
            //_stateMachine.ChangeState(null);
            // Purge messages that are set to expire when exiting this state
            //MessageDispatcher.Instance.RemoveByEarlyTermination(agent.ID(), Enums.TelegramEarlyTermination.ExitChildState);
        }

        //public virtual void PhysicsUpdate()
        //{

        //}

        //this is the states normal update function
        //public virtual void Update()
        //{

        //}

        //public virtual void FixedUpdate()
        //{

        //}

        public override bool HandleMessage(Telegram telegram)
        {
            base.HandleMessage(telegram);
            //if (_childStateMachine != null)
            //    _childStateMachine.HandleMessage(telegram);

            switch (telegram.Msg)
            {
                case Enums.Telegrams.BattleMode:
                    _stateMachine.GetStateInfo().SwitchToBattle = (bool)telegram.ExtraInfo;
                    break;
                case Enums.Telegrams.StateComplete:
                    if (this.UniqueId == (string)telegram.ExtraInfo)
                    StateMachine.GetStateInfo().IsComplete = true;
                    break;
            }
            return false;
        }

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    //var mapping = new StateMapping()
        //    //{
        //    //    Name = name,
        //    //    UniqueId = UniqueId
        //    //};
        //    //if (entitySerializedData.UniqueIdMappings.ContainsKey(name))
        //    //    throw new Exception("State " + name + " already exists in the dictionary for " + StateMachine.GetEntity().name);
        //    //entitySerializedData.UniqueIdMappings.Add(name, UniqueId);
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    //if (!entitySerializedData.UniqueIdMappings.ContainsKey(name))
        //    //    return;
        //    //var mapping = entitySerializedData.UniqueIdMappings[name];
        //    //SetUniqueId(mapping);
        //}

        public virtual void SetEnabled(bool enable)
        {
            //this.
            if (this.gameObject != null)
                this.gameObject.SetActive(enable);
            //this.enabled = enabled;
        }

        public void Complete()
        {
            StateMachine.GetStateInfo().IsComplete = true;
        }

        public bool IsComplete()
        {
            return StateMachine.GetStateInfo().IsComplete;
        }

        public virtual void Serialize(StateData stateData)
        {

        }

        public virtual void Deserialize(StateData stateData)
        {

        }
        //public abstract int GetState();
    }
}
