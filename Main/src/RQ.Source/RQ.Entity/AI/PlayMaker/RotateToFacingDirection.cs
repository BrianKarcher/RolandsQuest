using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Rotate entity to the facing direction")]
    public class RotateToFacingDirection : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public RotateToFacingDirectionAtom _rotateToFacingDirectionAtom;
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
            _rotateToFacingDirectionAtom.Start(_entity);
            Tick();
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
            Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _rotateToFacingDirectionAtom.End();
        }

        public override void OnUpdate()
        {
            //Tick();
        }

        private void Tick()
        {
            _rotateToFacingDirectionAtom.OnUpdate();
            //if (storeResult.IsNone) return;
            //_getSpeedAtom.OnUpdate();
            //storeResult.Value = _getSpeedAtom.Speed;
        }
    }
}
