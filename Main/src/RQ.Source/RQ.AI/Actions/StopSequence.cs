using RQ.AI.Atom.GameManager;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Stop Sequence")]
    public class StopSequence : ActionBase
    {
        public StopSequenceAtom _atom;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _atom.Start(GetEntity());
        }
    }
}
