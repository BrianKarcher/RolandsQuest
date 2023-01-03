using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Editor.Common;
using RQ.Model.GameData.StoryProgress;
using RQ.Editor.GUIStyles;

namespace RQ.Editor
{
    [CustomEditor(typeof(StoryActConfig), true)]
    public class StoryActConfigEditor : EditorBase
    {
        StoryActConfig agent;

        [MenuItem("Assets/Create/RQ/Story/Act Config")]
        public static void CreateNewAsset()
        {
            var actConfig = ScriptableObject.CreateInstance<StoryActConfig>();
            AssetDatabase.CreateAsset(actConfig, "Assets/Story/Acts/NewActConfig.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = actConfig;
        }

        public void OnEnable()
        {
            agent = target as StoryActConfig;
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;
            base.OnInspectorGUI();
            if (agent.ChapterConfigs == null)
                agent.ChapterConfigs = new List<StoryChapterConfig>();


            agent.Name = EditorGUILayout.TextField("Act Name", agent.Name);
                        
            //_showSpawnPoints = GUILayout.Toggle(_showSpawnPoints, "Spawn Points", "Button");

            //var spawnPoints = agent.SpawnPoints;

            //if (_showSpawnPoints)
            //{
                if (GUILayout.Button("Add Chapter", GUILayout.Width(130)))
                {
                    agent.ChapterConfigs.Add(null);
                }

                // Table Header
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Id", GUILayout.Width(20));
                EditorGUILayout.LabelField("Chapter");
                EditorGUILayout.EndHorizontal();

                int removeIndex = -1;
                int moveDown = -1;
                int moveUp = -1;

                for (int i = 0; i < agent.ChapterConfigs.Count; i++)
                {
                    var chapter = agent.ChapterConfigs[i];
                    //GUILayout.Space(4.0f);
                    //GUILayout.BeginVertical(tk2dExternal.Skin.Inst.GetStyle("InspectorHeaderBG"));
                    EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Id", GUILayout.Width(15));
                    EditorGUILayout.LabelField(chapter == null ? "0" : chapter.Id.ToString(), GUILayout.Width(20));
                    //spawnPoint.Name = EditorGUILayout.TextField("Name", spawnPoint.Name);
                    
                    //EditorGUILayout.LabelField("Unique Id", spawnPoint.UniqueId);
                    //EditorGUILayout.LabelField("Chapter", GUILayout.Width(50));
                    agent.ChapterConfigs[i] = EditorGUILayout.ObjectField(chapter, typeof(StoryChapterConfig), GUILayout.Width(150)) as StoryChapterConfig;
                    //spawnPoint.IsInitialSpawnPoint = EditorGUILayout.Toggle("Initial Spawn Point", spawnPoint.IsInitialSpawnPoint);
                    //spawnPoint.ExtraInfo = EditorGUILayout.TextField("Extra Info", spawnPoint.ExtraInfo);
                    GUI.enabled = (i != agent.ChapterConfigs.Count - 1);
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
                        var chapterName = chapter == null ? "(null)" : chapter.Name;
                        if (EditorUtility.DisplayDialog("Remove Chapter?", "Remove Chapter " + chapterName + "?", "Yes", "No"))
                        {
                            removeIndex = i;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    //GUILayout.EndVertical();
                }

                if (removeIndex != -1)
                {
                    agent.ChapterConfigs.RemoveAt(removeIndex);
                    Dirty(false);
                }
                if (moveDown != -1)
                {
                    agent.ChapterConfigs.Swap(moveDown, moveDown + 1);
                    Dirty(false);
                }
                if (moveUp != -1)
                {
                    agent.ChapterConfigs.Swap(moveUp, moveUp - 1);
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
