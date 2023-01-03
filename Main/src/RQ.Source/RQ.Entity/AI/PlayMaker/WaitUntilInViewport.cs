using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Finished when the entity is in the viewport of the main camera.")]
    public class WaitUntilInViewport : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public WaitUntilInViewportAtom _waitUnitlInViewportAtom;
        private IComponentRepository _entity;

        //public override void Reset()
        //{
        //    //gameObject = null;
        //    storeResult = null;
        //    everyFrame = false;
        //}

        public override void OnEnter()
        {
            var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            _entity = rqSM.GetComponentRepository();
            _waitUnitlInViewportAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _waitUnitlInViewportAtom.End();
        }

        public override void OnUpdate()
        {
            if (_waitUnitlInViewportAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
            {
                Finish();
            }
            //DoGetSpeed();
        }

        //void DoGetSpeed()
        //{
        //    if (storeResult.IsNone) return;
        //    _getSpeedAtom.OnUpdate();
        //    storeResult.Value = _getSpeedAtom.Speed;
        //}
    }
}
