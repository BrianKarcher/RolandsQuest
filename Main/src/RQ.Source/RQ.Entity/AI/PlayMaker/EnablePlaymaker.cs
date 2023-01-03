using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Enables the Playmaker component on enter. Disables on exit.")]
    public class EnablePlaymaker : FsmStateAction
    {
        public EnablePlaymakerAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (_atom != null)
                _atom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (_atom != null)
                _atom.End();
        }
    }
}
