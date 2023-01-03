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
    public class SceneSetupExistsAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            Debug.Log("Entering AppStart State");
            GameController.Instance.AppStart();

            // Load the resources at app start
            //var resources = ConfigsContainer.Instance;
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
