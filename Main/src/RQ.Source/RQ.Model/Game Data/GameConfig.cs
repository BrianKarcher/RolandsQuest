using RQ.Common.Config;
using RQ.Model;
using RQ.Model.GameData.StoryProgress;
using RQ.Model.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.ManageScene
{
    public class GameConfig : ScriptableObject
    {
        public bool IsAutoStart = true;
        public SceneConfig AutoStartSceneConfig;

        [HideInInspector]
        public List<AreaConfig> AreaConfigs;

        [HideInInspector]
        public List<Variable> Variables;

        public List<StoryActConfig> ActConfigs;

        public float GlobalSpriteZOffset;

        //[SerializeField]
        //private LayerMask _levelOneLayerMask;
        //public LayerMask LevelOneLayerMask { get { return _levelOneLayerMask; } set { _levelOneLayerMask = value; } }
        //[SerializeField]
        //private LayerMask _levelTwoLayerMask;
        //public LayerMask LevelTwoLayerMask { get { return _levelTwoLayerMask; } set { _levelTwoLayerMask = value; } }
        //[SerializeField]
        //private LayerMask _sharedLayerMask;
        //public LayerMask SharedLayerMask { get { return _sharedLayerMask; } set { _sharedLayerMask = value; } }

        [SerializeField]
        private List<EntityPrefab> _entityPrefabs;
        public List<EntityPrefab> EntityPrefabs { get { return _entityPrefabs; } }

        [SerializeField]
        private List<string> _animationTypes;
        public List<string> AnimationTypes { get { return _animationTypes; } set { _animationTypes = value; } }

        //public List<SceneConfig> SceneConfigs;
        public List<RQBaseConfig> Assets;
        private Dictionary<string, RQBaseConfig> _assets;

        public T GetAsset<T>(string id) where T : class
        {
            if (_assets == null)
                GenerateAssetList();
            if (!_assets.TryGetValue(id, out var value))
                throw new Exception($"Could not locate asset by id {id}");

            return value as T;
        }

        public List<T> GetAssets<T>() where T : class
        {
            if (_assets == null)
                GenerateAssetList();
            var assets = new List<T>();
            //for (int i = 0; i < _assets.Count; i++)
            foreach (var asset in _assets)
            {
                //var asset = _assets[i];
                if (asset.Value is T)
                    assets.Add(asset.Value as T);
            }
            //if (!_assets.TryGetValue(id, out var value))
            //    throw new Exception($"Could not locate asset by id {id}");

            //return value as T;
            return assets;
        }

        private void GenerateAssetList()
        {
            _assets = new Dictionary<string, RQBaseConfig>();

            for (int i = 0; i < Assets.Count; i++)
            {
                var asset = Assets[i];
                if (asset != null)
                    _assets.Add(asset.UniqueId, asset);
            }

            //_assets = Assets.ToDictionary(i => i.UniqueId);
        }
    }
}
