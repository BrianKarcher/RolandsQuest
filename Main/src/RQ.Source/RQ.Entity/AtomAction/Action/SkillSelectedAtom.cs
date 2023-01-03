using RQ.AI;
using RQ.Common.Controllers;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Item;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SkillSelectedAtom : AtomActionBase
    {
        public ItemConfig _skill;

        public override AtomActionResults OnUpdate()
        {
            var currentSkillUniqueId = GameDataController.Instance.Data.SelectedSkill;
            return _skill.UniqueId == currentSkillUniqueId ? AtomActionResults.Success : AtomActionResults.Failure;
        }
    }
}
