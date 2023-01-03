using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Sets the velocity.")]
    public class SetVelocity : FsmStateAction
    {
        [UIHint(UIHint.FsmVector2)]
        public FsmVector2 Velocity;

        public SetVelocityAtom _atom;
        private IComponentRepository _entity;
        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!Velocity.IsNone)
                _atom.SetVelocity(Velocity.Value);
            _atom.Start(_entity);
            Tick();
            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Tick();
        }

        private void Tick()
        {
            _atom.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
