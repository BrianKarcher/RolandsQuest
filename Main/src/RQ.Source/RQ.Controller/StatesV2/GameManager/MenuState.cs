using RQ.Messaging;
using UnityEngine;
//using Sprites = RQ.Entity.Sprites;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Menu")]
    public class MenuState : PausableState
    {
        [SerializeField]
        private bool _stopSound;
        [SerializeField]
        private bool _pauseGame;
        [SerializeField]
        private string _uiMessageOnEnter;
        [SerializeField]
        private string _uiMessageOnFirstUpdate;
        [SerializeField]
        private string _uiMessageOnExit;

        private bool _firstUpdate = true;

        public override void Enter()
        {
            base.Enter();
            if (_stopSound)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _audioComponent.UniqueId,
                    Enums.Telegrams.StopSound, null);
            }
            if (_pauseGame)
            {
                MessageDispatcher2.Instance.DispatchMsg("Pause", 0f, string.Empty, _spriteBase.UniqueId, null);
            }
            if (!string.IsNullOrEmpty(_uiMessageOnEnter))
            {
                MessageDispatcher2.Instance.DispatchMsg(_uiMessageOnEnter, 0f, this.UniqueId, "UI Manager", null);
            }
        }

        public override void Update()
        {
            base.Update();
            if (_firstUpdate)
            {
                if (!string.IsNullOrEmpty(_uiMessageOnFirstUpdate))
                {
                    MessageDispatcher2.Instance.DispatchMsg(_uiMessageOnFirstUpdate, 0f, this.UniqueId, "UI Manager", null);
                }
                _firstUpdate = false;
            }
        }

        public override void Exit()
        {
            base.Exit();
            //if (_pauseGame)
            //{
            //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _spriteBase.UniqueId,
            //          Enums.Telegrams.Unpause, null);
            //}
            if (!string.IsNullOrEmpty(_uiMessageOnExit))
            {
                MessageDispatcher2.Instance.DispatchMsg(_uiMessageOnExit, 0f, this.UniqueId, "UI Manager", null);
            }
        }
    }
}
