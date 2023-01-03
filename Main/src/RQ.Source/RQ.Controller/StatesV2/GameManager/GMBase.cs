using RQ.FSM.V2.States;
using RQ.Model;
using UnityEngine;

namespace RQ.Controller.StatesV2.GameManager
{
    public class GMBase : ChangeState
    {
        [SerializeField]
        protected TweenToColorInfo _overlayColor = null;
        [SerializeField]
        private float _stateCompleteDelay = 0f;
        [SerializeField]
        private bool _hasStateCompleteFlag = false;

        public override void Enter()
        {
            base.Enter();
            TweenOverlayToColor();
            if (_hasStateCompleteFlag)
                SendMessageToSelf(_stateCompleteDelay, Enums.Telegrams.StateComplete, this.UniqueId);
        }

        protected virtual void TweenOverlayToColor()
        {
            if (_overlayColor != null)
            {
                if (_overlayColor.Active)
                {
                    GameController.Instance.GetGraphicsEngine().TweenOverlayToColor(_overlayColor);
                }
            }
        }
    }
}
