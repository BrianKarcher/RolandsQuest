using System;
using RQ.Entity.Components;
using RQ.Messaging;
using UnityEngine;
using RQ.AI;
using RQ.Physics.Components;

namespace RQ.Entity.AtomAction.Action
{
    [Serializable]
    public class PlayerMoveAtom : AtomActionBase
    {
        private PlayerComponent _playerComponent;
        private PhysicsComponent _physicsComponent;
        private long _handleInputMessageId;
        public bool MoveOnEnter = true;
        public int Priority = 0;
        public bool MoveOnExit = false;
        public bool AlsoAffectFacingDirectionOnInput = true;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //Debug.LogError("PlayerMove Start");
            _playerComponent = entity.Components.GetComponent<PlayerComponent>();
            _playerComponent.SetMoveOnInput(MoveOnEnter, Priority, false);
            if (AlsoAffectFacingDirectionOnInput)
            {
                _playerComponent.SetFacingDirectionOnInput(MoveOnEnter);
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }

        public override void End()
        {
            base.End();
            //Debug.LogError("PlayerMove End");
            _physicsComponent = _entity.Components.GetComponent<PhysicsComponent>();
            //_physicsComponent.Stop();
            _playerComponent.SetMoveOnInput(MoveOnExit, Priority, true);
            if (AlsoAffectFacingDirectionOnInput)
            {
                _playerComponent.SetFacingDirectionOnInput(MoveOnExit);
            }
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            //Debug.Log("PlayerMoveAtom - StartListening");
            //_handleInputMessageId = MessageDispatcher2.Instance.StartListening("HandleInput", entity.UniqueId, (data) =>
            //{
            //    if (data.ExtraInfo == null)
            //        return;
            //    var input = (RawInput)data.ExtraInfo;
            //    //if (!_isSetup)
            //    //return;
            //    if (_playerComponent == null)
            //        Debug.LogError("PlayerComponent not found in " + entity.name + " in " + entity.UniqueId);
            //    if (!_playerComponent.IsInputEnabled())
            //        return;
            //    //_playerComponent.ProcessMovementInput(input);
            //    ////////if (input.IsButtonPressed(Button.Secondary) && !Utility.IsSelectedSkillAffordable())
            //    ////////{
            //    ////////    base.PlaySound(_invalidSkillUse);
            //    ////////}
            //    //agent.ProcessButtonInput(input);
            //    //var sprite = agent as ISprite;
            //    //sprite.IsAnimationComplete = true;
            //});
        }

        public override void StopListening(IComponentRepository entity)
        {
            //Debug.Log("PlayerMoveAtom - StopListening");
            base.StopListening(entity);
            //MessageDispatcher2.Instance.StopListening("HandleInput", entity.UniqueId, _handleInputMessageId);
        }
    }
}
