using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class StopSequenceAtom : AtomActionBase
    {
        //public string SequenceName;
        //public bool PlayDirectly;
        //public SequencerLink _sequencerLink;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var sequence = GameController.Instance.UIManager.CurrentSequence;
            if (sequence == null)
                return;
            sequence.Stop();
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
