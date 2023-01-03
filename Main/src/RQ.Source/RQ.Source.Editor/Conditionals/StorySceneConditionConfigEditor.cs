using UnityEditor;
using RQ.FSM.V3.Conditionals;

namespace RQ.Editor
{
    [CustomEditor(typeof(StorySceneConditionConfig), true)]
    public class StorySceneConditionConfigEditor : EditorBase
    {
        StorySceneConditionConfig agent;

        [MenuItem("Assets/Create/RQ/Conditional/Story Scene Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<StorySceneConditionConfig>("NewStorySceneConditionConfig.asset");
            //var actConfig = ScriptableObject.CreateInstance<StoryActConfig>();
            //AssetDatabase.CreateAsset(actConfig, "Assets/Story/Acts/NewActConfig.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = actConfig;
        }
    }
}
