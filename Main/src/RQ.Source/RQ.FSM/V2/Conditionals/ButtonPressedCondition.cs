using RQ.Input;
using RQ.Input.Data;
using RQ.Messaging;
using UnityEngine;

namespace RQ.FSM.V2.Conditionals
{
    [AddComponentMenu("RQ/States/Conditions/Button Pressed")]
    public class ButtonPressedCondition : StateTransitionConditionBase, IMessagingObject
    {
        private InputComponent _inputComponent;
        //private bool _isButtonPressed = false;
        //protected IRQEntity _entity;

        //public virtual void SetEntity(IRQObject entity)
        //{
        //    _entity = entity as ISprite;
        //}

        [SerializeField]
        private Button _button = Button.Primary;

        //public void OnEnable()
        //{
        //    InputManager.Instance.AddEntity(this);
        //}

        //public void OnDisable()
        //{
        //    InputManager.Instance.RemoveEntity(this);
        //}

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            //InputManager.Instance.AddEntity(this);
            MessageDispatcher2.Instance.StartListening("HandleInput", this.UniqueId, (data) =>
                {
                    if (data.ExtraInfo == null)
                        return;
                    if (_inputComponent != null)
                    {
                        // We do not handle the message if input is disabled
                        bool exitFunction = false;
                        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                            _inputComponent.UniqueId, Enums.Telegrams.GetInputState, null,
                            (enabled) =>
                            {
                                if (!(bool)enabled) { exitFunction = true; }
                            });
                        if (exitFunction)
                            return;
                    }
                    var input = (RawInput)data.ExtraInfo;
                    var _isButtonPressed = GetIsConditionSatisfied();
                    // Keep it true if it is already true
                    bool newStatus;
                    newStatus = _isButtonPressed == true ? true : input.IsButtonPressed(_button);
                    if (!input.IsButtonDown(_button))
                    {
                        newStatus = false;
                    }

                    if (newStatus)
                    {
                        int i = 1;
                    }

                    SetIsConditionSatisfied(newStatus);
                });
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            //InputManager.Instance.RemoveEntity(this);
            MessageDispatcher2.Instance.StopListening("HandleInput", this.UniqueId, -1);
        }

        public override void SetEntity(Entity.Components.IComponentRepository entity, string stateMachineId, 
            StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);

            _inputComponent = entity.Components.GetComponent<InputComponent>();
        }


        public override bool TestCondition(IStateMachine stateMachine)
        {
            if (!base.TestCondition(stateMachine))
                return false;

            return base.GetIsConditionSatisfied();
        }

        public void LateUpdate()
        {
            //_isButtonPressed = false;
        }
    }
}
