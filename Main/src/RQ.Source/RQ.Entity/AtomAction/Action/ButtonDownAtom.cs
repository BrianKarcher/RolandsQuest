using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Input.Data;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    [Obsolete]
    public class ButtonDownAtom : AtomActionBase
    {
        [SerializeField]
        public Button _button = Button.Primary;
        private long _handleInputId;
        private bool _isDown;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            throw new Exception($"Using obsolete ButtonDownAtom in {entity.name}");
            _entity = entity;
            _isDown = false;
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _handleInputId = MessageDispatcher2.Instance.StartListening("HandleInput", entity.UniqueId, (data) =>
        //    {
        //        if (data.ExtraInfo == null)
        //            return;
        //        var input = (RawInput)data.ExtraInfo;
        //        _isDown = input.IsButtonDown(_button);
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    MessageDispatcher2.Instance.StopListening("HandleInput", entity.UniqueId, _handleInputId);
        //}

        public override AtomActionResults OnUpdate()
        {            
            //var input = InputManager.Instance.GetInput();
            //_isDown = input.IsButtonDown(_button);
            return _isDown ? AtomActionResults.Success : AtomActionResults.Failure;
        }
    }
}
