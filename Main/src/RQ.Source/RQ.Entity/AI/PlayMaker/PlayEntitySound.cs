using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Entity.AtomAction.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    //[ActionCategory("RQ")]
    [PM.Tooltip("Plays an Entity Sound")]
    public class PlayEntitySound : FsmStateAction
    {
        public PlayEntitySoundAtom _soundAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _soundAtom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _soundAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            _soundAtom.OnUpdate();
        }
    }
}
