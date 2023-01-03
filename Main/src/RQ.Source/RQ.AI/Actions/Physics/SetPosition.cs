using RQ.Animation.BasicAction.Action;
using RQ.Controller.Actions;
using UnityEngine;

namespace RQ.AI.Actions.Physics
{
    [AddComponentMenu("RQ/Action/Physics")]
    public class Damaged : ActionBase
    {
        public GameObject ManualReference;
        public SetPositionAtom _atom;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (ManualReference != null)
                _atom.ManualVector = ManualReference.transform.position;
            _atom.Start(GetEntity());
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            _atom.End();
        }
    }
}
