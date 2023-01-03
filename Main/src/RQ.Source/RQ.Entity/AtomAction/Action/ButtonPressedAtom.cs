using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Input.Data;
using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public class ButtonPressedAtom : AtomActionBase
    {
        [SerializeField]
        public Button _button = Button.Primary;
        private long _handleInputId;
        //private PhysicsComponent _physicsComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity;
        }

        public override void StartListening(IComponentRepository entity)
        {
            _handleInputId = MessageDispatcher2.Instance.StartListening("HandleInput", entity.UniqueId, (data) =>
            {
                if (data.ExtraInfo == null)
                    return;
                //if (!_isRunning)
                //    return;
                //if (_inputComponent != null)
                //{
                //    // We do not handle the message if input is disabled
                //    bool exitFunction = false;
                //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                //        _inputComponent.UniqueId, Enums.Telegrams.GetInputState, null,
                //        (enabled) =>
                //        {
                //            if (!(bool)enabled) { exitFunction = true; }
                //        });
                //    if (exitFunction)
                //        return;
                //}
                var input = (RawInput)data.ExtraInfo;
                //var _isButtonPressed = GetIsConditionSatisfied();
                var _isButtonPressed = !_isRunning;
                // Keep it true if it is already true
                bool newStatus;
                newStatus = _isButtonPressed == true ? true : input.IsButtonPressed(_button);
                //newStatus = input.IsButtonPressed(_button);
                if (!input.IsButtonDown(_button))
                {
                    newStatus = false;
                }

                if (newStatus)
                {
                    int i = 1;
                }

                //SetIsConditionSatisfied(newStatus);
                _isRunning = !newStatus;
            });
        }

        public override void StopListening(IComponentRepository entity)
        {
            MessageDispatcher2.Instance.StopListening("HandleInput", entity.UniqueId, _handleInputId);
        }

        //public override void End()
        //{
        //    base.End();
        //}

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
            //return AtomActionResults.Success;
        }
    }
}
