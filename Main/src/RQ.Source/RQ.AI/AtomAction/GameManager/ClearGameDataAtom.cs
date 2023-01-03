using RQ.Common.Controllers;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class ClearGameDataAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            GameDataController.Instance.DeleteAllGameData();
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
