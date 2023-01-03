using RQ.Controller.UI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    public class ShowPanelAtom : AtomActionBase
    {
        protected IUIManager _entity;

        public Enum.Panels[] _panels;
        public string[] panels;
        //public List<string> panels;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity.GetComponent<IUIManager>();
            if (_entity == null)
                throw new Exception("FSM - UIManager not found.");

            //if (_panels != null)
            //{
            //    foreach (var panel in _panels)
            //        _entity.ShowPanel(panel, true);
            //}
            foreach (var panel in panels)
                _entity.ShowPanel(panel, true);
        }

        public override void End()
        {
            base.End();
            if (_panels == null)
                return;

            //foreach (var panel in _panels)
            //    _entity.ShowPanel(panel, false);
            foreach (var panel in panels)
                _entity.ShowPanel(panel, false);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Running;
        }
    }
}
