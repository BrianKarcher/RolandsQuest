using RQ.Common.Controllers;
using RQ.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Title Screen")]
    public class TitleScreenState : MenuState
    {
        [SerializeField]
        private string _usableInactiveMessage;

        public override void Enter()
        {
            Debug.LogWarning("Entering Title Screen state");
            base.Enter();
            
            GameDataController.Instance.DeleteAllGameData();
            // The title screen is an unpaused state
            MessageDispatcher2.Instance.DispatchMsg("Unpause", 0f, string.Empty, "Game Controller", null);

            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _audioComponent.UniqueId,
            //    Enums.Telegrams.StopSound, null);            
            if (!string.IsNullOrEmpty(_usableInactiveMessage))
            {
                MessageDispatcher2.Instance.DispatchMsg(_usableInactiveMessage, 0f,
                    this.UniqueId, "Usable Controller", null);
            }
            var titleScreenScene = GameController.Instance.TitleScreenScene;
            var titleScreen = SceneManager.GetSceneByName(titleScreenScene);

            if (!string.IsNullOrEmpty(titleScreenScene) && titleScreen == null)
                GameStateController.Instance.LoadScene(titleScreenScene);
            //GameController.Instance.GetGraphicsEngine().SetOverlayToColor(new Color(0,0,0,0));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
