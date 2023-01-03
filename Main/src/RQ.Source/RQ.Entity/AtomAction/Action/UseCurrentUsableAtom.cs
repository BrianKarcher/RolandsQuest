using RQ.AI;
using RQ.Common.Controllers;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using RQ.Controller.Contianers;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class UseCurrentUsableAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var usableContainer = UsableContainerController.Instance.UsableContainer;
            //MessageDispatcher.Instance.DispatchMsg(0f, entity.UniqueId,
            //    usableContainer.CurrentUsableObject, Enums.Telegrams.UseUsable, null);
            MessageDispatcher2.Instance.DispatchMsg("UseUsable", 0f, entity.UniqueId,
                usableContainer.CurrentUsableObject, entity.UniqueId);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
