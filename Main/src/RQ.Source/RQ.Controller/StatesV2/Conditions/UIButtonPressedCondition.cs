using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [AddComponentMenu("RQ/States/Conditions/UI Button Pressed")]
    public class UIButtonPressedCondition : StateTransitionConditionBase//, IMessageHandler
    {
        // TODO: Take this out when done testing
        [SerializeField]
        private bool _isButtonPressed = false;
        //protected IRQEntity _entity;

        //public virtual void SetEntity(IRQObject entity)
        //{
        //    _entity = entity as ISprite;
        //}

        //[SerializeField]
        //private Button _button;

        //public void OnEnable()
        //{
        //    InputManager.Instance.AddEntity(this);
        //}

        //public void OnDisable()
        //{
        //    InputManager.Instance.RemoveEntity(this);
        //}

        public void ButtonPressed()
        {
            _isButtonPressed = true;
        }


        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            return _isButtonPressed;
        }

        //public void LateUpdate()
        //{
        //    //_isButtonPressed = false;
        //}

        //public override bool HandleMessage(Telegram telegram)
        //{
        //    //var data = agent as ISprite;
        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.HandleInput:
        //            if (telegram.ExtraInfo != null)
        //            {
        //                var input = (RawInput)telegram.ExtraInfo;
        //                // Keep it true if it is already true
        //                _isButtonPressed = _isButtonPressed == true ? true : input.IsButtonPressed(_button);
        //                if (!input.IsButtonDown(_button))
        //                {
        //                    _isButtonPressed = false;
        //                }
        //                //if ()
        //                //{
        //                //    return true;
        //                //}
        //                //sprite.ProcessMovementInput(input);
        //                //agent.ProcessButtonInput(input);
        //                //var sprite = agent as ISprite;
        //                //sprite.IsAnimationComplete = true;
        //            }
        //            // TODO FIX THIS - The state transition table should be determing the next state
        //            //data.GetFSM().RevertToPreviousState();
        //            return true;
        //    }
        //    return false;
        //}

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            _isButtonPressed = false;
        }
    }
}
