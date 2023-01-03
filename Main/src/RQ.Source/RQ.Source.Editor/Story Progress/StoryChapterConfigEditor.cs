using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Editor.Common;
using RQ.Model.GameData.StoryProgress;
using RQ.Editor.GUIStyles;

namespace RQ.Editor
{
    [CustomEditor(typeof(StoryChapterConfig), true)]
    public class StoryChapterConfigEditor : EditorBase
    {
        StoryChapterConfig agent;

        [MenuItem("Assets/Create/RQ/Story/Chapter Config")]
        public static void CreateNewAsset()
        {
            var actConfig = ScriptableObject.CreateInstance<StoryChapterConfig>();
            AssetDatabase.CreateAsset(actConfig, "Assets/Story/NewChapterConfig.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = actConfig;
        }

        public void OnEnable()
        {
            agent = target as StoryChapterConfig;
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();
            if (agent.SceneConfigs == null)
                agent.SceneConfigs = new List<StorySceneConfig>();


            agent.Name = EditorGUILayout.TextField("Chapter Name", agent.Name);
                        
            //_showSpawnPoints = GUILayout.Toggle(_showSpawnPoints, "Spawn Points", "Button");

            //var spawnPoints = agent.SpawnPoints;

            //if (_showSpawnPoints)
            //{
                if (GUILayout.Button("Add Scene", GUILayout.Width(130)))
                {
                    agent.SceneConfigs.Add(null);
                }

                // Table Header
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Id", GUILayout.Width(20));
                EditorGUILayout.LabelField("Scene");
                EditorGUILayout.EndHorizontal();

                int removeIndex = -1;
                int moveDown = -1;
                int moveUp = -1;

                for (int i = 0; i < agent.SceneConfigs.Count; i++)
                {
                    var scene = agent.SceneConfigs[i];
                    //GUILayout.Space(4.0f);
                    //GUILayout.BeginVertical(tk2dExternal.Skin.Inst.GetStyle("InspectorHeaderBG"));
                    EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Id", GUILayout.Width(15));
                    EditorGUILayout.LabelField(scene == null ? "0" : scene.Id.ToString(), GUILayout.Width(20));
                    //spawnPoint.Name = EditorGUILayout.TextField("Name", spawnPoint.Name);
                    
                    //EditorGUILayout.LabelField("Unique Id", spawnPoint.UniqueId);
                    //EditorGUILayout.LabelField("Chapter", GUILayout.Width(50));
                    var oldSceneConfig = agent.SceneConfigs[i];
                    agent.SceneConfigs[i] = EditorGUILayout.ObjectField(scene, typeof(StorySceneConfig), GUILayout.Width(150)) as StorySceneConfig;
                    if (oldSceneConfig != agent.SceneConfigs[i])
                    {
                        agent.SceneConfigs[i].Id = i;
                        EditorUtility.SetDirty(agent);
                    }
                    //spawnPoint.IsInitialSpawnPoint = EditorGUILayout.Toggle("Initial Spawn Point", spawnPoint.IsInitialSpawnPoint);
                    //spawnPoint.ExtraInfo = EditorGUILayout.TextField("Extra Info", spawnPoint.ExtraInfo);
                    GUI.enabled = (i != agent.SceneConfigs.Count - 1);
                    if (GUILayout.Button("", Utils.SimpleButton("btn_down")))
                    {
                        moveDown = i;
                    }

                    GUI.enabled = (i != 0);
                    if (GUILayout.Button("", Utils.SimpleButton("btn_up")))
                    {
                        moveUp = i;
                    }

                    GUI.enabled = true;

                    if (GUILayout.Button("", Styles.CreateTilemapDeleteItemStyle()))
                    {
                        var sceneName = scene == null ? "(null)" : scene.Name;
                        if (EditorUtility.DisplayDialog("Remove Scene?", "Remove Scene " + sceneName + "?", "Yes", "No"))
                        {
                            removeIndex = i;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //GUILayout.EndVertical();
                }

                if (removeIndex != -1)
                {
                    agent.SceneConfigs.RemoveAt(removeIndex);
                    ResetIds();
                    Dirty(false);
                }
                if (moveDown != -1)
                {
                    agent.SceneConfigs.Swap(moveDown, moveDown + 1);
                    ResetIds();
                    Dirty(false);
                }
                if (moveUp != -1)
                {
                    agent.SceneConfigs.Swap(moveUp, moveUp - 1);
                    ResetIds();
                    Dirty(false);
                }

                if (GUILayout.Button("Reset Ids"))
                {
                    if (EditorUtility.DisplayDialog("Reset Ids?", "Reset all Ids for this Chapter?", "Yes", "No"))
                    {
                        ResetIds();
                    }
                }
            //}

            if (GUI.changed)
            {
                Dirty(false);
            }
        }

        private void ResetIds()
        {
            for (int i = 0; i < agent.SceneConfigs.Count; i++)
            {
                agent.SceneConfigs[i].Id = i;
                EditorUtility.SetDirty(agent);
            }
        }
    }
}
