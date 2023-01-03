using RQ.Common.Controllers;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Model.Item;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Is Complete")]
    public class SkillSelectedConditionConfig : StateTransitionConditionBaseConfig
    {
        [SerializeField]
        private ItemConfig _skill;

        public override bool TestCondition(IStateMachine stateMachine)
        {
            var currentSkillUniqueId = GameDataController.Instance.Data.SelectedSkill;
            return _skill.UniqueId == currentSkillUniqueId;
            //return stateMachine.GetStateInfo().IsComplete;
        }
    }
}
