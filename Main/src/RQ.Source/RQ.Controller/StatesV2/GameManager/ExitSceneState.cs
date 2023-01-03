using UnityEngine;

namespace RQ.Controller.StatesV2.GameManager
{
    [AddComponentMenu("RQ/States/State/Game Manager/Exit Scene")]
    public class ExitSceneState : GMBase
    {
        public override void Enter()
        {
            base.Enter();
            if (GameController.Instance.ActionController != null)
            {
                GameController.Instance.ActionController.RunEndSceneActions();
                GameController.Instance.ActionController.CheckAndRunActionSequences("End");
            }
            Complete();
        }

        protected override void TweenOverlayToColor()
        {
        }
    }
}
