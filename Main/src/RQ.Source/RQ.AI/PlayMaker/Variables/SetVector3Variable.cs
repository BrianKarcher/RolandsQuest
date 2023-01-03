using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Variable")]
    [PM.Tooltip("Gets the value and stores it in a Vector3 Variable in the FSM.")]
    public class SetVector3Variable : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The variable to store into")]
        public FsmVector3 storeResult;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public SetVector3VariableAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
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
        }
    }
}
