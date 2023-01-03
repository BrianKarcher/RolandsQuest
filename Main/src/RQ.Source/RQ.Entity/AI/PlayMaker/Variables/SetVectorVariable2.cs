using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.Entity.AtomAction;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Gets the value and stores it in a Vector Variable.")]
    public class SetVectorVariable2 : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Storage variable")]
        public FsmVector2 storeResult;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public SetVectorVariableAtom2 _atom;
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
