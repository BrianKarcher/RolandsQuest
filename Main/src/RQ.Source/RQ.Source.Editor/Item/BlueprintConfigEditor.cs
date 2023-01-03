using RQ.Entity.Skill;
using UnityEditor;

namespace RQ.Editor.Item
{
    [CustomEditor(typeof(BlueprintConfig), true)]
    public class BlueprintConfigEditor : RQBaseConfigEditor
    {
        [MenuItem("Assets/Create/RQ/Skill/Blueprint Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<BlueprintConfig>("NewBlueprintConfig.asset");
        }
    }
}
