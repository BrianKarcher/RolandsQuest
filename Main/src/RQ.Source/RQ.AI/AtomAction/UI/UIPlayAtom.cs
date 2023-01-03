using RQ.AI;
using RQ.Controller.UI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using System.Collections.Generic;

namespace RQ.AI.Atom.UI
{
    [Serializable]
    [Obsolete]
    public class UIPlayAtom : AtomActionBase
    {
        protected IUIManager _entity;
        //private Rewired.Player _player;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //    _entity = entity.GetComponent<IUIManager>();
            //    if (_entity == null)
            //        throw new Exception("FSM - UIManager not found.");
            //    if (_player == null)
            //        _player = Rewired.ReInput.players.Players[0];
        }

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {
            //    if (_player.GetButtonDown(_toggleAction))
            //    {
            //        _entity.ToggleShard();
            //    }
            return AtomActionResults.Success;
        }
    }
}
