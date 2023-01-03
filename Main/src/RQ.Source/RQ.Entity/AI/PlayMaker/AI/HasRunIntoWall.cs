using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace RQ.AI.PM_State_Machine
{
    [ActionCategory("RQ.AI")]
    [PM.Tooltip("Checks whether the entity has collided with environment.")]
    public class HasRunIntoWall : FsmStateAction
    {
        //[UIHint(UIHint.Layer)]
        //public FsmInt ObstacleLayer;

        public HasRunIntoWallAtom _atom;
        private IComponentRepository _entity;
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when entity has run into a wall.")]
        public FsmEvent RunIntoWallEvent;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //_atom._obstacleLayerMask = ObstacleLayer.Value;
            _atom.Start(_entity);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_atom.OnUpdate() == AtomActionResults.Success)
            {
                if (RunIntoWallEvent != null)
                    Fsm.Event(RunIntoWallEvent);
                Finish();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }
    }
}
