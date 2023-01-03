using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.FSM.V2.States.Events
{
    [AddComponentMenu("RQ/States/State/Events/Trigger Usable")]
    public class TriggerUsableState : StateBase
    {
        public override void Enter()
        {
            base.Enter();
            var usableComponent = _componentRepository.Components.GetComponent<UsableComponent>();
            var mainCharacterUniqueId = EntityContainer._instance.GetMainCharacter().UniqueId;
            usableComponent.Trigger(mainCharacterUniqueId);
            //var usableContainer = GameDataController.Instance.Data.UsableContainer;
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
            //    usableContainer.CurrentUsableObject, Enums.Telegrams.UseUsable, _spriteBase.UniqueId);
            //MessageDispatcher2.Instance.DispatchMsg("UseUsable", 0f, this.UniqueId, 
            //    usableContainer.CurrentUsableObject, _spriteBase.UniqueId);
            //Complete();
        }

        //public override void Exit()
        //{
        //    base.Exit();
        //}
    }
}
