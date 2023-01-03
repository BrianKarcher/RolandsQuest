using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI.AtomAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace RQ.AI.PlayMaker.Audio
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Plays an Entity Sound")]
    public class PlayEntitySound2 : FsmStateAction
    {
        public PlayEntitySound2Atom _atom;
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

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            _atom.OnUpdate();
        }
    }
}
