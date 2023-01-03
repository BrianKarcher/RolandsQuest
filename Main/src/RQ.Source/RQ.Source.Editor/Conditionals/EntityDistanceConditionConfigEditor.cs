using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(EntityDistanceConditionConfig), true)]
    public class EntityDistanceConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Entity Distance Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<EntityDistanceConditionConfig>("NewEntityDistanceConditionConfig.asset");
        }
    }
}
