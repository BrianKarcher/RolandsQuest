using RQ.AI.Conditions;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using UnityEngine;
using RQ.AI;
using System;
using RQ.Messaging;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Is Damaged")]
    public class IsDamagedCondition : StateTransitionConditionBase
    {
        private IsDamagedConditionAI _isDamagedConditionAI;
        private long _damagedId;
        private Action<Telegram2> _damagedDelegate;        

        public override void Start()
        {
            base.Start();
            _damagedDelegate = (data) =>
            {
                //Debug.LogError("Damaged, switching state");
                _stateMachine.CalculateNextState();
            };
        }

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    //_physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
        //    _damageComponent = entity.Components.GetComponent<DamageComponent>();
        //    if (_damageComponent == null)
        //        throw new Exception("Damage Component is required for condition IsDammaged in entity " + entity.GetName());
        //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId, 
        //        Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            
            if (!base.TestCondition(stateMachine))
                return false;


            //return false;

            //bool isDamaged;

            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId, 
            //    Enums.Telegrams.IsDamaged, ref isDamaged);

            //return _physicsComponent.GetPhysicsData().IsDamaged;
            //return _damageInfo.IsDamaged;
            return _isDamagedConditionAI.OnUpdate() == AtomActionResults.Success;
        }

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            _isDamagedConditionAI = new IsDamagedConditionAI();
            _isDamagedConditionAI.Start(entity);
            //_damageInfo.IsDamaged = false;
            //_damageInfo = null;
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            _damagedId = MessageDispatcher2.Instance.StartListening("Damaged", entity.UniqueId, _damagedDelegate);
            //entity.StartListening("Damaged", this.UniqueId, _damagedDelegate);
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            MessageDispatcher2.Instance.StopListening("Damaged", entity.UniqueId, _damagedId);
            //entity.StopListening("Damaged", this.UniqueId);
        }
    }
}
