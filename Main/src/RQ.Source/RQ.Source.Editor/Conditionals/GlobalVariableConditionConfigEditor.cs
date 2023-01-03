using RQ.Entity.StatesV2.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(GlobalVariableConditionConfig), true)]
    public class GlobalVariableConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Global Variable Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<GlobalVariableConditionConfig>("NewGlobalVariableConditionConfig.asset");
        }
    }
}
