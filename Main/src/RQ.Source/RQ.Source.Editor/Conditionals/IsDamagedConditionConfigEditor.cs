using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(IsDamagedConditionConfig), true)]
    public class IsDamagedConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Is Damaged Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<IsDamagedConditionConfig>("NewIsDamagedConditionConfig.asset");
        }
    }
}
