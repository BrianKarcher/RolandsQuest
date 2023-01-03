using RQ.Controller.UI;
using RQ.FSM.V2.States;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Panel")]
    public class PanelState : ChangeState //<ISprite>
    {
        protected IUIManager _entity;

        //[SerializeField]
        //private List<Enum.Panels> _panels = null;
        [SerializeField]
        private GameObject _focus;

        private PanelState _parentState;
        [SerializeField]
        private string _GMMessageOnEnter;
        //[SerializeField]
        //private string _GMMessageOnFirstUpdate;
        [SerializeField]
        private string _GMMessageOnExit;

        public override void SetupState()
        {
            base.SetupState();
            _entity = _spriteBase.GetComponent<IUIManager>();
            if (_entity == null)
                throw new Exception("FSM - UIManager not found.");
        }

        public override void Enter()
        {
            base.Enter();
            if (_entity == null)
                return;
            //GetAllPanelButtons();

            //if (_panels != null)
            //{
            //    //_entity.HideAllPanels();
            //    foreach (var panel in _panels)
            //        _entity.ShowPanel(panel, true);
            //}
            //if (_focus != null)
            //    UICamera.hoveredObject = _focus;

            // Treat this as a modal dialog by disabling any parent panel states
            _parentState = StateMachine.GetParentState() as PanelState;

            EnableParentPanel(false);
            SetEnabled(true);
            if (!string.IsNullOrEmpty(_GMMessageOnEnter))
            {
                MessageDispatcher2.Instance.DispatchMsg(_GMMessageOnEnter, 0f, this.UniqueId, "Game Controller", null);
            }
            //UIButton button;
            ////button
            //UIWidget widget;
            //widget.
            //button.SetState(UIButtonColor.State.)
        }

        public override void Exit()
        {
            base.Exit();
            //if (_panels == null)
            //    return;
            
            if (_entity == null)
                throw new Exception("Entity is null in PanelState in " + name);

            //foreach (var panel in _panels)
            //    _entity.ShowPanel(panel, false);

            EnableParentPanel(true);
            SetEnabled(false);

            if (!string.IsNullOrEmpty(_GMMessageOnExit))
            {
                MessageDispatcher2.Instance.DispatchMsg(_GMMessageOnExit, 0f, this.UniqueId, "Game Controller", null);
            }
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("HandleInput", this.UniqueId, (data) =>
                {
                    return;
                });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("HandleInput", this.UniqueId, -1);
        }

        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);
            if (enabled)
            {
                
                //InputManager.Instance.AddEntity(this);
                //NGUITools.
            }
            else
            {
                //EnableParentPanel(true);
                //InputManager.Instance.RemoveEntity(this);
            }

            EnableButtons(enabled);
        }

        protected void EnableButtons(bool state)
        {
            var entity = GetEntity();
            if (entity == null)
            {
                Debug.Log("No Sprite Base in " + this.name);
                return;
            }
            if (_entity == null)
                _entity = entity.GetComponent<IUIManager>();
            //if (_panels != null)
            //    _entity.EnablePanelButtons(_panels, state);
            //if (buttons == null)
            //    return;

            //PanelStateState panelstates = new PanelStateState()
            //{
            //    Panels = _panels,
            //    Enabled = state
            //};
            //MessageDispatcher2.Instance.DispatchMsg("EnablePanelButtons", 0f, this.UniqueId, "UI Manager", panelstates);
            //GetAllPanelButtons();
        }

        private void EnableParentPanel(bool enable)
        {
            if (_parentState != null)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                    _parentState.UniqueId, Enums.Telegrams.SetEnabled, enable);
            }
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.SetEnabled:
                    var enabled = (bool)telegram.ExtraInfo;
                    EnableButtons(enabled);
                    break;
            }

            return false;
        }
    }
}
