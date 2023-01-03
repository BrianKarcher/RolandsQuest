using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Input;
using RQ.Input.Data;
using RQ.Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.FSM.V3.Conditionals
{
    //[AddComponentMenu("RQ/States/Conditions/Button Pressed")]
    public class ButtonPressedConditionConfig : StateTransitionConditionBaseConfig
    {
        //private InputComponent _inputComponent;
        //private bool _isButtonPressed = false;
        //protected IRQEntity _entity;

        //public virtual void SetEntity(IRQObject entity)
        //{
        //    _entity = entity as ISprite;
        //}

        [SerializeField]
        private Button _button = Button.Primary;
        //private bool _isButtonPressed = false;
        private List<IStateMachine> _listeners = new List<IStateMachine>();
        //private bool _isListening = false;
        //void Awake()
        //{
        //    //if (_listeners == null)
        //    //    _listeners = new List<IStateMachine>();
        //    StartListening();
        //    //InputManager.Instance.AddEntity(this);
        //}

        void StartListening()
        {
            MessageDispatcher2.Instance.StartListening("HandleInput", this.UniqueId, (data) =>
                {
                    if (data.ExtraInfo != null)
                    {
                        var input = (RawInput)data.ExtraInfo;
                        //var isButtonPressed = GetIsConditionSatisfied(stateMachine);
                        // Keep it true if it is already true
                        bool newStatus;
                        //newStatus = isButtonPressed == true ? true : input.IsButtonPressed(_button);
                        newStatus = input.IsButtonPressed(_button);
                        //if (!input.IsButtonDown(_button))
                        //{
                        //    newStatus = false;
                        //}

                        if (newStatus)
                        {
                            int i = 1;
                        }
                        if (newStatus)
                        {
                            foreach (var listener in _listeners)
                            {
                                var entity = listener.GetComponentRepository();
                                // TODO Reorganize this with state machines
                                var inputComponent = entity.Components.GetComponent<InputComponent>();
                                if (inputComponent != null)
                                {
                                    // We do not handle the message if input is disabled
                                    bool exitFunction = false;
                                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                                        inputComponent.UniqueId, Enums.Telegrams.GetInputState, null,
                                        (enabled) =>
                                        {
                                            if (!(bool)enabled) { exitFunction = true; }
                                        });
                                    if (exitFunction)
                                        continue;
                                }
                                SetIsConditionSatisfied(listener, newStatus);
                            }
                        }
                        //if ()
                        //{
                        //    return true;
                        //}
                        //sprite.ProcessMovementInput(input);
                        //agent.ProcessButtonInput(input);
                        //var sprite = agent as ISprite;
                        //sprite.IsAnimationComplete = true;
                    }
                });
        }

        void OnDestroy()
        {
            StopListening();
            //InputManager.Instance.RemoveEntity(this);
        }

        void StopListening()
        {
            MessageDispatcher2.Instance.StopListening("HandleInput", this.UniqueId, -1);
        }
        
        //public void OnEnable()
        //{
        //    InputManager.Instance.AddEntity(this);
        //}

        //public void OnDisable()
        //{
        //    InputManager.Instance.RemoveEntity(this);
        //}

        //public override void SetEntity(Entity.Components.IComponentRepository entity, string stateMachineId, 
        //    StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);

        //    _inputComponent = entity.Components.GetComponent<InputComponent>();
        //}

        //public override void ConditionInit(IStateMachine stateMachine)
        //{
        //    base.ConditionInit(stateMachine);


        //}

        public override void ConditionEnter(IStateMachine stateMachine)
        {
            base.ConditionEnter(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            //entity.StartListening("HandleInput", this.UniqueId, (telegram) =>
            //{
                
            //});
            StartListening();
            //if (!_isListening)
            //{
                
            //    _isListening = true;
            //}
            if (!_listeners.Contains(stateMachine))
                _listeners.Add(stateMachine);
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            //entity.StopListening("HandleInput", this.UniqueId);
            if (_listeners.Contains(stateMachine))
                _listeners.Remove(stateMachine);
        }


        public override bool TestCondition(IStateMachine stateMachine)
        {
            return base.GetIsConditionSatisfied(stateMachine);


            //var input = InputManager.Instance.GetInput();
            //if (input.IsButtonPressed(_button))
            //{
            //    return true;
            //}

            //return false;
        }

        //public void LateUpdate()
        //{
        //    //_isButtonPressed = false;
        //}

        public bool HandleMessage(Telegram msg)
        {
            return false;
        }

        //public string GetName()
        //{
        //    return name;
        //}
        //public string GetTag()
        //{
        //    return string.Empty;
        //}
        //public int Id { get { return 0; } }

        //public bool IsActive { get { return true; } }

    }
}
