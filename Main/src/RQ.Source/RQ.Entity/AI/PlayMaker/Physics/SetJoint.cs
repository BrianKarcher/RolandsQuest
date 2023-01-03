using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Sets the joint of the target.")]
    public class SetJoint : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        public FsmGameObject AddJointTo;

        [UIHint(UIHint.Variable)]
        public FsmGameObject ConnectTo;

        public SetJointAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.SetAddJointTo(AddJointTo.Value);
            _atom.SetConnectedTo(ConnectTo.Value);
            _atom.Start(_entity);
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
