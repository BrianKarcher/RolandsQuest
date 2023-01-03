using RQ.AI.Action;
using RQ.AI.Atom.GameManager;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Quest/Set Status")]
    public class SetQuestStatus : ActionBase
    {
        public SetQuestStatusAtom _atom;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            _atom.Start(GetEntity());
        }
    }
}
