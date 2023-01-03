using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(AttackCountConditionConfig), true)]
    public class AttackCountConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Attack Count Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<AttackCountConditionConfig>("NewAttackCountConditionConfig.asset");
        }
    }
}
