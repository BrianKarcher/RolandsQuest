using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.AtomAction;
using RQ.AI.Action;
using RQ.AI;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ.Skill")]
    [PM.Tooltip("Carry object Follow Parent.")]
    public class CarryObjectFollowParent : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Object being lifted.")]
        public FsmGameObject ParentObject;

        public CarryObjectFollowParentAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!ParentObject.IsNone)
                _atom.SetParentObject(ParentObject.Value);
            _atom.Start(_entity);
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            if (Tick() != AtomActionResults.Running)
            {
                Finish();
            }
        }

        AtomActionResults Tick()
        {
            return _atom.OnUpdate();
        }
    }
}
