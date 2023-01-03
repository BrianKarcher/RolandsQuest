using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.AI.AtomAction.Variables;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Gets the value from the FSM.")]
    public class GetBoolVariable3 : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.FsmBool)]
        [PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmBool Variable;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmString VariableName;

        public GetBoolVariableAtom3 _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!VariableName.IsNone)
                _atom.SetVariableName(VariableName.Value);
            _atom.Value = Variable.Value;
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
        }
    }
}
