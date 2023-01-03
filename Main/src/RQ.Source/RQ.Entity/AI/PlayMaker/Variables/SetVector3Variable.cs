using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Returns Success if Button is pressed.")]
    public class GetVector3Variable : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is pressed.")]
        public FsmVector3 Variable;

        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public GetVector3VariableAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();            
            _atom.Start(_entity);
            Tick();
            if (!everyFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        private void Tick()
        {
            _atom.Variable = Variable.Value;
            _atom.OnUpdate();
        }
    }
}
