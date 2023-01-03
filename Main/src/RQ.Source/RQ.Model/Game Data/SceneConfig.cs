using RQ.Common.Config;
using RQ.Model;
using RQ.Model.Game_Data;
using RQ.Model.Item;
using System;
using System.Collections.Generic;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Controller.ManageScene
{
    [Serializable]
    public class SceneConfig : RQBaseConfig
    {
        /// <summary>
        /// Pointer to the Scene file (.unity)
        /// </summary>
        public UnityEngine.Object Scene;

        [HideInInspector]
        public string SceneName;

        public SubsceneData[] SubsceneData;

        [HideInInspector]
        public List<Variable> Variables;

        [SerializeField]
        public float LevelOneZIndex;

        [SerializeField]
        public float LevelTwoZIndex = -1.0f;

        [SerializeField]
        public float LevelThreeZIndex = -2.0f;

        [SerializeField]
        public StartingItem[] StartingItems;

        public List<ObjectPool.Pool> Pools;

        //public EntityUIBase PersistedEntities;

        //[HideInInspector]
        //public Points<SpawnPoint> SpawnPoints;
        //public PointsUsingSpawnpoint SpawnPoints;
        public List<SpawnPointInAsset> SpawnPoints = new List<SpawnPointInAsset>();

        //public Points2Int SpawnPoints2;

        //public string GetPathToScene()
        //{
        //    string pathToScene = AssetDatabase.GetAssetPath(Scene);
        //    return pathToScene;

        //    //EditorBuildSettingsScene
        //}
    }
}
