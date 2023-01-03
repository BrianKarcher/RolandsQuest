using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(PathFinishedConditionConfig), true)]
    public class PathFinishedConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Path Finished Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<PathFinishedConditionConfig>("NewPathFinishedConditionConfig.asset");
        }
    }
}
