using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Gets the value from the FSM.")]
    public class GetVectorVariable : FsmStateAction
    {
        //[RequiredField]
        [UIHint(UIHint.FsmVector2)]
        [PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmVector2 Variable;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public GetVectorVariableAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.Value = Variable.Value;
            _atom.Start(_entity);
            DoGetValue();
            if (!everyFrame)
            {
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
            _atom.OnUpdate();
        }
    }
}
