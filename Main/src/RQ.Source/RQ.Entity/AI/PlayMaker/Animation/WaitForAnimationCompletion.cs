using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Animation")]
    [PM.Tooltip("Waits for the animation by the given name to complete, then Finishes.")]
    public class WaitForAnimationCompletion : FsmStateAction
    {
        public WaitForAnimationCompletionAtom _animCompleteAtom;
        private IComponentRepository _entity;

        [UIHint(UIHint.FsmEvent)]
        [PM.Tooltip("Event to call when the wait finishes.")]
        public FsmEvent FinishEvent;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _animCompleteAtom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _animCompleteAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            if (_animCompleteAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
            {
                Finish();
                if (FinishEvent != null)
                    Fsm.Event(FinishEvent);
            }
        }
    }
}
