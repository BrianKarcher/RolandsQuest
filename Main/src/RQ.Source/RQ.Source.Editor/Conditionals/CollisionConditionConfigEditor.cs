using RQ.Entity.StatesV2.Conditions;
using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(CollisionConditionConfig), true)]
    public class CollisionConditionConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Collision Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<CollisionConditionConfig>("NewCollisionConditionConfig.asset");
        }
    }
}
