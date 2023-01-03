using RQ.Controller.ManageScene;
using RQ.Editor.Common;
using RQ.Model.Item;
using UnityEditor;

namespace RQ.Editor.Item
{
    [CustomEditor(typeof(SkillConfig), true)]
    public class SkillConfigEditor : RQBaseConfigEditor
    {
        [MenuItem("Assets/Create/RQ/Skill Config")]
        public static void CreateNewAsset()
        {
            var newSkill = EditorBase.CreateAsset<SkillConfig>("NewSkill.asset");
            var gameConfig = Utils.FindAssetsByType<GameConfig>()[0];
            //if (!gameConfig.SceneConfigs.Contains(baseAgent as SceneConfig))
            //{
            //    gameConfig.SceneConfigs.Add(baseAgent as SceneConfig);
            //    EditorUtility.SetDirty(gameConfig);
            //}

            gameConfig.Assets.Add(newSkill);
            EditorUtility.SetDirty(gameConfig);

            //ItemConfig sceneData = ScriptableObject.CreateInstance<ItemConfig>();            
            //AssetDatabase.CreateAsset(sceneData, "Assets/Items/NewItem.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = sceneData;
        }
    }
}
