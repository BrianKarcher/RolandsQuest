using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.ManageScene;
using RQ.Entity.Common;
using System;
using System.Collections;
using RQ.Controller.Contianers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RQ.Controllers
{
    [AddComponentMenu("RQ/Manager/Scene Controller")]
    public class SceneController : MonoBehaviour
    {
        public event Action BeforeSceneUnload;
        public event Action AfterSceneLoad;
        public float fadeDuration;
        public SceneConfig startingScene;
        //public string startingSceneName;
        public string initialStartingPositionName;

        private bool isFading;
        private Coroutine _currentSceneSwitch = null;
        //private IEnumerator Start()
        //{
        //    if (SceneManager.sceneCount == 1)
        //        yield return StartCoroutine(LoadSceneAndSetActive(startingScene.SceneName));
        //    StartCoroutine(Fade(0f));
        //}

        public void Awake()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
            if (sceneSetup != null)
            {
                sceneSetup.LevelLoaded();
            }
        }

        public void FadeAndLoadScene(string sceneName)
        {
            Debug.Log($"Loading scene {sceneName}");
            // In case scenes get switched quickly and the previous one didn't fully load, just abort it
            // and load this instead.
            if (_currentSceneSwitch != null)
                StopCoroutine(_currentSceneSwitch);
            //StopCoroutine()
            _currentSceneSwitch = StartCoroutine(FadeAndSwitchScenes(sceneName));
        }

        public bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                    return true;
            }
            return false;
        }

        //public void LoadScene(string sceneName)
        //{
        //    // Ensure the same scene is not loaded twice

        //    SceneManager.LoadScene(sceneName);
        //}

        private IEnumerator FadeAndSwitchScenes(string sceneName)
        {
            yield return StartCoroutine(Fade(1f));
            if (BeforeSceneUnload != null)
                BeforeSceneUnload();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                //if ()
                if (!scene.name.Contains("Persist"))
                {
                    Debug.Log($"(SceneController) Unloading Scene {scene.name}");
                    yield return SceneManager.UnloadSceneAsync(scene.name);
                }
            }
            ClearScene(true);
             //SceneManager.GetAllScenes();
            //yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
            if (AfterSceneLoad != null)
                AfterSceneLoad();
            //var sceneSetup = GameController.Instance.GetSceneSetup();
            var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
            //var _overlayColor = sceneSetup.GetSceneLoadColorInfo();

            if (sceneSetup == null)
            {
                yield return StartCoroutine(Fade(0f));
            }
            else if (sceneSetup.GetSceneLoadPerformFadeIn())
            {
                yield return StartCoroutine(Fade(0f));
                //Debug.Log("Performing fade in (BeginSceneState).");
                //GameController.Instance.GetGraphicsEngine().TweenOverlayToColor(_overlayColor);
                //base.TweenOverlayToColor();
            }
            
        }

        private void ClearScene(bool destroyAllEntities)
        {
            //Debug.Log("SceneController.ClearScene called");
            //if (destroyAllEntities)
            //    EntityController.Instance.DestroyAllEntities();
            //else
            //    EntityController.Instance.DestroyReceatedEntities();
            EntityController.Instance.Cleanup();
            //Cleanup();
            EntityContainer._instance.SetMainCharacter(null);
            EntityContainer._instance.SetCompanionCharacter(null);
            if (GameDataController.Instance.Data != null)
            {
                GameDataController.Instance.Data.NextSpawnpointUniqueId = null;
                UsableContainerController.Instance.UsableContainer.ClearList();
                UsableContainerController.Instance.UsableContainer.SetCurrentUsable(null);
            }

            //InputManager.Instance.RemoveAllEntities();
            // Each scene has its own Action Controller, make sure we don't use one from a previous scene in the new scene
            GameController.Instance.ActionController = null;
        }

        private IEnumerator LoadSceneAndSetActive(string sceneName)
        {
            Debug.Log($"SceneController LoadSceneAsync called for {sceneName}");
            //yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            //Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            Scene newlyLoadedScene = SceneManager.GetSceneByPath(sceneName);
            Debug.Log($"Setting Active scene to {sceneName}");
            SceneManager.SetActiveScene(newlyLoadedScene);
            yield return null;
        }

        private IEnumerator Fade(float finalAlpha)
        {
            var overlayWindow = GameController.Instance.GetGraphicsEngine().GetOverlayWindow();
            overlayWindow.TweenOverlayToColor(
                new RQ.Model.TweenToColorInfo(new Color(0, 0, 0, finalAlpha), 0, fadeDuration));
            isFading = true;
            var endTime = Time.unscaledTime + fadeDuration;
            //float fadeSpeed = Math.Abs(overlay;
            //while (!Mathf.Approximately(overlayWindow.GetOverlayColor().a, finalAlpha)
            while (Time.unscaledTime < endTime)
            {
                yield return null;
            }
            isFading = false;

        }
    }
}
