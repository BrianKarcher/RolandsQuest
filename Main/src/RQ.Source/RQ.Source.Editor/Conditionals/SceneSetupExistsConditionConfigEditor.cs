using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SceneSetupExistsConditionConfig), true)]
    public class SceneSetupExistsConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/SceneSetup Exists Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<SceneSetupExistsConditionConfig>("NewSceneSetupExistsConditionConfig.asset");
        }
    }
}
