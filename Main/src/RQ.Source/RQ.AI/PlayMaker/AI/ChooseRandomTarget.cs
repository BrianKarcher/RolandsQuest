using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI.AtomAction.AI;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.AI")]
    [PM.Tooltip("Gets a random target.")]
    public class ChooseRandomTarget : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Variable to store.")]
        public FsmGameObject storeResult;

        public ChooseRandomTargetAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);
            storeResult.Value = _atom.GetTarget().gameObject;
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        //public override void OnUpdate()
        //{
        //    DoGetValue();
        //}

        //void DoGetValue()
        //{           
        //    _atom.OnUpdate();
        //}
    }
}
