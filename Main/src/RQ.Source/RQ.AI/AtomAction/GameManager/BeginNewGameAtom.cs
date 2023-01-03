using RQ.Common.Container;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class BeginNewGameAtom : AtomActionBase
    {
        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            Debug.Log("Entering Begin Game state " + Time.time);

            GameController.Instance.BeginNewGame();
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
