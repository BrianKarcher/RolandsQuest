using RQ.Common.Controllers;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using System;
using UnityEngine;

namespace RQ.AI.Atom.GameManager
{
    [Serializable]
    public class LoadSceneAtom : AtomActionBase
    {
        public string SceneName;
        public bool LoadNextScene;
        private bool _startCutscene;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _startCutscene = false;
            string nextScene = null;
            //if (GameDataController.Instance.NextSceneConfig != null)
            //{
            //Debug.Log("1");
            if (GameDataController.Instance == null)
                Debug.Log("(LoadSceneAtom) GameDataController is null");
            GameDataController.Instance.CurrentSceneConfig = GameDataController.Instance.NextSceneConfig;
            //Debug.Log("2");
            if (GameDataController.Instance.NextSceneConfig == null)
                Debug.Log("(LoadSceneAtom) No NextSceneConfig");
            //if (GameDataController.Instance.NextSceneConfig.Scene == null)
            //    Debug.Log("(LoadSceneAtom) No NextSceneConfig.Scene in SceneConfig " + GameDataController.Instance.NextSceneConfig.UniqueId);
            nextScene = LoadNextScene ? GameDataController.Instance.NextSceneConfig?.SceneName : SceneName;
            //Debug.Log("3");
            GameDataController.Instance.NextSceneConfig = null;
            //}
            if (!string.IsNullOrEmpty(nextScene))
            {
                Debug.Log($"LoadSceneAtom: Loading Scene {nextScene}, LoadNextScene = {LoadNextScene}");
                //Debug.Log("4");
                GameStateController.Instance.LoadScene(nextScene);
                //Debug.Log("5");
            }
            else
            {
                //Debug.LogError($"Could not locate scene in {GameDataController.Instance.NextSceneConfig.name}");
            }
        }

        public override void End()
        {
        }

        public override AtomActionResults OnUpdate()
        {
            if (!Application.isPlaying)
                return AtomActionResults.Success;
            if (!GameStateController.Instance.ChangingScene)
            {
                if (GameStateController.Instance.PlayCutscene)
                {
                    _startCutscene = true;
                    //MessageDispatcher2.Instance.DispatchMsg("StartCutscene", 0f, string.Empty, "Game Controller", null);
                }
                else
                    return AtomActionResults.Success;
            }
            return AtomActionResults.Success;
        }

        public bool GetStartCutscene()
        {
            return _startCutscene;
        }
    }
}
