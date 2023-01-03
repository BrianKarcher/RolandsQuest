using RQ.Common.Controllers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoPlayModeSceneSetup
{
    static AutoPlayModeSceneSetup()
    {
        // Ensure at least one build scene exist.
        if (EditorBuildSettings.scenes.Length == 0)
            return;
        //var sceneSetup = GameObject.FindObjectOfType<RQ.SceneSetup>();
        //var activeScene = EditorSceneManager.GetActiveScene();
        //var activeSceneName = activeScene.name;
        //Debug.Log($"Active scene name: {sceneSetup?.SceneConfig?.name}");
        //GameDataController.Instance.AppStartScene = sceneSetup.SceneConfig;
        // Set Play Mode scene to first scene defined in build settings.
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
    }
}