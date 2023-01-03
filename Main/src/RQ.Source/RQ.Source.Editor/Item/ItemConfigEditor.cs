using RQ.Model.Item;
using UnityEditor;

namespace RQ.Editor.Item
{
    [CustomEditor(typeof(ItemConfig), true)]
    public class ItemConfigEditor : RQBaseConfigEditor
    {
        [MenuItem("Assets/Create/RQ/Item Config")]
        public static void CreateNewAsset()
        {
            EditorBase.CreateAsset<ItemConfig>("NewItem.asset");
            //ItemConfig sceneData = ScriptableObject.CreateInstance<ItemConfig>();            
            //AssetDatabase.CreateAsset(sceneData, "Assets/Items/NewItem.asset");
            //EditorUtility.FocusProjectWindow();
            //Selection.activeObject = sceneData;
        }
    }
}
