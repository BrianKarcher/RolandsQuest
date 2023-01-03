using System;
using System.Collections.Generic;
using UnityEditor;
using RQ.Entity.Common;
using RQ.Editor.UnityExtensions;

namespace RQ.Editor
{
    [CustomEditor(typeof(SpriteBaseComponent), true)]
    public class SpriteBaseEditor : BaseObjectEditor
    {
        SpriteBaseComponent _spriteBaseComponent;
        //protected tk2dTileMap tileMap;
        public bool isGetCenterPointPressed = false;

        public override void OnEnable()
        {
            base.OnEnable();
            _spriteBaseComponent = target as SpriteBaseComponent;
            //tileMap = GameObject.FindObjectOfType<tk2dTileMap>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var prefabList = new List<KeyValuePair<string, string>>();
            prefabList.Add(new KeyValuePair<string, string>("", "Select Prefab..."));
            if (SceneSetup.Instance == null)
                return;
            if (SceneSetup.Instance.GameConfig == null)
                throw new Exception("Scene Setup missing GameConfig");
            var allPrefabs = SceneSetup.Instance.GameConfig.EntityPrefabs;
            if (allPrefabs == null)
                return;
            foreach (var prefab in allPrefabs)
            {
                //Debug.Log("Adding prefab " + prefab.UniqueId + " " + prefab.Name);
                prefabList.Add(new KeyValuePair<string, string>(prefab.UniqueId, prefab.Name));
            }

            _spriteBaseComponent.EntityPrefabUniqueId = ControlExtensions.Popup("Prefab", 
                _spriteBaseComponent.EntityPrefabUniqueId, prefabList);
        }
    }
}
