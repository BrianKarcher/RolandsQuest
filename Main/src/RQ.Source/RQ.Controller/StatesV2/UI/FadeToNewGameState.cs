//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/UI/Fade To New Game")]
    public class FadeToNewGameState : StateBase //<ISprite>
    {
        [SerializeField]
        private float _fadeSpeed;
        [SerializeField]
        private TweenToColorInfo _fadeColor = null;
        [SerializeField]
        private string _audioMessage;
        [SerializeField]
        private string _gameControllerMessageAfterFade;

        public override void Enter()
        {
            base.Enter();
            GameController.Instance.GetGraphicsEngine().TweenOverlayToColor(_fadeColor);
            MessageDispatcher2.Instance.DispatchMsg(_audioMessage, 0f, this.UniqueId, "Game Controller", null);
            MessageDispatcher2.Instance.DispatchMsg(_gameControllerMessageAfterFade, _fadeSpeed, this.UniqueId, "Game Controller", null);
            //if (_entity == null)
            //    return;
            //Log.Info("Entering Play state");
        }

        //public override void Exit()
        //{
        //    base.Exit();
        //    Log.Info("Exiting Play state");
        //}
    }
}
