using RQ.Entity.AtomAction;
using RQ.Entity.Components;

namespace RQ.AI.Conditions
{
    public class IsStuckAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);

        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
