using RQ.AI;
using RQ.Controller.UI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    public class EnableButtonsAtom : AtomActionBase
    {
        public List<Enum.Panels> Panels = null;
        public List<string> EnablePanels;
        public bool Enabled;

        protected IUIManager _entity;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity.GetComponent<IUIManager>();
            if (_entity == null)
                throw new Exception("FSM - UIManager not found.");
            EnableButtons(Enabled);
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        protected void EnableButtons(bool state)
        {
            //if (Panels != null)
            //    _entity.EnablePanelButtons(Panels, state);
            _entity.EnablePanelButtons(EnablePanels, state);
        }
    }
}
