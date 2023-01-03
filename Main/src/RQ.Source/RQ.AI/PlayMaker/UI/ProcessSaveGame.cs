using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.AI.Atom.UI;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.UI")]
    [PM.Tooltip("Save the game to the designated file.")]
    public class ProcessSaveGame : FsmStateAction
    {
        public ProcessSaveGameAtom _atom;
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
            base.OnUpdate();
            var result = _atom.OnUpdate();
        }
    }
}
