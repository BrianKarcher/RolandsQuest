using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace RQ.AI.PM_State_Machine
{
    [ActionCategory("RQ.AI")]
    [PM.Tooltip("Gets the offset for the nearest attack point on the target.")]
    public class GetNearestAttackPointOffset : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Could not locate an attack point.")]
        public FsmEvent Failed;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Target Offset to be used in PursuitOffset.")]
        public FsmVector3 Offset;

        [UIHint(UIHint.Layer)]
        public int[] ObstacleLayers;
        //public List<FsmInt> ObstacleLayer;

        public GetNearestAttackPointOffsetAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _atom.SetFailedAction(() => {
                Fsm.Event(Failed);
            });
            //_atom.ObstacleLayer = ObstacleLayer.First().Value;
            _atom.ObstacleLayers = ObstacleLayers;
            _atom.Start(_entity);
            Offset.Value = _atom.Offset;
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
