using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.ManageScene;
//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Item;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    /// <summary>
    /// Starts a new game
    /// </summary>
    [AddComponentMenu("RQ/States/State/Game Manager/Begin Game")]
    public class BeginGameState : StateBase
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

            Debug.Log("Entering Begin Game state " + Time.time);

            GameController.Instance.BeginNewGame();
            MessageDispatcher2.Instance.DispatchMsg(_audioMessage, 0f, this.UniqueId, "Game Controller", null);
            Complete();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exiting Begin Game state " + Time.time);
        }
    }
}
