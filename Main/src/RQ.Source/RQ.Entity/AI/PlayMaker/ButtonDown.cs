using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Fires the Event if the button is currently down, no matter how long it has been down.")]
    public class ButtonDown : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Store boolean button status.")]
        public FsmBool Down;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is down.")]
        public FsmEvent DownEvent;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is up.")]
        public FsmEvent UpEvent;

        public bool FinishWhenDown;
        public bool FinishWhenUp;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public ButtonDownAtom _atom;
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
            _atom.Start(_entity);
            Tick();
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _atom.End();
        }

        public override void OnUpdate()
        {
            Tick();
        }

        public void Tick()
        {
            // Down = success
            var result = _atom.OnUpdate() == RQ.AI.AtomActionResults.Success;
            Down.Value = result;
            if (result)
            {
                Fsm.Event(DownEvent);
                if (FinishWhenDown)
                    Finish();
            }
            else
            {
                Fsm.Event(UpEvent);
                if (FinishWhenUp)
                    Finish();
            }
        }
    }
}
