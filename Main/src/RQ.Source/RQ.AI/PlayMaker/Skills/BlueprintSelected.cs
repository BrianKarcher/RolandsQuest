using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.AI;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Skill")]
    [PM.Tooltip("Compare current blueprint to selected.")]
    public class BlueprintSelected : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when compare is a success.")]
        public FsmEvent Success;

        public BlueprintSelectedAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            if (_atom.GetCanUse())
                Fsm.Event(Success);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            if (Tick() != AtomActionResults.Running)
            {
                Finish();
            }
        }

        AtomActionResults Tick()
        {
            return _atom.OnUpdate();
        }
    }
}
