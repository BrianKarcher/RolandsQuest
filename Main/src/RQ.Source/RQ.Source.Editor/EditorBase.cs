using UnityEngine;
using UnityEditor;
using System.IO;

namespace RQ.Editor
{
    public abstract class EditorBase : UnityEditor.Editor
    {
        public void Dirty()
        {
            Dirty(true);
        }

        public void Dirty(bool makeSceneDirty)
        {
            EditorUtility.SetDirty(target);
            if (makeSceneDirty && !Application.isPlaying)
            {
                EditorApplication.MarkSceneDirty();
            }
        }

        protected static T CreateAsset<T>(string fileName) where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            //string path = "Assets/Items/SpriteAnimations.asset";
            //if (Selection.activeObject != null)
            //{
            //    path = EditorUtility.GetAssetPath(Selection.activeGameObject) + "/SpriteAnimations.asset";
            //}
            T newAsset = ScriptableObject.CreateInstance<T>();
            //PopulateNewAsset(newAsset);
            //AssetDatabase.CreateAsset(sceneData, path);
            AssetDatabase.CreateAsset(newAsset, AssetDatabase.GenerateUniqueAssetPath(path + "/" + fileName));
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
            return newAsset;
        }

        //protected static virtual void PopulateNewAsset<T>(T newAsset)
        //{

        //}
    }
}
