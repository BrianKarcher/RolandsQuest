using System;
using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Input
{
    [AddComponentMenu("RQ/Components/Input")]
    //public class InputComponent : InputComponent<InputComponent>
    //{ }

    public class InputComponent : ComponentPersistence<InputComponent>
    {
        protected IMessagingObject _spriteBaseComponent;
        // TODO Make this Private when done testing
        public bool _inputEnabled;
        [SerializeField]
        private bool _alwaysOn = false;

        private long _setInputStateId, _enableInputId;
        private Action<Telegram2> _setInputStateDelegate, _enableInputDelegate;

        public override void Awake()
        {
            //base.SetSpriteAnimator(Animator);
            base.Awake();
            if (!Application.isPlaying)
                return;
            _spriteBaseComponent = _componentRepository.GetComponent<IComponentRepository>() as IMessagingObject;
            _setInputStateDelegate = (data) =>
            {
                var enable = (bool) data.ExtraInfo;
                if (enable)
                    TurnOnInput();
                else
                    TurnOffInput();
            };
            _enableInputDelegate = (data) =>
            {
                string enabled = (string) data.ExtraInfo;
                if (enabled == "1")
                    TurnOnInput();
                else
                    TurnOffInput();
            };

            //GetStateMachine().SetGlobalState(RolanV2GlobalState.Instance);
            //set up the state machine
            //_stateMachine = new StateMachine<RolanV2>(this);


            //_stateMachine.GetCurrentState().Enter(this);
            //EntityController._instance.AddEntity(this);
            // Set this entity to receive player input


            //_steering.TurnOn(SteeringBehaviorManager.behavior_type.seek);
        }

        //public override void Start()
        //{
        //    base.Start();
        //    if (!Application.isPlaying)
        //        return;
            
        //}

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            if (GetIsStarted())
            {
                if (_spriteBaseComponent != null)
                    TurnOnInput();
            }
            _enableInputId =
                MessageDispatcher2.Instance.StartListening("EnableInput", _componentRepository.UniqueId, _enableInputDelegate);
            //_componentRepository.StartListening("EnableInput", this.UniqueId, _enableInputDelegate);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;
            TurnOffInput();
            MessageDispatcher2.Instance.StopListening("EnableInput", _componentRepository.UniqueId, _enableInputId);
            //_componentRepository.StopListening("EnableInput", this.UniqueId);
        }

        public override void StartListening()
        {
            base.StartListening();
            _setInputStateId = MessageDispatcher2.Instance.StartListening("SetInputState", _componentRepository.UniqueId, _setInputStateDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetInputState", _componentRepository.UniqueId, _setInputStateId);
        }

        public override void Start()
        {
            base.Start();
            if (_spriteBaseComponent != null)
                TurnOnInput();
        }

        public void TurnOnInput()
        {
            _inputEnabled = true;
            //InputManager.Instance.AddEntity(_spriteBaseComponent);
        }

        public void TurnOffInput()
        {
            if (_alwaysOn)
                return;
            _inputEnabled = false;
            //InputManager.Instance.RemoveEntity(_spriteBaseComponent);
        }

        public bool IsInputEnabled()
        {
            return _inputEnabled;
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.GetInputState:
                    msg.Act(_inputEnabled);
                    break;
                case Telegrams.SetEnabled:
                    bool enabled = (bool)msg.ExtraInfo;
                    if (enabled)
                        TurnOnInput();
                    else
                        TurnOffInput();
                    break;
                case Telegrams.GetEnabled:
                    msg.Act(_inputEnabled);
                    break;
            }

            return false;
        }
    }
}
