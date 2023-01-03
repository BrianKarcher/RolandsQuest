using RQ.Controller.ManageScene;
using RQ.Model;
using RQ.Model.Enums;
using RQ.Model.Game_Data;
using RQ.Model.Game_Data.Quest;
using RQ.Model.GameData.StoryProgress;
using RQ.Model.Item;
using RQ.Model.Serialization;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Common.Controllers
{
    //[Serializable]
    public class GameDataController
    {
        public static readonly GameDataController Instance = new GameDataController();

        private GameConfig _gameConfig;
        private Dictionary<string, StorySceneConfig> _storySceneConfigs { get; set; }
        
        public SceneSnapshot SceneSnapshot { get; set; }
        public GameData Data { get; set; }
        //public string NextSceneUniqueId { get; set; }
        //public string NextSceneName { get; set; }
        public SceneConfig NextSceneConfig { get; set; }
        public SceneConfig CurrentSceneConfig { get; set; }
        public IItemConfig CurrentBlueprint { get; set; }
        public IItemConfig CurrentMold { get; set; }

        [HideInInspector]
        public bool LoadingGame { get; set; }
        //[HideInInspector]
        //public bool DestoyEntitiesOnAwake { get; set; }
        public EntitySerializedData LoadingEntity { get; set; }
        public IEntity LastUsedEntity { get; set; }
        public SceneConfig AppStartScene { get; set; }
        public ICameraClass Camera { get; set; }

        private GameDataController()
        {            
            _storySceneConfigs = new Dictionary<string, StorySceneConfig>();
        }

        public void AddSceneToDeathPersistence(string sceneUniqueId)
        {
            // If scene is already in the list
            // Don't you just love linked lists?
            var currentNode = Data.SceneDeathDatas.First;
            SceneDeathData scene = null;
            while (currentNode != null)
            {
                if (currentNode.Value.SceneUniqueId == sceneUniqueId)
                {
                    scene = currentNode.Value;
                    break;
                }
                currentNode = currentNode.Next;
            }
            //var scene = Data.SceneDeathDatas.FirstOrDefault(i => i.SceneUniqueId == sceneUniqueId);
            if (scene != null)
            {
                // Remove it
                Data.SceneDeathDatas.Remove(scene);
            }
            else
            {
                // Create a new entry
                scene = new SceneDeathData();
                scene.SceneUniqueId = sceneUniqueId;
                scene.DeadEntities = new List<string>();
            }

            // Add the scene to the end of the list
            Data.SceneDeathDatas.AddLast(scene);

            // If the threshhold has been passed, remove the first entry
            while (Data.SceneDeathDatas.Count > 5)
            {
                Data.SceneDeathDatas.RemoveFirst();
            }
        }

        /// <summary>
        /// Starts a new game from the supplied Config
        /// </summary>
        /// <param name="gameConfig"></param>
        public void LoadFromConfig(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            var areaDatas = LoadAreaConfigs(gameConfig.AreaConfigs);
            Data.SceneDatas = new Dictionary<string, SceneData>();
            foreach (var areaData in areaDatas)
            {
                foreach (var sceneData in areaData.SceneDatas)
                {
                    Data.SceneDatas[sceneData.Key] = sceneData.Value.Clone();
                }
            }

            Data.QuestDatas = LoadQuestConfigs(gameConfig.GetAssets<QuestConfig>());

            Data.GlobalVariables = LoadVariables(gameConfig.Variables);
        }

        public void LoadStoryConfigs(GameConfig gameConfig)
        {
            _storySceneConfigs.Clear();
            foreach (var actConfig in gameConfig.ActConfigs)
                foreach (var chapterConfig in actConfig.ChapterConfigs)
                {
                    chapterConfig.SceneConfigs.ForEach(i => _storySceneConfigs.Add(i.UniqueId, i));
                }
        }

        private List<AreaData> LoadAreaConfigs(List<AreaConfig> areaConfigs)
        {
            var areaDatas = new List<AreaData>();
            //if (Data.AreaDatas == null)
            //Data.AreaDatas = new Dictionary<WorldAreas, AreaData>();
            //Data.AreaDatas.Clear();

            for (int i = 0; i < areaConfigs.Count; i++)
            {
                var areaConfig = areaConfigs[i];
                var areaData = LoadFromAreaConfig(areaConfig);
                areaDatas.Add(areaData);
            }
            return areaDatas;
        }

        private Dictionary<string, QuestData> LoadQuestConfigs(List<QuestConfig> questConfigs)
        {
            var questDatas = new Dictionary<string, QuestData>();
            //var questDatas = new List<QuestData>();
            for (int i = 0; i < questConfigs.Count; i++)
            {
                var questConfig = questConfigs[i];
                var questData = new QuestData();
                questData.QuestEntryConfigUniqueId = questConfig.UniqueId;
                questData.QuestStatus = QuestStatus.Unassigned;

                questData.QuestEntryDatas = new List<QuestEntryData>();
                for (int k = 0; k < questConfig.QuestEntries.Count; k++)
                {
                    var questEntryConfig = questConfig.QuestEntries[k];
                    var questEntry = new QuestEntryData()
                    {
                        Id = questEntryConfig.Id,
                        Status = QuestStatus.Unassigned,
                        QuestEntryConfigUniqueId = questEntryConfig.UniqueId
                    };
                    questData.QuestEntryDatas.Add(questEntry);
                }
                questDatas.Add(questConfig.UniqueId, questData);
            }
            return questDatas;
        }

        private Dictionary<string, Variable> LoadVariables(List<Variable> variables)
        {
            var newVariables = new Dictionary<string, Variable>();
            for (int i = 0; i < variables.Count; i++)
            {
                var variable = variables[i];
                newVariables.Add(variable.UniqueId, variable.Clone());
            }
            return newVariables;
        }

        public SceneData GetSceneData(string sceneConfigUniqueId)
        {
            if (!Data.SceneDatas.TryGetValue(sceneConfigUniqueId, out var sceneData))
            {
                Debug.LogError($"Could not locate scene {sceneConfigUniqueId} in Scene Data");
                return null;
            }

            return sceneData;            
        }

        public void SetCurrentSceneData()
        {
            Data.CurrentScene = GetSceneData(CurrentSceneConfig.UniqueId);
            //CurrentSceneConfig = ConfigsContainer.Instance.GetConfig<SceneConfig>(sceneConfigUniqueId);
        }

        public SceneConfig GetSceneConfig(string sceneConfigUniqueId)
        {
            var sceneConfig = _gameConfig.GetAsset<SceneConfig>(sceneConfigUniqueId);
            if (sceneConfig == null)
                Debug.LogError("Could not locate asset " + sceneConfigUniqueId);
            return sceneConfig;
            //return ConfigsContainer.Instance.GetConfig<SceneConfig>(sceneConfigUniqueId);
            //return _sceneConfigs[sceneConfigUniqueId];
        }

        public StorySceneConfig GetStorySceneConfig(string storySceneConfigUniqueId)
        {
            return _storySceneConfigs[storySceneConfigUniqueId];
        }

        //public LayerMask GetLayerMask(Enum.LevelLayer levelLayer)
        //{
        //    if (levelLayer == Enum.LevelLayer.LevelOne)
        //        return _gameConfig.LevelOneLayerMask;
        //    else if (levelLayer == Enum.LevelLayer.LevelTwo)
        //        return _gameConfig.LevelTwoLayerMask;
        //    else
        //        return _gameConfig.SharedLayerMask;
        //}

        //public WorldAreas GetWorldArea()
        //{
        //    //if (_areaConfig == null)
        //    //    return WorldAreas.None;

        //    return _areaConfig.Areas;
        //}

        public AreaData LoadFromAreaConfig(AreaConfig areaConfig)
        {
            AreaData areaData = new AreaData();
            areaData.AreaConfigUniqueId = areaConfig.UniqueId;
            //Name = areaConfig.Name;
            //Areas = areaConfig.Areas;
            areaData.SceneDatas = LoadSceneData(areaConfig.Scenes);
            return areaData;
        }

        private Dictionary<string, SceneData> LoadSceneData(List<SceneConfig> sceneConfigs)
        {
            var sceneDatas = new Dictionary<string, SceneData>();
            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                var sceneConfig = sceneConfigs[i];
                var sceneData = LoadFromSceneConfig(sceneConfig);
                sceneDatas.Add(sceneConfig.UniqueId, sceneData);
            }
            return sceneDatas;
        }

        public SceneData LoadFromSceneConfig(SceneConfig sceneConfig)
        {
            SceneData sceneData = new SceneData();
            sceneData.SceneConfigUniqueId = sceneConfig.UniqueId;
            //Id = sceneConfig.GetInstanceID();
            //Name = sceneConfig.SceneName;
            sceneData.LevelOneZIndex = sceneConfig.LevelOneZIndex;
            sceneData.LevelTwoZIndex = sceneConfig.LevelTwoZIndex;
            sceneData.LevelThreeZIndex = sceneConfig.LevelThreeZIndex;
            sceneData.Variables = LoadVariables(sceneConfig.Variables);
            //LoadSpawnPoints(sceneConfig.SpawnPoints);
            return sceneData;
        }

        public string GetVariable(VariableType type, string variableUniqueId)
        {   
            switch (type)
            {
                case VariableType.Global:
                    if (!GameDataController.Instance.Data.GlobalVariables.TryGetValue(variableUniqueId, out var variable))
                        throw new Exception("Could not locate variable " + variableUniqueId);
                    return variable.Value;
                default:
                    //if (!GameDataController.Instance.Data.CurrentScene.Variables.ContainsKey(variableUniqueId))
                    //    return null;
                    return GameDataController.Instance.Data.CurrentScene.Variables[variableUniqueId].Value;
            }
        }

        //public string GetVariable(VariableType type, Guid variableUniqueId)
        //{
        //    switch (type)
        //    {
        //        case VariableType.Global:
        //            return GameDataController.Instance.Data.Variables[variable].Data;
        //        default:
        //            return GameDataController.Instance.Data.CurrentScene.Variables[variable].Data;
        //    }
        //}

        public void SetVariable(VariableType type, string variable, string value)
        {
            switch (type)
            {
                case VariableType.Global:
                    GameDataController.Instance.Data.GlobalVariables[variable].Value = value;
                    break;
                default:
                    GameDataController.Instance.Data.CurrentScene.Variables[variable].Value = value;
                    break;
            }
        }

        public void CreateNewGameData()
        {
            Data = new GameData();
        }

        public void DeleteAllGameData()
        {
            Data = null;
        }

        public GameConfig GetGameConfig()
        {
            return _gameConfig;
        }

        //private void LoadVariables(List<Variable> variables)
        //{
        //    Variables = new Dictionary<string, Variable>();
        //    for (int i = 0; i < variables.Count; i++)
        //    {
        //        var variable = variables[i];
        //        Variables.Add(variable.Name, variable.Clone());
        //    }
        //}

        //public SceneConfig GetSceneConfig()
        //{
        //    return _sceneConfig;
        //}

        //private void LoadSpawnPoints(PointsUsingSpawnpoint spawnPoints)
        //{
        //    SpawnPoints = new Dictionary<string, SpawnPoint>();

        //    for (int i = 0; i < spawnPoints.Count; i++)
        //    {
        //        var spawnPoint = spawnPoints[i].Clone();
        //        //SpawnPoints.Add(spawnPoint.Name, spawnPoint);
        //        SpawnPoints.Add(spawnPoint.UniqueId, spawnPoint);
        //    }
        //}
    }
}
