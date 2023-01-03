using RQ.Controller.StatesV3.Conditions;
using RQ.Entity.StatesV3.Conditions;
using RQ.FSM.V3.Conditionals;
using UnityEditor;

namespace RQ.Editor
{
    [CustomEditor(typeof(EntityInTilemapBoundsConfig), true)]
    public class EntityInTilemapBoundsConfigEditor : EditorBase
    {
        [MenuItem("Assets/Create/RQ/Conditional/Entity In Tilemap Bounds Condition Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<EntityInTilemapBoundsConfig>("NewEntityInTilemapBoundsConditionConfig.asset");
        }
    }
}
