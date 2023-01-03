using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;
using UnityEngine;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class BeginSceneAtom : AtomActionBase
    {
        private bool _placedPlayer;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _placedPlayer = false;
        }

        public override void End()
        {
        }

        // This is placed in Update because we need to wait until the Main Character and companion
        // are added to the Entity Container, and thus need to wait a frame.
        public override AtomActionResults OnUpdate()
        {
            if (!_placedPlayer)
            {
                _placedPlayer = true;
                return AtomActionResults.Success;
                //SendMessageToSelf(0f, Enums.Telegrams.StateComplete, this.UniqueId);
            }
            return AtomActionResults.Running;
        }
    }
}
