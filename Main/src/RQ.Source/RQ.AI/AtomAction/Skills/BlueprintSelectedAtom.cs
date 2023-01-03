using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using RQ.Model.Item;
using RQ.Common.Controllers;
using RQ.Entity.Skill;

namespace RQ.AI.Action
{
    [Serializable]
    public class BlueprintSelectedAtom : AtomActionBase
    {
        public ItemConfig Blueprint;
        private bool _canUse;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);

            if (GameDataController.Instance.CurrentBlueprint.GetUniqueId() == Blueprint.UniqueId)
                _canUse = true;
            else
                _canUse = false;

        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        public bool GetCanUse()
        {
            return _canUse;
        }
    }
}
