using RQ.Common.Controllers;
using RQ.Controller.ManageScene;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Common.UI
{
    [AddComponentMenu("RQ/UI/On Click Load Scene")]
    public class OnClickLoadScene : OnClickHandler
    {
        [SerializeField]
        private SceneConfig _scene;

        public override void OnClick()
        {
            if (GameDataController.Instance.Data == null)
                GameDataController.Instance.CreateNewGameData();
            Debug.Log("Loading Scene " + _scene.name);
            GameDataController.Instance.NextSceneConfig = _scene;
            base.OnClick();
        }
    }
}
