using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Editor.Common;
using RQ.Model.GameData.StoryProgress;
using RQ.Editor.GUIStyles;

namespace RQ.Editor
{
    [CustomEditor(typeof(StoryConfig), true)]
    public class StoryConfigEditor : EditorBase
    {
        StoryConfig agent;

        [MenuItem("Assets/Create/RQ/Story/Story Config")]
        public static void CreateNewAsset()
        {
            var storyConfig = ScriptableObject.CreateInstance<StoryConfig>();
            AssetDatabase.CreateAsset(storyConfig, "Assets/Story/NewStoryConfig.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = storyConfig;
        }

        public void OnEnable()
        {
            agent = target as StoryConfig;
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();
            if (agent.ActConfigs == null)
                agent.ActConfigs = new List<StoryActConfig>();


            agent.Name = EditorGUILayout.TextField("Name", agent.Name);
                        
            //_showSpawnPoints = GUILayout.Toggle(_showSpawnPoints, "Spawn Points", "Button");

            //var spawnPoints = agent.SpawnPoints;

            //if (_showSpawnPoints)
            //{
                if (GUILayout.Button("Add Act", GUILayout.Width(130)))
                {
                    agent.ActConfigs.Add(null);
                }

                // Table Header
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Id", GUILayout.Width(20));
                EditorGUILayout.LabelField("Act");
                EditorGUILayout.EndHorizontal();

                int removeIndex = -1;
                int moveDown = -1;
                int moveUp = -1;

                for (int i = 0; i < agent.ActConfigs.Count; i++)
                {
                    var act = agent.ActConfigs[i];
                    //GUILayout.Space(4.0f);
                    //GUILayout.BeginVertical(tk2dExternal.Skin.Inst.GetStyle("InspectorHeaderBG"));
                    EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Id", GUILayout.Width(15));
                    EditorGUILayout.LabelField(act == null ? "0" : act.Id.ToString(), GUILayout.Width(20));
                    //spawnPoint.Name = EditorGUILayout.TextField("Name", spawnPoint.Name);
                    
                    //EditorGUILayout.LabelField("Unique Id", spawnPoint.UniqueId);
                    //EditorGUILayout.LabelField("Chapter", GUILayout.Width(50));
                    agent.ActConfigs[i] = EditorGUILayout.ObjectField(act, typeof(StoryActConfig), GUILayout.Width(150)) as StoryActConfig;
                    //spawnPoint.IsInitialSpawnPoint = EditorGUILayout.Toggle("Initial Spawn Point", spawnPoint.IsInitialSpawnPoint);
                    //spawnPoint.ExtraInfo = EditorGUILayout.TextField("Extra Info", spawnPoint.ExtraInfo);
                    GUI.enabled = (i != agent.ActConfigs.Count - 1);
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
                        var actName = act == null ? "(null)" : act.Name;
                        if (EditorUtility.DisplayDialog("Remove Act?", "Remove Act " + actName + "?", "Yes", "No"))
                        {
                            removeIndex = i;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //GUILayout.EndVertical();
                }

                if (removeIndex != -1)
                {
                    agent.ActConfigs.RemoveAt(removeIndex);
                    Dirty(false);
                }
                if (moveDown != -1)
                {
                    agent.ActConfigs.Swap(moveDown, moveDown + 1);
                    Dirty(false);
                }
                if (moveUp != -1)
                {
                    agent.ActConfigs.Swap(moveUp, moveUp - 1);
                    Dirty(false);
                }
            //}

            if (GUI.changed)
            {
                Dirty(false);
            }
        }
    }
}
