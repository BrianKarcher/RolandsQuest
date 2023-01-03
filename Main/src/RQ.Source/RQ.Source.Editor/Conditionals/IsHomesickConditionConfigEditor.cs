using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(IsHomesickConditionConfig), true)]
    public class IsHomesickConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Is Homesick Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<IsHomesickConditionConfig>("NewIsHomesickConditionConfig.asset");
        }
    }
}
