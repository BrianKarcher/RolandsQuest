using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(ItemUsedConditionConfig), true)]
    public class ItemUsedConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Item Used Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<ItemUsedConditionConfig>("NewItemUsedConditionConfig.asset");
        }
    }
}
