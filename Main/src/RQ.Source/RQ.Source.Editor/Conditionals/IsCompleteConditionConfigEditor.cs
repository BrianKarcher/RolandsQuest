using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(IsCompleteCondition), true)]
    public class IsCompleteConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Is Complete Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<IsCompleteCondition>("NewIsCompleteConditionConfig.asset");
        }
    }
}
