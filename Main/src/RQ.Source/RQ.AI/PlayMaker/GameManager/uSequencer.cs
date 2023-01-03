using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Atom.GameManager;

namespace RQ.AI.PlayMaker
{
    [ActionCategory("RQ.GameManager")]
    [PM.Tooltip("Manages the cutscene sequencer.")]
    public class uSequencer : FsmStateAction
    {
        public uSequencerAtom _atom;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when all sequences are complete.")]
        public FsmEvent AllComplete;

        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
        }

        public override void OnUpdate()
        {
            Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        void Tick()
        {
            var result = _atom.OnUpdate();
            if (result == RQ.AI.AtomActionResults.Success)
            {
                Fsm.Event(AllComplete);
                Finish();
            }
        }
    }
}
