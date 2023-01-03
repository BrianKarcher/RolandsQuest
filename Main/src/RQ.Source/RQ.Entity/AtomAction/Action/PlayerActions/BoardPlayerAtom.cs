using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action.Player
{
    [Serializable]
    public class BoardPlayerAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            // Set player position with offset

            // Set player joint

            // Set player can't move
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
