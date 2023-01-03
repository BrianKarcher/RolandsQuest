using System;
using UnityEngine;
using UnityEditor;
using RQ.Controller.ManageScene;
using RQ.Editor.Common;
using RQ.Common.Config;

namespace RQ.Editor
{
    [CustomEditor(typeof(RQBaseConfig), true)]
    public class RQBaseConfigEditor : EditorBase
    {
        private RQBaseConfig baseAgent;

        //[MenuItem("Assets/Create/RQ/Story/Act Config")]
        //public static void CreateNewAsset()
        //{
        //    var actConfig = ScriptableObject.CreateInstance<StoryActConfig>();
        //    AssetDatabase.CreateAsset(actConfig, "Assets/Story/Acts/NewActConfig.asset");
        //    EditorUtility.FocusProjectWindow();
        //    Selection.activeObject = actConfig;
        //}

        public override void OnInspectorGUI()
        {
            baseAgent = target as RQBaseConfig;
            GUI.changed = false;
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate UniqueId"))
            {
                baseAgent.UniqueId = Guid.NewGuid().ToString();
                Dirty(true);
            }

            if (GUILayout.Button("Add to Asset List"))
            {
                var gameConfig = Utils.FindAssetsByType<GameConfig>()[0];
                //if (!gameConfig.SceneConfigs.Contains(baseAgent as SceneConfig))
                //{
                //    gameConfig.SceneConfigs.Add(baseAgent as SceneConfig);
                //    EditorUtility.SetDirty(gameConfig);
                //}
                if (!gameConfig.Assets.Contains(baseAgent))
                {
                    gameConfig.Assets.Add(baseAgent);
                    EditorUtility.SetDirty(gameConfig);
                }
                
                //Dirty(false);
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();

            }

            if (GUILayout.Button("Remove from Asset List"))
            {
                var gameConfig = Utils.FindAssetsByType<GameConfig>()[0];
                //if (!gameConfig.SceneConfigs.Contains(baseAgent as SceneConfig))
                //{
                //    gameConfig.SceneConfigs.Add(baseAgent as SceneConfig);
                //    EditorUtility.SetDirty(gameConfig);
                //}
                if (gameConfig.Assets.Contains(baseAgent))
                {
                    gameConfig.Assets.Remove(baseAgent);
                    EditorUtility.SetDirty(gameConfig);
                }

                //Dirty(false);
                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();

            }
        }
    }
}
