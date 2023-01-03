using RQ.Animation.BasicAction.Action;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Set Facing Direction")]
    public class SetFacingDirection : ActionBase
    {
        public bool everyFrame;
        public SetFacingDirectionAtom atom;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            atom.Start(GetEntity());
            if (!everyFrame)
                _isRunning = false;
        }

        public override void Update()
        {
            base.Update();
            if (_isRunning)
            {
                atom.OnUpdate();
            }
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            atom.End();
        }
    }
}
