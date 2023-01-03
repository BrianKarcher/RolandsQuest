using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(SeesTargetConditionConfig), true)]
    public class SeesTargetConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/SeesTarget Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<SeesTargetConditionConfig>("NewSeesTargetConditionConfig.asset");
        }
    }
}
