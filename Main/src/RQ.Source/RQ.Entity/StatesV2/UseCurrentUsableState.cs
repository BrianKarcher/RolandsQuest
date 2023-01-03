using RQ.Common.Controllers;
using RQ.Controller.Contianers;
using RQ.Messaging;
using UnityEngine;

namespace RQ.FSM.V2.States
{
    [AddComponentMenu("RQ/States/State/Use Current Usable")]
    public class UseCurrentUsableState : StateBase
    {
        public override void Enter()
        {
            base.Enter();
            var usableContainer = UsableContainerController.Instance.UsableContainer;
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
            //    usableContainer.CurrentUsableObject, Enums.Telegrams.UseUsable, _spriteBase.UniqueId);
            MessageDispatcher2.Instance.DispatchMsg("UseUsable", 0f, this.UniqueId, 
                usableContainer.CurrentUsableObject, _spriteBase.UniqueId);
            Complete();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
