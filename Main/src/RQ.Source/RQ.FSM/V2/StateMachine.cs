using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Entity.Components;
using RQ.Extensions;
using RQ.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V2
{
    [AddComponentMenu("RQ/States/State Machine")]
    [ExecuteInEditMode]
    public class StateMachine : ComponentPersistence<StateMachine>, IStateMachine  //where T : class
    {
        //[UniqueIdentifier]
        //public string ID;

        //a pointer to the agent that owns this instance
        //public Transform Entity;

        [SerializeField]
        private StateBase _initialState;

        public StateBase InitialState { get { return _initialState; } set { _initialState = value; } }

        [SerializeField]
        private StateTransitionTable _stateTransitionTable;

        [SerializeField]
        private StateBase _currentState;

        [SerializeField]
        private StateMachineReentry _stateMachineReentry = StateMachineReentry.FromPrevious;

        //a record of the last state the agent was in
        private IState _previousState;

        public List<IState> States { get; set; }

        private bool _hasStateMachineStarted = false;
        [SerializeField]
        private StateInfo _stateInfo = new StateInfo();
        [SerializeField]
        private bool _logTransitions;

        private IState _parentState;
        //bool _firstRun = true;        

        //this is called every time the FSM is updated
        //private IState _globalState;

        //private StateMachine _childStateMachine;

        //private List<IState> _childrenStates = new List<IState>();

        public override void Init()
        {
            base.Init();
            // Generally, the state in the same game object as the
            // State Machine is its parent
            _parentState = GetComponent<IState>();

            //States = transform.GetComponentsInChildrenOneDeep<IState>().ToList(); //GetComponentsInChildren<IState>().ToList();
            States = new List<IState>();
            transform.GetComponentsInChildrenOneDeep<IState>(States);
            //States = _childrenStates;
            if (_stateTransitionTable == null)
                _stateTransitionTable = GetComponent<StateTransitionTable>();

            _currentState = null;

            var recreateOnLoadGame = GetComponentRepository().GetRecreateOnLoadGame();
            //if (recreateOnLoadGame && GameDataController.Instance.LoadingGame)
            //    return;
            

            //_stateInfo = new StateInfo();
            _stateInfo.StateMachineUniqueId = this.UniqueId;
            _stateInfo.StateMachineName = this.name;
            //_owner = owner;
            //_currentState = _initialState;
            //_previousState = null;
            //_globalState = null;
            //DisableStates();
        }

        public override void Reset()
        {
            base.Reset();
            //ChangeStateToInitial();
            //_hasStateMachineStarted = false;
            _currentState = null;
            //_currentState = _initialState;
            _previousState = null;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            var recreateOnLoadGame = GetComponentRepository().GetRecreateOnLoadGame();
            //if (recreateOnLoadGame && GameDataController.Instance.LoadingGame)
            //    return;
           
            //if (!Application.isPlaying)
            //{
            //    if (String.IsNullOrEmpty(this.ID))
            //        ID = Guid.NewGuid().ToString();
            //    UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
            //}



            DisableStates();
            if (_hasStateMachineStarted)
            {
                //Debug.Log("StateMachine - Enable called on state already started");
                StateBase state;
                if (_stateMachineReentry == StateMachineReentry.FromStart)
                {
                    state = _initialState;
                }
                else
                {
                    state = _currentState;
                }
                ChangeState(state);
            }

            //MessageDispatcher2.Instance.StartListening("ChangeStateByName", this.UniqueId, (data) =>
            //{
            //    var newName = data.ToString();
            //    ChangeStateByName(newName);
            //});
            
            _previousState = null;
        }

        //public virtual void OnDestroy()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        UniqueIdRegistry.Deregister(this.ID);
        //    }
        //}

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;

            //MessageDispatcher2.Instance.StopListening("ChangeStateByName", this.UniqueId);

            if (_currentState != null)
            {
                ExitStateConditions(_currentState);
                _currentState.Exit();
            }
                
            //_currentState = null;
            //_previousState = null;
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_currentState != null)
                ExitStateConditions(_currentState);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_currentState != null)
                ExitStateConditions(_currentState);
        }

        public override void Start()
        {
            base.Start();
            

            if (!Application.isPlaying)
                return;

            //if (name == "Primary Action")
            //{
            //    int i = 0;
            //}

            // The dots need to be connected for the Unity Editor to function properly
            // Such as my Property Drawers for locating animations... maybe not
            // This explodes on inactive objects.  Switching to a "pull" method to get this info
            // in the states on Enter when it is not yet initialized.  Or maybe in OnEnable.
            // Or maybe in Start.  ... I'm thinking Start may be best.
            //if (_componentRepository != null)
            //{
            //    for (int i = 0; i < States.Count; i++)
            //    {
            //        States[i].SetEntity(_componentRepository.transform);
            //    }
            //}


            //RegisterToMessageDispatcher();

            //if (!_hasStateMachineStarted)
            //{
            //    //SetupStates();
            _hasStateMachineStarted = true;
            //    //SetupStateTransitionTable();
            //    //GetEntityComponents();
            //}

            var recreateOnLoadGame = GetComponentRepository().GetRecreateOnLoadGame();

            //if (recreateOnLoadGame && GameDataController.Instance.DestoyEntitiesOnAwake)
            //    return;
            //if (recreateOnLoadGame && GameDataController.Instance.LoadingGame)
            //    return;

            //_stateTransitionTable.SetStateInfo(_stateInfo);

                //StartStateMachine();

            //_stateTransitionTable.SetStateMachine(this.UniqueId);
            //if (_currentState != null)
            //    _currentState.Enter();
            ChangeState(_initialState);
        }

        //private void StartStateMachine()
        //{
        //    if (_componentRepository == null)
        //        throw new Exception("Component Repository in State Machine not set.");
        //    //Debug.Log("Starting state machine " + name + " for entity " + GetEntity().name);
        //    Log.Info("State Machine - Start called for entity " + _componentRepository.name);

        //    //SetupStates();

        //    //SetupConditions();
        //    //_hasStateMachineStarted = true;

        //    //SetupStateTransitionTable();
        //    //GetEntityComponents();

        //    //ChangeState(_initialState);
        //}

        //private void SetupStates()
        //{
        //    foreach (var state in States)
        //    {
        //        // TODO - Change SetEntity so it only runs once
        //        // Optional TODO - Have the entity component ids live in the State Machine
        //        // and the States can get them from there
        //        state.SetEntity(GetEntity());
        //    }
        //}

        //private void SetupStateTransitionTable()
        //{
        //    if (_stateTransitionTable != null)
        //    {
        //        // Notify the State Transition Table that this is the state machine
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateTransitionTable.UniqueId,
        //            Enums.Telegrams.SetStateMachine, this.UniqueId);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateTransitionTable.UniqueId,
        //        //    Enums.Telegrams.SetEntity, _spriteBaseId);
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateTransitionTable.UniqueId,
        //            Enums.Telegrams.SetEntity, _componentRepository.transform);
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateTransitionTable.UniqueId,
        //            Enums.Telegrams.Setup, null);
        //        _stateTransitionTable.SetStateMachine(this);
        //    }
        //}

        //private void GetEntityComponents()
        //{

        //}

        //private void RegisterToMessageDispatcher()
        //{
        //    var messageDispatcher = GetMessageDispatcher();
        //    if (messageDispatcher == null)
        //    {
        //        messageDispatcher = Entity.GetComponent<MessageDispatcher>();
        //    }
        //    SetMessageDispatcher(messageDispatcher);
        //    RegisterMessageHandler();
        //}

        private void DisableStates()
        {
            //foreach (var state in States)
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                //States[i].SetEntity(Entity);
                //if ((StateBase)state != _initialState)
                    state.SetEnabled(false);
            }
        }

        private void SetInactiveStatesToDisabled()
        {
            //foreach (var state in States)
            //{
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                if (state != _currentState)
                    state.SetEnabled(false);
            }
        }

        /// <summary>
        /// TODO - Change this to only run 10 times per second instead of the current 50
        /// </summary>
        //public void FixedUpdate()
        //{
        //    if (_stateTransitionTable != null)
        //    {
        //        //Log.Info("Checking next state");
        //        var nextState = _stateTransitionTable.CalculateNextState(_currentState, this);
        //        if (nextState != null)
        //        {
        //            ChangeState(nextState);
        //        }
        //    }
        //}

        // Doing this in Update because FixedUpdate does not work when the game is puased
        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;
            //if (_firstRun)
            //{
            //    // Used to be in Start

            //    _firstRun = false;
            //}
            //if (GameDataController.Instance.LoadingGame)
            //    return;

            //if (!Application.isPlaying)
            //{
            //    if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.ID))
            //    {
            //        ID = Guid.NewGuid().ToString();
            //        UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
            //    }
            //}

            CalculateNextState();
        }

        public void CalculateNextState()
        {
            if (_stateTransitionTable != null)
            {
                //Log.Info("Checking next state");
                var nextState = _stateTransitionTable.CalculateNextState(_currentState, this);
                if (nextState != null)
                {
                    ChangeState(nextState);
                }
            }
        }

        //public override void FixedUpdate()
        //{
        //    base.FixedUpdate();
        //    if (_currentState != null)
        //        _currentState.PhysicsUpdate();
        //}

        //use these methods to initialize the FSM
        public void SetCurrentState(IState state) 
        { 
            _currentState = state as StateBase;
            if (_currentState != null)
            {
                _stateInfo.CurrentStateUniqueId = state.UniqueId;
                ResetStateInfo();
                _currentState.SetEnabled(true);
            }
            _stateInfo.StateStartTime = Time.time;
        }
        public void SetCurrentState(string stateId)
        {
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                if (state.UniqueId == stateId)
                {
                    SetCurrentState(state);
                    break;
                }
                //var state = States.FirstOrDefault(i => i.UniqueId == stateId);
                //if (state != null)
            }
        }
        //public void SetGlobalState(IState s) { _globalState = s; }
        public void SetPreviousState(IState state) 
        { 
            _previousState = state; 
        }

        //call this to update the FSM
        //public void Update()
        //{
        //    //if a global state exists, call its execute method, else do nothing
        //    if (_globalState != null) _globalState.Update(_owner);

        //    //same for the current state
        //    if (_currentState != null) _currentState.Update(_owner);

        //    // repeat for child state machine
        //    if (_childStateMachine != null) _childStateMachine.Update();
        //}

        //public void FixedUpdate()
        //{
        //    //if a global state exists, call its execute method, else do nothing
        //    if (_globalState != null) _globalState.FixedUpdate(_owner);

        //    //same for the current state
        //    if (_currentState != null) _currentState.FixedUpdate(_owner);

        //    // repeat for child state machine
        //    if (_childStateMachine != null) _childStateMachine.FixedUpdate();
        //}

        public override bool HandleMessage(Telegram msg)
        {
            //if (!_hasStateMachineStarted)
            //{
            //    Debug.LogError("State machine " + this.name + " sent message " + msg.Msg + " before it has started.");
            //    StartStateMachine();
            //}

            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                //case Enums.Telegrams.RequestStateInfo:
                //    // Send state info back to the sender
                //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, msg.SenderId,
                //        Enums.Telegrams.SetStateInfo, _stateInfo);
                //    return false;
                case Enums.Telegrams.ChangeState:
                    ChangeState((string)msg.ExtraInfo);
                    return false;
                case Enums.Telegrams.ChangeStateByName:
                    ChangeStateByName((string)msg.ExtraInfo);
                    return false;
                case Enums.Telegrams.Kill:
                    _currentState = null;
                    break;
            }
            // If the state machine cannot handle it, pass it along to the conditions and active state
            foreach (var condition in _stateTransitionTable.GetConditions())
            {
                condition.HandleMessage(msg);
            }
            //first see if the current state is valid and that it can handle
            //the message
            //if (_currentState != null && _currentState.HandleMessage(msg))
            //{
            //    return true;
            //}

            //if not, and if a global state has been implemented, send 
            //the message to the global state
            //if (_globalState != null && _globalState.OnMessage(_owner, msg))
            //{
            //    return true;
            //}

            //if (_childStateMachine != null)
            //{
            //    return _childStateMachine.HandleMessage(msg);
            //}

            return false;
        }

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    base.SerializeComponent(entitySerializedData, _stateInfo);
        //    _stateTransitionTable.CustomSerialize(entitySerializedData);
        //    _stateInfo.StateData = new List<StateData>();
        //    if (_initialState != null)
        //    {
        //        _stateInfo.InitialStateUniqueId = _initialState.UniqueId;
        //    }
        //    if (States == null)
        //        throw new Exception("No states in " + entitySerializedData.Name + " " + this.name);
        //    foreach (var state in States)
        //    {
        //        var stateData = new StateData();
        //        stateData.DataObjects = new Dictionary<string, object>();
        //        state.Serialize(stateData);
        //        stateData.StateUniqueId = state.UniqueId;
        //        _stateInfo.StateData.Add(stateData);
        //    }
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    // For newly created objects in the Deserialize process,
        //    // The UniqueId is not relevant since it is created new and thus does not match
        //    // the recorded value
        //    // Must use Name instead, and Name must not be repeated in the entity
            
        //    // StateInfo just got changed, need to reset the State Transition Table to reflect this.
        //    //SetupStateTransitionTable();
        //    if (!entitySerializedData.ComponentData.ContainsKey(GetName()))
        //    {
        //        Debug.Log("(" + GetEntity().name + ") StateInfo not found for " + GetName());
        //        gameObject.SetActive(false);
        //        return;
        //    }
        //    _stateInfo = base.DeserializeComponent<StateInfo>(entitySerializedData);
        //    if (!String.IsNullOrEmpty(_stateInfo.InitialStateUniqueId))
        //    {
        //        var initialState = States.FirstOrDefault(i => i.UniqueId == _stateInfo.InitialStateUniqueId);
        //        _initialState = initialState as StateBase;
        //    }

        //    if (_stateInfo.StateData != null)
        //    {
        //        foreach (var stateData in _stateInfo.StateData)
        //        {
        //            var state = States.FirstOrDefault(i => i.UniqueId == stateData.StateUniqueId);
        //            if (state != null)
        //                state.Deserialize(stateData);
        //        }
        //    }

        //    SetUniqueId(_stateInfo.StateMachineUniqueId, true);
        //    //if (!entitySerializedData.StateInfos.ContainsKey(this.UniqueId))
        //    //    return;
        //    //_stateInfo = entitySerializedData.StateInfos[this.UniqueId];
        //    //SetCurrentState(_stateInfo.CurrentStateUniqueId);
        //    var currentState = States.FirstOrDefault(i => i.UniqueId == _stateInfo.CurrentStateUniqueId);
        //    _currentState = currentState as StateBase;
            
        //    if (_currentState != null)
        //    {
        //        _currentState.SetupState();
        //        _currentState.SetEnabled(true);                
        //    }
        //    SetInactiveStatesToDisabled();

        //    //var stateInfo = entitySerializedData.StateInfos.FirstOrDefault(i => i.Value.StateMachineName == this.name);
        //    //if (stateInfo.Value == null)
        //    //    return;
        //    //_stateInfo = stateInfo.Value;
        //    _stateTransitionTable.CustomDeserialize(entitySerializedData);
        //    if (_currentState != null)
        //        InitStateConditions(_currentState);
        //    //else
        //    //    Debug.LogError(entitySerializedData.Name + " - " + this.name + " has no current state");
        //}

        public void ChangeState(string stateId)
        {
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                if (state.UniqueId == stateId)
                {
                    ChangeState(state);
                    break;
                }
            }
        }

        public void ChangeStateByName(string name)
        {
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                if (state.Name == name)
                {
                    ChangeState(state);
                    break;
                }
            }
        }

        public IEnumerable<IState> GetStates()
        {
            return States;
        }

        //change to a new state
        public void ChangeState(IState newState)
        {
            //string name;
            //if (this.name == "Primary Action")
            //{
            //    int i = 0;
            //}
            //if (newState != null)
            //    name = newState.name;
            //else
            //    name = "(null)";
            //if (_logTransitions)
            //    Debug.Log("Changing state to " + name + " in entity " + _componentRepository.name);


            //if (newState == null)
            //    throw new Exception("<StateMachine::ChangeState>:trying to assign null state to current");
            //assert(pNewState && "<StateMachine::ChangeState>:trying to assign null state to current");

            //_childStateMachine = null;

            //keep a record of the previous state
            _previousState = _currentState;

            if (_currentState != null)
            {
                ExitStateConditions(_currentState);
                //call the exit method of the existing state
                ExitCurrentState();
            }

            //change state to the new state
            //_currentState = newState as StateBase;
            //_stateInfo.StateStartTime = Time.time;
            SetCurrentState(newState);

            if (_currentState != null)
            {
                //call the entry method of the new state
                
                EnterStateConditions(_currentState);
                InitStateConditions(_currentState);
                
                // Enter gets components, which rely on the state being enabled first
                //if (_currentState != null)
                //    _currentState.SetEnabled(true);
                try
                {
                    _currentState.Enter();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
                // Sometimes the state's Enter command will destroy the object, wherein _currentState 
                // becomes NULL

            }
        }

        public void ResetStateInfo()
        {
            _stateInfo.IsComplete = false;
            _stateInfo.ChangeScene = false;
            _stateInfo.IsConditionSatisfied.Clear();
            _stateInfo.IsStuck = false;
        }

        public void ExitCurrentState()
        {
            _currentState.Exit();
            _currentState.SetEnabled(false);
        }

        public void ChangeStateToInitial()
        {
            ChangeState(_initialState);
        }

        // Allows conditions to reset things like timers etc.
        public void ExitStateConditions(IState state)
        {
            _stateTransitionTable.ExitStateConditions(state);
        }

        public void EnterStateConditions(IState state)
        {
            _stateTransitionTable.EnterStateConditions(state);
        }

        public void InitStateConditions(IState state)
        {
            _stateTransitionTable.InitStateConditions(state);
        }

        //change state back to the previous state
        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }

        //returns true if the current state's type is equal to the type of the
        //class passed as a parameter. 
        public bool isInState(IState st)
        {
            return _currentState.GetType() == st.GetType();
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }
        //public IState GetGlobalState()
        //{
        //    return _globalState;
        //}
        public IState GetPreviousState()
        {
            return _previousState;
        }

        public Transform GetEntity()
        {
            return _componentRepository.transform;
        }

        public IComponentRepository GetComponentRepository()
        {
            return _componentRepository;
        }

        public StateInfo GetStateInfo()
        {
            return _stateInfo;
        }

        public IState GetParentState()
        {
            return _parentState;
        }

        //public virtual void Exit()
        //{
        //    _currentState.Exit();
        //    _currentState = null;
        //}

        //public float GetStateStartTime()
        //{
        //    return StateStartTime;
        //}

        //public StateMachine<T> Child()
        //{
        //    return _childStateMachine;
        //}

        //public void CreateChildStateMachine(T owner)
        //{
        //    _childStateMachine = new StateMachine<T>(owner);
        //}



        //only ever used during debugging to grab the name of the current state
        //std::string         GetNameOfCurrentState()const
        //{
        //  std::string s(typeid(*m_pCurrentState).name());

        //  //remove the 'class ' part from the front of the string
        //  if (s.size() > 5)
        //  {
        //    s.erase(0, 6);
        //  }

        //  return s;
        //}
    }
}
