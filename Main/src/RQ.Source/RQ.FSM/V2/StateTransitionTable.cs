using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V2
{
    [AddComponentMenu("RQ/States/State Transition Table")]
    [ExecuteInEditMode]
    public class StateTransitionTable : ComponentPersistence<StateTransitionTable>
    {
        public List<StateTransitionRecord> Records;
        private List<StateTransitionConditionBase> _conditions;
        //private Dictionary<string, StateTransitionConditionBase> _uniqueConditions = new Dictionary<string, StateTransitionConditionBase>();
        private Dictionary<string, IList<StateTransitionRecord>> _stateRecords;
        //private Dictionary<IState, IEnumerable<StateTransitionRecord>> _stateRecords;
        private string _stateMachineId;
        private Transform _entity;
        private StateMachine _stateMachine;

        public ConditionsList ConditionsList;
        public Transform ConditionsListTransform;
        private bool _hasStarted = false;
        //private Dictionary<string, IRQConfig> _configs;

        public override void Awake()
        {
            base.Awake();
            _stateMachine = GetComponent<StateMachine>();
            _entity = _stateMachine.GetEntity().transform;

            _conditions = new List<StateTransitionConditionBase>();
            if (ConditionsList != null)
            {
                //_conditions = ConditionsList.GetComponents<StateTransitionConditionBase>().ToList();
                
                _conditions.AddRange(ConditionsList.GetComponents<StateTransitionConditionBase>());
                //_conditions.AddRange(ConditionsList.GetComponentsInChildren<StateTransitionConditionBase>());
                AddConditionRange(_conditions, ConditionsList.GetComponentsInChildren<StateTransitionConditionBase>());
            }
            else
            {
                //_conditions = transform.GetComponents<StateTransitionConditionBase>().ToList();
                //_conditions.AddRange(transform.GetComponents<StateTransitionConditionBase>());
                AddConditionRange(_conditions, transform.GetComponents<StateTransitionConditionBase>());
            }

            if (ConditionsListTransform != null)
            {
                //if (_conditions == null)
                //    _conditions = new List<StateTransitionConditionBase>();
                //_conditions.AddRange(ConditionsListTransform.GetComponents<StateTransitionConditionBase>());
                //_conditions.AddRange(ConditionsListTransform.GetComponentsInChildren<StateTransitionConditionBase>());
                AddConditionRange(_conditions, ConditionsListTransform.GetComponents<StateTransitionConditionBase>());
                AddConditionRange(_conditions, ConditionsListTransform.GetComponentsInChildren<StateTransitionConditionBase>());
            }

            //if (_conditions != null)
            //    _conditions = _conditions.Distinct().ToList();

            //if (Records != null)
            //{
            //    // Remove entries with no conditions
            //    foreach (var record in Records)
            //    {
            //        //var recordstodelete = record.Conditions.Where(i => i.Condition == null);
            //        record.Conditions.RemoveAll(i => i.GetCondition() == null);
            //        //foreach (var condition in record.Conditions)
            //        //{
            //        //    if (condition.Condition == null)
            //        //}
            //        //foreach (var condition in record.Conditions)
            //        //{
            //        //    if (!_uniqueConditions.ContainsKey(condition.Condition.UniqueId))
            //        //        _uniqueConditions.Add(condition.Condition.UniqueId, condition);
            //        //    //    _uniqueConditions[condition.Condition.UniqueId] = condition;
            //        //    //else

            //        //}
            //    }

            //    //var orderedRecords = Records.OrderBy(i => i.CurrentState.UniqueId);


            //}
        }

        private void AddConditionRange(List<StateTransitionConditionBase> conditionList, StateTransitionConditionBase[] conditions)
        {
            foreach (var condition in conditions)
            {
                bool isFound = IsConditionFound(condition);
                if (!isFound)
                    conditionList.Add(condition);
            }
        }

        private bool IsConditionFound(StateTransitionConditionBase conditionToCheck)
        {
            foreach (var condition in _conditions)
            {
                if (condition == conditionToCheck)
                    return true;
            }
            return false;
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            _stateMachineId = _stateMachine.UniqueId;
            // This is in Start and not Awake because the Unique Id's can get changed in Awake
            // If the states awake order is after the transition table
            //Debug.Log("STT Init beign called from Start");
            StartInit();
            //foreach (var state in Records.Where(i => i.CurrentState != null)
                //.Select(i => i.CurrentState).Distinct())
            HashSet<string> completedRecords = new HashSet<string>();
            foreach (var state in Records)
            {
                if (state.CurrentState == null)
                    continue;
                // Skip duplicates
                if (completedRecords.Contains(state.CurrentState.UniqueId))
                    continue;
                completedRecords.Add(state.CurrentState.UniqueId);


                // Reset the dict on any UniqueId change
                state.CurrentState.UniqueIdChanged += (a, b) =>
                {
                    Debug.Log("(" + _stateMachine.GetEntity().name + ":" + state.CurrentState.name + ") Unique Id changed from " + a + " to " + b);
                    StartInit();
                };
            }
        }

        public void StartInit()
        {
            //if (_isStarted)
            //    return;
            SetupConditions();
            _stateRecords = new Dictionary<string, IList<StateTransitionRecord>>();
            var states = _stateMachine.GetStates();
            foreach (var state in states)
            {
                var key = state.UniqueId;
                var stateRecordsList = new List<StateTransitionRecord>();
                foreach (var record in Records)
                {
                    var canAdd = record.CurrentState == state
                                 || (record.CurrentState == null && record.TargetState != state);
                    if (canAdd)
                        stateRecordsList.Add(record);
                }
                _stateRecords.Add(key, stateRecordsList);
                // A null CurrentState is an "Any" condition - automatically add unless the target is this
                //_stateRecords.Add(key, Records.Where(i => i.CurrentState == state
                //    || (i.CurrentState == null && i.TargetState != state)).ToList());
            }
            // Get the records for each state into a Dictionary for faster lookup
            //foreach (var state in Records.Where(i => i.CurrentState != null)
            //    .Select(i => i.CurrentState).Distinct())
            //{
            //    //var key = state == null ? "any" : state.UniqueId;
            //    var key = state.UniqueId;
            //    // A null CurrentState is an "Any" condition - automatically add
            //    _stateRecords.Add(key, Records.Where(i => i.CurrentState == state
            //        || i.CurrentState == null));
            //    //_stateRecords.Add(state, )
            //}
            _hasStarted = true;
        }


        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    if (!Application.isPlaying)
        //    {
        //        UniqueIdRegistry.Deregister(this.ID);
        //    }
        //}


        //public override void Update()
        //{
        //    base.Update();
        //    if (!Application.isPlaying)
        //    {
        //        if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.ID))
        //        {
        //            var instanceId = this.GetInstanceID();
        //            UniqueIdRegistry.GetInstanceId(this.ID);
        //            ID = Guid.NewGuid().ToString();
        //            UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
        //        }
        //    }
        //}


        //public override void OnEnable()
        //{
        //    base.OnEnable();
        //    if (!Application.isPlaying)
        //    {
        //        if (String.IsNullOrEmpty(this.ID))
        //            ID = Guid.NewGuid().ToString();
        //        UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
        //    }
        //    else
        //    {

        //    }
        //}

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    if (!Application.isPlaying)
        //    {
        //        UniqueIdRegistry.Deregister(this.ID);
        //    }
        //}

        //public override void Update()
        //{
        //    base.Update();
        //    if (!Application.isPlaying)
        //    {
        //        if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.ID))
        //        {
        //            ID = Guid.NewGuid().ToString();
        //            UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
        //        }
        //    }
        //}

        // Loose coupling, the Messenger will handle communications
        //private void SetStateMachine(string stateMachineId)
        //{
        //    _stateMachineId = stateMachineId;
        //}

        public IState CalculateNextState(IState currentState, IStateMachine stateMachine)
        {
            if (currentState == null)
                return null;

            if (_stateRecords == null)
                StartInit();

            if (!_stateRecords.ContainsKey(currentState.UniqueId))
            {
                return null;
            }

            //foreach (var record in Records.Where(i => i.CurrentState == currentState))
            // This is faster, no?
            var transitionRecords = _stateRecords[currentState.UniqueId];
            //foreach (var record in )
            for (int i = 0; i < transitionRecords.Count; i++)
            {
                var record = transitionRecords[i];
                if (!record.IsActive)
                    continue;
                if (record.Conditions != null)
                {
                    bool isAnyFalse = false;
                    //foreach (var condition in record.Conditions)
                    for (int k = 0; k < record.Conditions.Count; k++)
                    {
                        var condition = record.Conditions[k];
                        var conditionTest = condition.GetCondition();

                        if (conditionTest == null)
                        {
                            continue;
                        }

                        var testResult = conditionTest.TestCondition(stateMachine);
                        // Flip the bit if it is NOT
                        if (condition.IsNot)
                        {
                            testResult = !testResult;
                            //testResult = testResult == true ? false : true;
                        }
                        if (record.Type == ConditionType.Or && testResult == true)
                        {
                            // An Or condition is allowed to circuit break
                            //Log.Info("Sprite " + stateMachine.GetEntity().name + " changed state because of " + record.Name);
                            return GetTarget(record);
                        }
                        if (!testResult)
                            isAnyFalse = true;
                        if (record.Type == ConditionType.And && testResult == false)
                        {
                            // An And condition is allowed to circuit break
                            break;
                        }
                    }
                    if (record.Type == ConditionType.And && !isAnyFalse)
                    {
                        //Log.Info("Sprite " + stateMachine.GetEntity().name + " changed state because of " + record.Name);
                        return GetTarget(record);
                    }
                }
            }
            // Don't change states
            return null;
        }

        private IState GetTarget(StateTransitionRecord record)
        {
            if (record.PreviousState)
                return _stateMachine.GetPreviousState();
            if (record.TargetStateSelected != null)
                return record.TargetStateSelected;
            return record.TargetState;
        }

        public List<StateTransitionConditionBase> GetConditions()
        {
            return _conditions;
        }

        public void SetStateMachine(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void SetupConditions()
        {
            if (_entity == null)
                return;

            var componentRepository = _entity.GetComponent<IComponentRepository>();

            if (_conditions != null)
            {
                foreach (var condition in _conditions)
                {
                    //if (_stateMachineId.StartsWith("4e263"))
                    //{
                    //    int i = 1;
                    //}
                    //var rqObject = stateEntity.GetRQObject();
                    condition.SetEntity(componentRepository, _stateMachineId, _stateMachine.GetStateInfo());
                }
            }
            //if (Records != null)
            //{
            //    //Log.Info("Checking next state");
            //    foreach (var record in Records)
            //    {
            //        if (record.Conditions != null)
            //        {
            //            foreach (var condition in record.Conditions)
            //            {
            //                //var stateEntity = Entity.GetComponent<IStateEntity>();
            //                if (_entity != null)
            //                {
            //                    if (_stateMachineId.StartsWith("4e263"))
            //                    {
            //                        int i = 1;
            //                    }
            //                    //var rqObject = stateEntity.GetRQObject();
            //                    condition.Condition.SetEntity(_entity.GetComponent<IComponentRepository>(), _stateMachineId);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public void ExitStateConditions(IState state)
        {
            if (_stateRecords == null)
                StartInit();

            if (!_stateRecords.TryGetValue(state.UniqueId, out var stateTransitionRecords))
            {
                return;
            }

            //if (!_stateRecords.ContainsKey(state.UniqueId))
            //{
            //    Log.Warn(name + " has no records.");
            //    return;
            //}

            //foreach (var record in _stateRecords[state.UniqueId])
            for (int i = 0; i < stateTransitionRecords.Count; i++)
            {
                var record = stateTransitionRecords[i];
                if (record.Conditions != null)
                {
                    //foreach (var condition in record.Conditions)
                    for (int k = 0; k < record.Conditions.Count; k++)
                    {
                        var condition = record.Conditions[k];
                        //var stateEntity = Entity.GetComponent<IStateEntity>();
                        //if (_spriteBaseComponent != null)
                        //{
                        //var rqObject = stateEntity.GetRQObject();
                        var conditionTest = condition.GetCondition();
                        if (conditionTest != null)
                            conditionTest.ConditionExit(this._stateMachine); //.SetEntity(rqObject, this);
                        //}
                    }
                }
            }

            //foreach (var record in _stateTransitionTable.Records.Where(i => i.CurrentState == (StateBase)state))
            //{
            //    if (record.Conditions != null)
            //    {
            //        foreach (var condition in record.Conditions)
            //        {
            //            //var stateEntity = Entity.GetComponent<IStateEntity>();
            //            //if (_spriteBaseComponent != null)
            //            //{
            //            //var rqObject = stateEntity.GetRQObject();
            //            if (condition.Condition != null)
            //                condition.Condition.Reset(); //.SetEntity(rqObject, this);
            //            //}
            //        }
            //    }
            //}
        }

        public void InitStateConditions(IState state)
        {
            if (_stateRecords == null)
                StartInit();
            //if (!_stateRecords.ContainsKey(state.UniqueId))
            if (!_stateRecords.TryGetValue(state.UniqueId, out var records))
            {
                return;
            }

            //foreach (var record in _stateRecords[state.UniqueId])
            //foreach (var record in records)
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (record.Conditions == null)
                    continue;
                //foreach (var condition in record.Conditions)
                for (int k = 0; k < record.Conditions.Count; k++)
                {
                    var condition = record.Conditions[k];
                    var conditionTest = condition.GetCondition();
                    if (conditionTest != null)
                    {
                        conditionTest.ConditionInit(this._stateMachine);
                    }
                }
            }
        }

        public void EnterStateConditions(IState state)
        {
            if (_stateRecords == null)
                StartInit();
            //if (!_stateRecords.ContainsKey(state.UniqueId))
            if (!_stateRecords.TryGetValue(state.UniqueId, out var records))
            {
                return;
            }

            //foreach (var record in _stateRecords[state.UniqueId])
            //foreach (var record in records)
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (record.Conditions == null)
                    continue;
                //foreach (var condition in record.Conditions)
                for (int k = 0; k < record.Conditions.Count; k++)
                {
                    var condition = record.Conditions[k];
                    var conditionTest = condition.GetCondition();
                    if (conditionTest != null)
                    {
                        conditionTest.ConditionReset(this._stateMachine);
                        conditionTest.ConditionEnter(this._stateMachine);
                    }
                }
            }
        }

        //public override bool HandleMessage(Telegram msg)
        //{
        //    if (base.HandleMessage(msg))
        //        return true;

        //    switch (msg.Msg)
        //    {
        //        case Enums.Telegrams.SetStateMachine:
        //            SetStateMachine((string) msg.ExtraInfo);
        //            break;
        //        case Enums.Telegrams.SetEntity:
        //            //_entity = EntityContainer._instance.GetEntity((string)msg.ExtraInfo).transform;
        //            _entity = (Transform)msg.ExtraInfo;
        //            break;
        //        //case Enums.Telegrams.Setup:
        //        //    //Debug.Log("STT Init beign called from State Machine");
        //        //    //Init();
        //        //    SetupConditions();
        //        //    break;
        //    }

        //    return false;
        //}

        //public void CustomSerialize(EntitySerializedData entitySerializedData)
        //{
        //    //base.Serialize(entitySerializedData);
        //    var StateTransitionDatas = new List<StateTransitionRecordData>();
        //    MessageDispatcher2.Instance.DispatchMsg("GetConfig", 0f, this.UniqueId, "Game Controller", null);
        //    foreach (var transitionRecord in Records)
        //    {
        //        var stateTransitionRecordData = new StateTransitionRecordData()
        //        {
        //            Active = transitionRecord.IsActive,
        //            Previous = transitionRecord.PreviousState,
        //            Name = transitionRecord.Name,
        //            ConditionType = transitionRecord.Type,
        //            ConditionExpressionDatas = new List<ConditionExpressionData>()
        //        };
        //        foreach (var conditionExpression in transitionRecord.Conditions)
        //        {
        //            if (string.IsNullOrEmpty(conditionExpression.UniqueId) || conditionExpression.Condition2 == null)
        //                continue;
        //            var data = new ConditionExpressionData();
        //            data.Condition2UniqueId = conditionExpression.Condition2.GetUniqueId();
        //            data.IsNot = conditionExpression.IsNot;
        //            stateTransitionRecordData.ConditionExpressionDatas.Add(data);
        //        }
        //        StateTransitionDatas.Add(stateTransitionRecordData);
        //    }
        //    entitySerializedData.ComponentData.Add(this.RQName2, StateTransitionDatas);
        //}

        //public void CustomDeserialize(EntitySerializedData entitySerializedData)
        //{
        //    //base.Deserialize(entitySerializedData);
        //    object StateTransitionDatasTemp;
        //    if (!entitySerializedData.ComponentData.TryGetValue(this.RQName2, out StateTransitionDatasTemp))
        //        return;
        //    var StateTransitionRecordDatas = Persistence.DeserializeObject<List<StateTransitionRecordData>>(StateTransitionDatasTemp);
            
        //    MessageDispatcher2.Instance.DispatchMsg("GetConfig", 0f, this.UniqueId, "Game Controller", null);

        //    //if (_configs == null)
        //    //{
        //    //    Debug.LogError("_configs is null in transition table in " + _stateMachine.GetEntity().name);
        //    //}

        //    foreach (var stateTransitionRecordData in StateTransitionRecordDatas)
        //    {
        //        // First find the transition record we are deserializing
        //        var stateTransitionRecord = Records.FirstOrDefault(i => i.Name == stateTransitionRecordData.Name);
        //        if (stateTransitionRecord == null)
        //            return;

        //        // If there are any Condition Expressions using Condition2, clear the Condition Expressions entries
        //        // Otherwise, skip this.
        //        //if (!stateTransitionRecord.Conditions.Any(i => i.Condition2 != null))
        //        if (!stateTransitionRecordData.ConditionExpressionDatas.Any(i => !String.IsNullOrEmpty(i.Condition2UniqueId)))
        //            continue;

        //        stateTransitionRecord.Conditions.Clear();

        //        stateTransitionRecord.IsActive = stateTransitionRecordData.Active;
        //        stateTransitionRecord.PreviousState = stateTransitionRecordData.Previous;
        //        stateTransitionRecord.Type = stateTransitionRecordData.ConditionType;

        //        foreach (var conditionExpressionData in stateTransitionRecordData.ConditionExpressionDatas)
        //        {
        //            var condition = new ConditionExpression();
        //            condition.Condition2 = ConfigsContainer.Instance.GetConfig(conditionExpressionData.Condition2UniqueId) as StateTransitionConditionBaseConfig;
        //            condition.IsNot = conditionExpressionData.IsNot;
        //            condition.UniqueId = Guid.NewGuid().ToString();
        //            stateTransitionRecord.Conditions.Add(condition);
        //        }
        //    }

        //    //foreach (var transitionRecord in Records)
        //    //{
        //    //    foreach (var conditionExpression in transitionRecord.Conditions)
        //    //    {
        //    //        if (string.IsNullOrEmpty(conditionExpression.UniqueId))
        //    //            continue;
        //    //        object tempSavedExpression;
        //    //        if (entitySerializedData.ComponentData.TryGetValue(conditionExpression.UniqueId, out tempSavedExpression))
        //    //        {
        //    //            var savedExpression = Persistence.DeserializeObject<ConditionExpressionData>(tempSavedExpression);
        //    //            conditionExpression.IsNot = savedExpression.IsNot;
        //    //            conditionExpression.Condition2 = _configs[savedExpression.Condition2UniqueId] as StateTransitionConditionBaseConfig;
        //    //        }
        //    //    }
        //    //}
        //}
    }
}