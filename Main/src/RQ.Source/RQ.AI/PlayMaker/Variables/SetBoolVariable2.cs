using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Gets the value and stores it in a Bool Variable.")]
    public class SetBoolVariable2 : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The variable to store into")]
        public FsmBool storeResult;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        private IComponentRepository _entity;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when true.")]
        public FsmEvent TrueEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when false.")]
        public FsmEvent FalseEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Variable name")]
        public FsmString VariableName;

        public SetBoolVariableAtom2 _atom;

        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Fire when success.")]
        //public FsmEvent HasNoLOSEvent;

        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("Fire when ")]
        //public FsmEvent HasNoLOSEvent;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!VariableName.IsNone)
                _atom.SetVariableName(VariableName.Value);
            _atom.Start(_entity);
            DoGetValue();
            if (!everyFrame)
            {
                //DoGetValue();
                Finish();
            }
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
            if (storeResult.IsNone) return;
            _atom.OnUpdate();
            storeResult.Value = _atom.Value;
            if (_atom.Value)
            {
                Fsm.Event(TrueEvent);
            }
            else
            {
                Fsm.Event(FalseEvent);
            }
        }
    }
}
