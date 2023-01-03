using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Messaging;
using RQ.Model.Serialization;
using RQ.Physics.Components;
using RQ.Serialization;
using System;
using System.Collections.Generic;

namespace RQ.Controller.ManageScene
{
    public class PersistenceController
    {
        public void CreateSceneSnapshot()
        {
            var sceneSnapshot = new SceneSnapshot();
            var mainCharacter = EntityContainer._instance.GetMainCharacter();
            // When the game is loading set the stats as they were when the player entered the scene
            //var mainCharacter = EntityContainer._instance.GetMainCharacter();
            if (mainCharacter != null)
            {
                var entityStats = mainCharacter.Components.GetComponent<EntityStatsComponent>().GetEntityStats();
                // CurrentEntityStats = null is possible new game start, in that case get the stats from
                // the player object
                if (GameDataController.Instance.Data.CurrentEntityStats == null)
                    sceneSnapshot.EntityStats = entityStats;
                else
                    sceneSnapshot.EntityStats = GameDataController.Instance.Data.CurrentEntityStats.Clone();

            }
            
            sceneSnapshot.SpawnpointUniqueId = GameDataController.Instance.Data.NextSpawnpointUniqueId;
            sceneSnapshot.SceneConfigUniqueId = GameDataController.Instance.Data.CurrentSceneUniqueId;
            sceneSnapshot.SelectedSkill = GameDataController.Instance.Data.SelectedSkill;
            sceneSnapshot.GlobalVariables = new Dictionary<string, Model.Variable>();
            foreach (var variable in GameDataController.Instance.Data.GlobalVariables)
            {
                sceneSnapshot.GlobalVariables[variable.Key] = variable.Value.Clone();
            }
            sceneSnapshot.Inventory = GameDataController.Instance.Data.Inventory.Clone();

            sceneSnapshot.SceneDeathDatas = new LinkedList<Model.SceneDeathData>();
            foreach (var sceneDeathData in GameDataController.Instance.Data.SceneDeathDatas)
            {
                sceneSnapshot.SceneDeathDatas.AddLast(sceneDeathData.Clone());
            }

            sceneSnapshot.Scenes = new Dictionary<string, SceneData>();
            foreach (var sceneData in GameDataController.Instance.Data.SceneDatas)
            {
                sceneSnapshot.Scenes[sceneData.Key] = sceneData.Value.Clone();
            }            

            GameDataController.Instance.SceneSnapshot = sceneSnapshot;
            //return sceneSnapshot;
        }

        public void LoadSnapshot(SceneSnapshot snapshot)
        {
            var mainCharacter = EntityContainer._instance.GetMainCharacter();
            // When the game is loading set the stats as they were when the player entered the scene
            GameDataController.Instance.Data.CurrentEntityStats = snapshot.EntityStats.Clone();
            GameDataController.Instance.Data.NextSpawnpointUniqueId = snapshot.SpawnpointUniqueId;
            GameDataController.Instance.Data.CurrentSceneUniqueId = snapshot.SceneConfigUniqueId;
            GameDataController.Instance.NextSceneConfig = GameDataController.Instance.GetSceneConfig(snapshot.SceneConfigUniqueId);
            GameDataController.Instance.Data.SelectedSkill = snapshot.SelectedSkill;
            GameDataController.Instance.Data.GlobalVariables = new Dictionary<string, Model.Variable>();
            foreach (var variable in snapshot.GlobalVariables)
            {
                GameDataController.Instance.Data.GlobalVariables[variable.Key] = variable.Value.Clone();
            }
            GameDataController.Instance.Data.Inventory = snapshot.Inventory.Clone();

            GameDataController.Instance.Data.SceneDeathDatas = new LinkedList<Model.SceneDeathData>();
            foreach (var sceneDeathData in snapshot.SceneDeathDatas)
            {
                GameDataController.Instance.Data.SceneDeathDatas.AddLast(sceneDeathData.Clone());
            }

            GameDataController.Instance.Data.SceneDatas = new Dictionary<string, SceneData>();
            foreach (var sceneData in snapshot.Scenes)
            {
                GameDataController.Instance.Data.SceneDatas[sceneData.Key] = sceneData.Value.Clone();
            }

            //foreach (var area in GameDataController.Instance.Data.AreaDatas)
            //{
            //    foreach (var scene in area.Value.SceneDatas)
            //    {
            //        if (snapshot.SceneVariables.TryGetValue(scene.Key, out var sceneData))
            //            scene.Value.Variables = sceneData..Select(i => i.Clone).ToList();
            //    }
            //}
        }

        //public Dictionary<string, List<Variable>> SceneVariables { get; set; }

        public void LoadGame(string fileName)
        {
            if (GameDataController.Instance.LoadingGame)
                return;

            GameDataController.Instance.LoadingGame = true;
            GameSerializedData gameData = Persistence.LoadGame(fileName);
            //PersistentDataManager.Record();
            //ClearScene(true);
            //Debug.LogWarning("Loading level '" + gameData.GameData.CurrentSceneString + "', entity count = " + EntityContainer._instance.EntityInstanceMap.Count);
            //GameDataController.Instance.Data = GameStateController.Instance.GetGameData();

            // TODO Create special serialization logic for this, we don't need to record so many variables.
            //GameDataController.Instance.Data = gameData.GameData;
            MessageDispatcher2.Instance.DispatchMsg("Deserialize", 0f, string.Empty, "Inventory Controller", null);
            LoadSnapshot(gameData.SceneSnapshot);
            //GameStateController.Instance.LoadScene(gameData.GameData.CurrentSceneString);

            // Loads the Lua script into the Dialogue System
            //PersistentDataManager.ApplySaveData(gameData.DialogueSystemData);
            // Store the data so we can use it next frame
            GameStateController.Instance.GameSerializedData = gameData;
        }

        public GameSerializedData CreateGameSerializedData()
        {
            GameSerializedData gameData = new GameSerializedData();
            gameData.SaveDate = DateTime.Now;
            gameData.SceneSnapshot = GameDataController.Instance.SceneSnapshot;
            //gameData.GameData = GameDataController.Instance.Data;
            //SerializeAudio(gameData);
            //gameData.DialogueSystemData = PersistentDataManager.GetSaveData();
            //gameData.EntityData = EntityController.Instance.Serialize();
            //gameData.MessageData = MessageDispatcher.Instance.Serialize();
            //gameData.MessageData2 = MessageDispatcher2.Instance.Serialize();
            //GameDataController.Instance.Data.UsableContainer.Serialize();
            return gameData;
        }
    }
}
