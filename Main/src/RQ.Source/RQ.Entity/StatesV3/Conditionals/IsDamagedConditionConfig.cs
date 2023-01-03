using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using System;
using System.Collections.Generic;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Is Damaged")]
    public class IsDamagedConditionConfig : StateTransitionConditionBaseConfig
    {
        private Dictionary<string, long> _damagedIds = new Dictionary<string, long>();
        private Action<Telegram2> _damagedDelegate;

        //protected PhysicsComponent _physicsComponent;
        //protected DamageComponent _damageComponent;
        //protected DamageEntityInfo _damageInfo;

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
            //if (!base.TestCondition(stateMachine))
            //    return false;

            //if (_damageInfo == null)
            //    throw new Exception("No damage info in IsDamagedCondition");

            //return _damageInfo.IsDamaged;
            return GetIsConditionSatisfied(stateMachine);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            //_damageInfo.IsDamaged = false;
            var entity = stateMachine.GetComponentRepository();
            if (_damagedDelegate == null)
            {
                _damagedDelegate = (data) =>
                {
                    SetIsConditionSatisfied(stateMachine, true);
                    stateMachine.CalculateNextState();
                };
            }
            var id = MessageDispatcher2.Instance.StartListening("Damaged", entity.UniqueId, _damagedDelegate);
            _damagedIds[entity.UniqueId] = id;

            //entity.StartListening("Damaged", this.UniqueId, _damagedDelegate);
            //_damageInfo = null;
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            //entity.StopListening("Damaged", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("Damaged", entity.UniqueId, _damagedIds[entity.UniqueId]);
            _damagedIds.Remove(entity.UniqueId);
        }
    }
}
