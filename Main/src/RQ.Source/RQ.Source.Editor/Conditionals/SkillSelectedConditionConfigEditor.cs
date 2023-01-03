using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SkillSelectedConditionConfig), true)]
    public class SkillSelectedConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Skill Selected Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<SkillSelectedConditionConfig>("NewSkillSelectedConditionConfig.asset");
        }
    }
}
