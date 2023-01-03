using UnityEditor;
using RQ.Model.GameData.StoryProgress;

namespace RQ.Editor
{
    [CustomEditor(typeof(StorySceneConfig), true)]
    public class StorySceneConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Story/Scene Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<StorySceneConfig>("NewSceneConfig.asset");
            //var actConfig = ScriptableObject.CreateInstance<StorySceneConfig>();
            //AssetDatabase.CreateAsset(actConfig, "Assets/Story/NewSceneConfig.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = actConfig;
        }
    }
}
