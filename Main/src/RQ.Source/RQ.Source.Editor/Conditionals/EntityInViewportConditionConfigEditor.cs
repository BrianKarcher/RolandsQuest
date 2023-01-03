using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(EntityInViewportConfig), true)]
    public class EntityInViewportConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Entity In Viewport Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<EntityInViewportConfig>("NewEntityInViewportConditionConfig.asset");
        }
    }
}
