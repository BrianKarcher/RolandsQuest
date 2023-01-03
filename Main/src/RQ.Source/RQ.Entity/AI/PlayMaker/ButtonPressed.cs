using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using RQ.AI.Action;

namespace Assets.Source.AI.PM_State_Machine
{
    [ActionCategory("RQ")]
    [PM.Tooltip("Fires the Event if the button has been clicked (previous frame it was up, now it is down).")]
    public class ButtonPressed : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when button is pressed.")]
        public FsmEvent Pressed;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("The variable to store into")]
        public FsmBool storeResult;

        //[PM.Tooltip("Repeat every frame.")]
        //public bool everyFrame;

        public ButtonPressedAtom _buttonPressedAtom;
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
            _buttonPressedAtom.Start(_entity);
            //DoGetSpeed();

            //if (!everyFrame)
            //{
            //    Finish();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
            _buttonPressedAtom.End();
        }

        public override void OnUpdate()
        {
            if (_buttonPressedAtom.OnUpdate() == RQ.AI.AtomActionResults.Success)
            {
                Fsm.Event(Pressed);
                if (!storeResult.IsNone)
                    storeResult.Value = true;
            }
            else
            {
                if (!storeResult.IsNone)
                    storeResult.Value = false;
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
