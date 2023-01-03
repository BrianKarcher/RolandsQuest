using RQ.Common.Config;
using RQ.Entity.Components;
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
    [Serializable]
    public abstract class StateTransitionConditionBaseConfig : RQBaseConfig, IStateTransitionCondition
    {
        //[UniqueIdentifier]
        //public string ID;

        //private IComponentRepository _entity;
        //private IStateMachine _stateMachine;
        //protected string _stateMachineId;
        //[SerializeField]
        //private string _name;
        //protected StateInfo _stateInfo;
        //protected string _spriteBaseId;


        //public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo,
        //    params TelegramEarlyTermination[] earlyTermination)
        //{
        //    MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _spriteBaseId,
        //        msg, extraInfo, null, earlyTermination);
        //}

        //[MenuItem("Assets/Create/RQ/Scene Config")]
        //public static void CreateNewAsset()
        //{
        //    SceneConfig sceneData = ScriptableObject.CreateInstance<SceneConfig>();
        //    AssetDatabase.CreateAsset(sceneData, "Assets/Areas/NewScene.asset");
        //    EditorUtility.FocusProjectWindow();
        //    Selection.activeObject = sceneData;
        //}




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

        public virtual void SetEntity(IComponentRepository entity, string stateMachineId)
        {
            //_entity = entity;
            //_stateMachine = stateMachine;
            //_stateMachineId = stateMachine.UniqueId;
            //_stateMachineId = stateMachineId;
            //_stateMachine = stateMachine;
            //var spriteBase = entity.GetComponent<IComponentRepository>();
            //_spriteBaseId = entity.UniqueId;
            //_stateInfo = stateMachine.GetStateInfo();
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _stateMachineId,
            //    Telegrams.RequestStateInfo, null);
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

        public abstract bool TestCondition(IStateMachine stateMachine);

        public virtual void ConditionEnter(IStateMachine stateMachine) { }
        public virtual void ConditionExit(IStateMachine stateMachine) { }
        public virtual void ConditionInit(IStateMachine stateMachine) { }

        
        //{
        //    //if (_entity == null)
        //    //    throw new Exception("Entity is null in condition test");
        //    return true;
        //}

        public virtual void ConditionReset(IStateMachine stateMachine)
        {
            SetIsConditionSatisfied(stateMachine, false);
        }

        public virtual void SetIsConditionSatisfied(IStateMachine stateMachine, bool satisfied)
        {
            var stateInfo = stateMachine.GetStateInfo();
            if (stateInfo == null)
            {
                //int i = 1;
                throw new Exception("StateInfo not located in " + this.UniqueId + " " + this.name);
            }
            if (!stateInfo.IsConditionSatisfied.ContainsKey(this.UniqueId))
                stateInfo.IsConditionSatisfied.Add(this.UniqueId, satisfied);
            else
                stateInfo.IsConditionSatisfied[this.UniqueId] = satisfied;
        }

        public virtual bool GetIsConditionSatisfied(IStateMachine stateMachine)
        {
            var stateInfo = stateMachine.GetStateInfo();
            if (stateInfo == null)
            {
                int i = 1;
                throw new Exception("StateInfo not located in " + this.UniqueId + " " + this.name);
            }
            if (!stateInfo.IsConditionSatisfied.ContainsKey(this.UniqueId))
                return false;
            else
                return stateInfo.IsConditionSatisfied[this.UniqueId];
        }
    }
}
