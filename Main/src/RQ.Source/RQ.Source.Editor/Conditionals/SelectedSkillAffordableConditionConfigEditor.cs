using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SelectedSkillAffordableConditionConfig), true)]
    public class SelectedSkillAffordableConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Selected Skill Affordable Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<SelectedSkillAffordableConditionConfig>("NewSelectedSkillAffordableConditionConfig.asset");
        }
    }
}
