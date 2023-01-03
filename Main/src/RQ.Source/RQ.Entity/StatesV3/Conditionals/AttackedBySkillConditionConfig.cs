using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model.Item;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Is Complete")]
    public class AttackedBySkillConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private ItemConfig _skill;
        private Dictionary<string, long> _damagedIds = new Dictionary<string, long>();
        private Action<Telegram2> _damagedDelegate;

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            //_damageInfo.IsDamaged = false;
            var entity = stateMachine.GetComponentRepository();
            if (_damagedDelegate == null)
            {
                _damagedDelegate = (data) =>
                {
                    var damageData = (DamageEntityInfo)data.ExtraInfo;
                    if (damageData.SkillUsed == _skill.UniqueId)
                    {
                        SetIsConditionSatisfied(stateMachine, true);
                        stateMachine.CalculateNextState();
                    }
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
            MessageDispatcher2.Instance.StopListening("Damaged", entity.UniqueId, _damagedIds[entity.UniqueId]);
            //entity.StopListening("Damaged", this.UniqueId);
            _damagedIds.Remove(entity.UniqueId);
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            //return stateMachine.iscomp
            //var currentSkillUniqueId = GameDataController.Instance.Data.SelectedSkill;
            //return _skill.UniqueId == currentSkillUniqueId;
            return GetIsConditionSatisfied(stateMachine);
            //return stateMachine.GetStateInfo().IsComplete;
        }
    }
}
