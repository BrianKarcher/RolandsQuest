using RQ.Editor.GUIStyles;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RQ.Editor.Common
{
    public static class Utils
    {
        public static GUIStyle SimpleButton(string textureInactive)
        {
            return SimpleButton(textureInactive, "");
        }

        public static GUIStyle SimpleButton(string textureInactive, string textureActive)
        {
            GUIStyle style = Styles.CreateSimpleButtonTemplateStyle();
            style.normal.background = GetTexture(textureInactive);
            style.active.background = string.IsNullOrEmpty(textureActive) ? null : GetTexture(textureActive);
            return style;
        }

        public static Texture2D GetTexture(string name)
        {
            Texture2D tex = null;
            //if (skinTextures.TryGetValue(name, out tex) && tex != null)
            //{
            //    return tex;
            //}

            tex = Resources.Load<Texture2D>("tk2dSkin/" + name);
            if (tex == null)
            {
                Debug.LogError("tk2d - Cant find skin texture " + name);
                return GetTexture("white");
            }

            return tex;
        }

        //public static Texture2D GetTexture(string name)
        //{
        //    return tk2dEditorSkin.GetTexture(name);
        //}

        //public static GUIStyle GetStyle(string name)
        //{
        //    return tk2dEditorSkin.GetStyle(name);
        //}

        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}
