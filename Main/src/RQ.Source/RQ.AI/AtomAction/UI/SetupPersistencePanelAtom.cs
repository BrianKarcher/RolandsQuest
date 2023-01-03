using RQ.Controller.UI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Model.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    public class SetupPersistencePanelAtom : AtomActionBase
    {
        protected IUIManager _entity;

        public bool _newGameSlot = false;
        public string _mainLabelText = null;
        public SaveOrLoad _savePanelState;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity.GetComponent<IUIManager>();
            _entity.SetupPersistenceGrid(_mainLabelText, _savePanelState, _newGameSlot);
        }

        public override void End()
        {
            base.End();
            _entity.ClearPersistenceGrid();
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
