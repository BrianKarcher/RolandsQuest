using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Sets the Game Object variable in the FSM.")]
    public class SetGameObjectVariable2 : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Variable to store.")]
        public FsmGameObject Variable;

        public SetGameObjectVariableAtom2 _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Start(_entity);            
            DoGetValue();
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
            Variable.Value = _atom.GetGameObject();
        }
    }
}
