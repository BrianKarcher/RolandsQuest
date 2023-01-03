using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RQ.Controller.ManageScene;
using RQ.Model.Serialization;
using RQ.Editor.List;
using RQ.Enums;

namespace RQ.Editor
{
    [CustomEditor(typeof(GameConfig), true)]
    public class GameConfigEditor : EditorBase
    {
        private GameConfig agent;
        private VariablesEditor _variablesEditor;

        
        private bool _showAreas = false;

        [MenuItem("Assets/Create/RQ/Game Config")]
        public static void CreateNewAsset()
        {
            GameConfig sceneData = ScriptableObject.CreateInstance<GameConfig>();
            AssetDatabase.CreateAsset(sceneData, "Assets/Areas/NewGameState.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = sceneData;
        }

        public void OnEnable()
        {
            agent = target as GameConfig;
            _variablesEditor = new VariablesEditor(this);
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

            _showAreas = GUILayout.Toggle(_showAreas, "Areas", "Button");

            if (_showAreas)
            {
                DrawAreas();
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Entity Prefabs");
            EditorGUILayout.EndHorizontal();
            ListEditor.DrawList(agent.EntityPrefabs, (entityPrefab, index) =>
            {
                entityPrefab.Type = (EntityType)EditorGUILayout.EnumPopup(entityPrefab.Type, GUILayout.Width(50));
                entityPrefab.Name = EditorGUILayout.TextField(entityPrefab.Name, GUILayout.Width(75));
                var oldPrefab = entityPrefab.Prefab;
                entityPrefab.Prefab = EditorGUILayout.ObjectField(entityPrefab.Prefab, typeof(UnityEngine.Transform), GUILayout.Width(90)) as Transform;
                if (oldPrefab != entityPrefab.Prefab)
                {
                    entityPrefab.Name = entityPrefab.Prefab.name;
                }
                //return entityPrefab.Name;
                return new ListEditor.DrawItemData<EntityPrefab>
                {
                    name = entityPrefab.Name,
                    data = entityPrefab
                };
            },
            () =>
            {
                return new EntityPrefab()
                {
                    UniqueId = Guid.NewGuid().ToString()
                };
            });

            if (GUI.changed)
            {
                Dirty();
            }
        }

        private void DrawAreas()
        {
            if (GUILayout.Button("Add Area", GUILayout.Width(120)))
            {
                AddArea();
                //CreateVariable(variables);
                //moveUp = layer;
                Repaint();
            }

            if (agent.AreaConfigs != null)
            {
                int deleteArea = -1;
                for (int i = 0; i < agent.AreaConfigs.Count; i++)
                {
                    var area = agent.AreaConfigs[i];
                    GUILayout.Space(7);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Area", GUILayout.Width(50));
                    EditorGUILayout.LabelField(area == null ? string.Empty : area.Name, GUILayout.Width(80));

                    if (GUILayout.Button("D", GUILayout.Width(30)))
                    {
                        if (EditorUtility.DisplayDialog("Delete area?", "Are you sure you want to delete the area " + (area == null ? string.Empty : area.Name) + "?", "Yes", "No"))
                        {
                            deleteArea = i;
                        }
                    }
                    GUILayout.EndHorizontal();

                    agent.AreaConfigs[i] = (AreaConfig)EditorGUILayout.ObjectField(area, typeof(AreaConfig), false, GUILayout.Width(150));

                }
                if (deleteArea != -1)
                {
                    DeleteArea(deleteArea);

                    Repaint();
                }
            }
        }

        private void AddArea()
        {
            if (agent.AreaConfigs == null)
            {
                agent.AreaConfigs = new List<AreaConfig>();
            }

            agent.AreaConfigs.Add(null);
            Dirty();
        }

        private void DeleteArea(int index)
        {
            agent.AreaConfigs.RemoveAt(index);
            Dirty();
        }
    }
}
