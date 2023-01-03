using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Stops the entity in its tracks.")]
    public class StopMoving : FsmStateAction
    {
        [PM.Tooltip("Repeat every frame.")]
        public bool everyFrame;

        public StopMovingAtom _stopMovingAtom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            //if (_entity.name.Contains("Rolan"))
            //{
            //    int i = 0;
            //}
            _stopMovingAtom.Start(_entity);
            if (!everyFrame)
                Finish();
            else
                _stopMovingAtom.StopPhysics(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _stopMovingAtom.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _stopMovingAtom.End();
        }
    }
}
