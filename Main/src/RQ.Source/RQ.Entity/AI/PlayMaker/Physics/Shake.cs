using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.Model;
using RQ.Common.Container;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Shake effect. Useful for cameras.")]
    public class Shake : FsmStateAction
    {
        [UIHint(UIHint.FsmVector2)]
        [RequiredField]
        [PM.Tooltip("Force")]
        public FsmVector2 force;

        public ShakeAtom _atom;
        private IComponentRepository _entity;

        public override void OnPreprocess()
        {
            base.OnPreprocess();
            Fsm.HandleLateUpdate = true;
        }

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _atom.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            _atom.OnLateUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
