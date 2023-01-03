using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Determines whether a skill is selected.")]
    public class SkillSelected : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when message is received.")]
        public FsmEvent Selected;

        public SkillSelectedAtom _skillSelectedAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _skillSelectedAtom.Start(_entity);
            if (_skillSelectedAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
            {
                Fsm.Event(Selected);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _skillSelectedAtom.End();
        }
    }
}
