using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class RunActionSequenceAtom : AtomActionBase
    {
        public string ActionSequence;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            GameController.Instance.ActionController.CheckAndRunActionSequences(ActionSequence);
            //MessageDispatcher2.Instance.DispatchMsg("CheckAndRunActionSequences", 0f, string.Empty, null, ActionSequence);
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
