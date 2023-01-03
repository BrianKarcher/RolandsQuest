using System;
using RQ.AI;
using RQ.Common.Controllers;
using RQ.Controller.Contianers;

namespace RQ.Entity.AtomAction.Action
{
    public class CurrentUsableAvailableAtom : AtomActionBase
    {
        public override AtomActionResults OnUpdate()
        {
            var usableContainer = UsableContainerController.Instance.UsableContainer;
            var hasCurrentUsable = !String.IsNullOrEmpty(usableContainer.GetCurrentUsable());
            return hasCurrentUsable ? AtomActionResults.Success : AtomActionResults.Failure;
        }
    }
}
