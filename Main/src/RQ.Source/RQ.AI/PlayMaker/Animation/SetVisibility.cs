using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Animation")]
    [PM.Tooltip("Sets whether the entity is visible.")]
    public class SetVisibility : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Entity to damage.")]
        //public FsmGameObject EntityToAffect;
        public SetVisibilityAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom.SetEntityToAffect(EntityToAffect.Value);
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
            DoGetValue();
        }

        void DoGetValue()
        {           
            _atom.OnUpdate();
        }
    }
}
