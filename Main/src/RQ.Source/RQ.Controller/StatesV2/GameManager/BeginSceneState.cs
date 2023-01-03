using UnityEngine;

namespace RQ.Controller.StatesV2.GameManager
{
    // TODO Consider moving this to the Play state machine
    [AddComponentMenu("RQ/States/State/Game Manager/Begin Scene")]
    public class BeginSceneState : GMBase
    {
        //private tk2dTileMap _tileMap;
        private bool PlacedPlayer = false;
        //private ColorOverlayEffect _fadeEffect;

        public override void Enter()
        {
            base.Enter();
            //Debug.Log("Entering Begin Scene state");
            PlacedPlayer = false;
        }

        // This is placed in Update because we need to wait until the Main Character and companion
        // are added to the Entity Container, and thus need to wait a frame.
        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;

            if (!PlacedPlayer)
            {
                PlacedPlayer = true;
                base.Complete();
                //SendMessageToSelf(0f, Enums.Telegrams.StateComplete, this.UniqueId);
            }
        }

        public override void Exit()
        {
            base.Exit();
            //StateMachine.GetStateInfo().ChangeScene = false;
            Debug.Log("Exiting Begin Scene state");
            //GameController._instance.GetGraphicsEngine().RemoveEffect(
            //    GameController._instance.SceneChangeColorOverlay);
        }

        protected override void TweenOverlayToColor()
        {
            //var sceneSetup = GameController.Instance.GetSceneSetup();
            //_overlayColor = sceneSetup.GetSceneLoadColorInfo();
            //if (sceneSetup.GetSceneLoadPerformFadeIn())
            //{
            //    //Debug.Log("Performing fade in (BeginSceneState).");
            //    GameController.Instance.GetGraphicsEngine().TweenOverlayToColor(_overlayColor);
            //    //base.TweenOverlayToColor();
            //}
        }
    }
}
