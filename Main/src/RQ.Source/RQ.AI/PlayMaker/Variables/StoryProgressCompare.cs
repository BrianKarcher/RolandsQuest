using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Compare the story progress.")]
    public class StoryProgressCompare : FsmStateAction
    {
        public StoryProgressCompareAtom _atom;
        private IComponentRepository _entity;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when true.")]
        public FsmEvent TrueEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when false.")]
        public FsmEvent FalseEvent;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            Tick();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();            
            Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public void Tick()
        {
            _atom.OnUpdate();
            if (!_atom.IsRunning())
            {
                Fsm.Event(TrueEvent);
                Finish();
            }
            else
                Fsm.Event(FalseEvent);

        }
    }
}
