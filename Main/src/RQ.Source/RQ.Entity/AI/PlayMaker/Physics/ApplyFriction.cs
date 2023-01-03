using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Physics")]
    [PM.Tooltip("Apply friction to the entity.")]
    public class ApplyFriction : FsmStateAction
    {
        //[UIHint(UIHint.FsmVector2)]
        //[RequiredField]
        //[PM.Tooltip("Force")]
        //public FsmVector2 force;

        public ApplyFrictionAtom _atom;
        private IComponentRepository _entity;

        public override void OnPreprocess()
        {
            base.OnPreprocess();
            Fsm.HandleFixedUpdate = true;
        }

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            //Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            _atom.OnUpdate();
        }
    }
}
