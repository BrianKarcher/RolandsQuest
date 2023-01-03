using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Gets the value and stores it in a Float Variable.")]
    public class SetFloatVariable2 : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        public FsmFloat storeResult;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public SetFloatVariableAtom2 _getFloatVariableAtom;
        private IComponentRepository _entity;

        public override void Reset()
        {
            //gameObject = null;
            storeResult = null;
            everyFrame = false;
        }

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _getFloatVariableAtom.Start(_entity);
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
            _getFloatVariableAtom.End();
        }

        public override void OnUpdate()
        {
            DoGetValue();
        }

        void DoGetValue()
        {
            if (storeResult.IsNone) return;
            _getFloatVariableAtom.OnUpdate();
            storeResult.Value = _getFloatVariableAtom.Value;
        }
    }
}
