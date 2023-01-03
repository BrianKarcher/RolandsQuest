using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Entity.AtomAction.Action.Physics;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Keeps the position centered on the target.")]
    public class LockOnTarget : FsmStateAction
    {
        public LockOnTargetAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            _atom.OnLateUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
