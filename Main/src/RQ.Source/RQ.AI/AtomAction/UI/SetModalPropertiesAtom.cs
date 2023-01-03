using RQ.Controller.UI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    public class SetModalPropertiesAtom : AtomActionBase
    {
        public string ModalText = string.Empty;

        public bool HasCancelButton = false;

        protected IUIManager _entity;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity.GetComponent<IUIManager>();
            if (_entity == null)
                _entity = GameController.Instance.UIManager;
            if (!String.IsNullOrEmpty(ModalText))
                GameController.Instance.UIManager.SetModalText(ModalText);
            _entity.SetupModal(HasCancelButton);
        }

        public override void End()
        {
            base.End();
            
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
