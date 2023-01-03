using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using RQ.Controller.ManageScene;

namespace RQ.Editor
{
    [CustomEditor(typeof(AreaConfig), true)]
    public class AreaConfigEditor : EditorBase
    {
        private AreaConfig agent;
        private VariablesEditor _variablesEditor;

        
        private bool _showScenes = false;

        [MenuItem("Assets/Create/RQ/Area Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<AreaConfig>("NewArea.asset");
            //AreaConfig sceneData = ScriptableObject.CreateInstance<AreaConfig>();
            //AssetDatabase.CreateAsset(sceneData, "Assets/Areas/NewArea.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = sceneData;
        }

        public void OnEnable()
        {
            agent = target as AreaConfig;
            _variablesEditor = new VariablesEditor(this);
            if (agent.Variables == null)
                agent.Variables = new List<Model.Variable>();
        }

        public void OnSceneGUI()
        {
            var ev = Event.current;
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();

            //agent.Scene = EditorGUILayout.ObjectField(agent.Scene, typeof(UnityEngine.Object));

            _variablesEditor.OnInspectorGUI(agent.Variables);

            _showScenes = GUILayout.Toggle(_showScenes, "Scenes", "Button");

            if (GUILayout.Button("Add/Enable All Scenes in Build Settings"))
            {
                foreach (var scene in agent.Scenes)
                {
                    // Find valid Scene paths and make a list of EditorBuildSettingsScene
                    List<EditorBuildSettingsScene> editorBuildSettingsScenes =
                        new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                    var editorBuildScene = editorBuildSettingsScenes.FirstOrDefault(i => i.path == scene.SceneName);
                    if (editorBuildScene == null)
                    {
                        editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scene.SceneName, true));

                    }
                    else
                    {
                        editorBuildScene.enabled = true;
                    }
                    EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
                    //foreach (var sceneAsset in editorBuildSettingsScenes)
                    //{
                    //    string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                    //    if (!string.IsNullOrEmpty(scenePath))

                    //}

                    // Set the Build Settings window Scene list

                    //SceneManager.
                }
            }

            if (GUILayout.Button("Disable All in Build Settings"))
            {
                foreach (var scene in agent.Scenes)
                {
                    // Find valid Scene paths and make a list of EditorBuildSettingsScene
                    List<EditorBuildSettingsScene> editorBuildSettingsScenes =
                        new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                    var editorBuildScene = editorBuildSettingsScenes.FirstOrDefault(i => i.path == scene.SceneName);
                    if (editorBuildScene != null)
                    {
                        editorBuildScene.enabled = false;
                    }
                    EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
                }
            }

            if (_showScenes)
            {
                if (GUILayout.Button("Add Scene", GUILayout.Width(75)))
                {
                    agent.Scenes.Add(null);
                }

                if (agent.Scenes != null)
                {
                    int removeSceneIndex = -1;
                    for (int i = 0; i < agent.Scenes.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Scene", GUILayout.Width(50));
                        agent.Scenes[i] = EditorGUILayout.ObjectField(agent.Scenes[i], typeof(SceneConfig), false, GUILayout.Width(250)) as SceneConfig;
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            if (EditorUtility.DisplayDialog("Remove Scene?", "Remove scene " + agent.Scenes[i].SceneName + " from this Area?", "Yes", "No"))
                            {
                                removeSceneIndex = i;
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                    }

                    if (removeSceneIndex != -1)
                    {
                        agent.Scenes.RemoveAt(removeSceneIndex);
                    }
                }
            }

            if (GUI.changed)
            {
                Dirty();
            }
        }
    }
}
