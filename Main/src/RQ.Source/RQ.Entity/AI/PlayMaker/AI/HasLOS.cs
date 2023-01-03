using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;
using RQ.AI.AtomAction;

namespace RQ.AI.PM_State_Machine
{
    [ActionCategory("RQ.AI")]
    [PM.Tooltip("Checks for an unobstructed line between entity and target")]
    public class HasLOS : FsmStateAction
    {
        //[UIHint(UIHint.Layer)]
        //public FsmInt ObstacleLayer;
        [UIHint(UIHint.Layer)]
        [Tooltip("Layers to avoid.")]
        public FsmInt[] ObstacleLayer;

        public HasLOSAtom _atom;
        private IComponentRepository _entity;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when target has LOS.")]
        public FsmEvent HasLOSEvent;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when target has no LOS.")]
        public FsmEvent HasNoLOSEvent;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom._obstacleLayerMask = Helper.LayerArrayToLayerMask(ObstacleLayer, false);
            _atom.Start(_entity);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_atom.OnUpdate() == AtomActionResults.Success)
            {
                if (HasLOSEvent != null)
                    Fsm.Event(HasLOSEvent);
                //Finish();
            }
            else
            {
                if (HasNoLOSEvent != null)
                    Fsm.Event(HasNoLOSEvent);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
