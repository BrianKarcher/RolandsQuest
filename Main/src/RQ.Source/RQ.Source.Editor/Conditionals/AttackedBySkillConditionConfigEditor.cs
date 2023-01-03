using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(AttackedBySkillConditionConfig), true)]
    public class AttackedBySkillConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Attacked By Skill Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<AttackedBySkillConditionConfig>("NewAttackedBySkillConditionConfig.asset");
        }
    }
}
