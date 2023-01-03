using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Sets the story progress.")]
    public class SetStoryProgress : FsmStateAction
    {
        public SetStoryProgressAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
