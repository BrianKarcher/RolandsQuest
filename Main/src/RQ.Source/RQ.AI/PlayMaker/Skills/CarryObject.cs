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
    [PM.Tooltip("Carry object.")]
    public class CarryObject : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Object being lifted.")]
        public FsmGameObject LiftedObject;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when done.")]
        public FsmEvent FinishEvent;

        public CarryObjectAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            if (!LiftedObject.IsNone)
                _atom.SetLiftableObject(LiftedObject.Value);
            //_atom.UnsafeLayerMask = Helper.LayerArrayToLayerMask(UnsafeLayerMask, false);
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
                Fsm.Event(FinishEvent);
                Finish();
            }
        }

        AtomActionResults Tick()
        {
            return _atom.OnUpdate();
        }
    }
}
