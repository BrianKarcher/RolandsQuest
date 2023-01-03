using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Sets the maximum speed for the entity.")]
    public class DragMaxSpeed : FsmStateAction
    {
        //[RequiredField]
        //[UIHint(UIHint.Variable)]
        //[PM.Tooltip("The speed, or in technical terms: velocity magnitude")]
        //public FsmFloat storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public DragMaxSpeedAtom _dragMaxSpeedAtom;
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
            _dragMaxSpeedAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
            //Finish();
        }

        public override void OnExit()
        {
            base.OnExit();
            _dragMaxSpeedAtom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        private void Tick()
        {
            if (Finished)
                return;
            //if (storeResult.IsNone) return;
            if (_dragMaxSpeedAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
                Finish();            
        }
    }
}
