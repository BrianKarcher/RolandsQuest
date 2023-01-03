using RQ.Entity.Skill;
using UnityEditor;

namespace RQ.Editor.Item
{
    [CustomEditor(typeof(MoldConfig), true)]
    public class MoldConfigEditor : RQBaseConfigEditor
    {
        [MenuItem("Assets/Create/RQ/Skill/Mold Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<MoldConfig>("NewMoldConfig.asset");
        }
    }
}
