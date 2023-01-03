using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace RQ.AI.PM_State_Machine
{
    [ActionCategory("RQ.AI")]
    [PM.Tooltip("Checks whether the target is in the FOV.")]
    public class TargetInFOV : FsmStateAction
    {
        //[UIHint(UIHint.Layer)]
        //public FsmInt ObstacleLayer;

        public TargetInFOVAtom _atom;
        private IComponentRepository _entity;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when target is in FOV.")]
        public FsmEvent InFOVEvent;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The bool value to store")]
        public FsmBool storeResult;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom._obstacleLayerMask = ObstacleLayer.Value;
            _atom.Start(_entity);
            Tick();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Tick();
        }

        private void Tick()
        {
            bool isTrue = false;
            if (_atom.OnUpdate() == AtomActionResults.Success)
            {
                isTrue = true;
                if (InFOVEvent != null)
                    Fsm.Event(InFOVEvent);
                //Finish();
            }
            else
                isTrue = false;
            if (!storeResult.IsNone)
            {
                storeResult.Value = isTrue;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
