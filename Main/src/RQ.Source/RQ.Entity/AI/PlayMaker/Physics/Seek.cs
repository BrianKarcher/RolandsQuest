using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using System;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Seek towards the target.")]
    [Obsolete]
    public class Seek : FsmStateAction
    {
        public SeekAtom _seekAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _seekAtom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _seekAtom.End();
        }
    }
}
