using RQ.Common;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.Actions;
using RQ.Controller.ManageScene;
using RQ.Controllers;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Game_Data;
using RQ.Model.GameData.StoryProgress;
using RQ.Model.Messaging;
using RQ.Model.Serialization;
using RQ.Model.Serialization.Input;
using RQ.Physics.Components;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ
{
    //[ExecuteInEditMode]
    // We are keeping this a MonoBehavior because it not only keeps this alive, but it keeps alive
    // everything within this gameObject, such as the AudioSource object so the sounds and music don't stop or skip
    // between scene loads
    [AddComponentMenu("RQ/Manager/Game State Controller")]
    public class GameStateController : ComponentRepository, IMessagingObject, IGameStateController
    {
        private string _filePath;
        private GamePrefsData _gamePrefsData;
        public bool IsCrafted { get; set; }
        //private bool _isCrafted = false;

        public override string UniqueId { get { return "Game State Controller"; } set { } }

        [HideInInspector]
        private GameSerializedData _gameSerializedData;

        [SerializeField]
        private SceneConfig _firstScene;
        [SerializeField]
        private StorySceneConfig _beginningStorySceneConfig;
        [SerializeField]
        private SceneController _sceneController;
        /// <summary>
        /// Used for reassigning input keys
        /// </summary>
        public InputCommand CurrentSelectedInputCommand { get; set; }
        //public StorySceneConfig 

        public GameSerializedData GameSerializedData { get { return _gameSerializedData; } set => _gameSerializedData = value; }

        /// <summary>
        /// Temporary storage ground between loading a game and it actually being fully loaded
        /// </summary>

        public bool ChangingScene { get; set; }
        public bool PlayCutscene { get; set; }
        public bool AutoStart { get; set; }

        private long GetGamePrefsDataId;

        private static GameStateController _instance;
        [HideInInspector]
        public static GameStateController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<GameStateController>();
                return _instance;
            }
        }

        //public string LoadGameFileName { get; set; }



        public override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying)
                return;

            var unityPath = GetUnityDataPath();
            _filePath = unityPath + "\\GamePrefs.json";

            LoadGamePrefsFromFile();
            
            //InputManager.Instance.SetInputs(_gamePrefsData.InputCommands);
            var gameConfig = GameController.Instance.GameConfig;

            EntityController.Instance.CreateEntityTransforms(gameConfig.EntityPrefabs);

            GameDataController.Instance.LoadingGame = false;
            ChangingScene = false;
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            // Broadcast to all AudioComponents
            MessageDispatcher2.Instance.DispatchMsg("UpdateVolume", 0f, this.UniqueId, null, null);
        }

        public override void StartListening()
        {
            base.StartListening();
            GetGamePrefsDataId = MessageDispatcher2.Instance.StartListening("GetGamePrefsData", this.UniqueId, (data) =>
            {
                var action = data.ExtraInfo as Action<object>;
                action(_gamePrefsData);
            });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("GetGamePrefsData", this.UniqueId, GetGamePrefsDataId);
        }

        private void LoadGamePrefsFromFile()
        {
            _gamePrefsData = new GamePrefsData();
            var hasMusicVolume = PlayerPrefs.HasKey("MusicVolume");
            if (!hasMusicVolume)
            {
                // Default to 30%
                _gamePrefsData.MusicVolume = 0.3f;
            }
            else
            {
                _gamePrefsData.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            }
            var hasSoundEffectVolume = PlayerPrefs.HasKey("SoundEffectVolume");
            if (!hasSoundEffectVolume)
            {
                // Default to 30%
                _gamePrefsData.SoundEffectVolume = 0.3f;
            }
            else
            {
                _gamePrefsData.SoundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume");
            }

            //if (File.Exists(_filePath))
            //{
            //    var fileData = File.ReadAllText(_filePath);
            //    _gamePrefsData = JsonSerializer.Deserialize<GamePrefsData>(fileData);
            //}
            //else
            //{
            //    _gamePrefsData = CreateDefaultPrefs();
            //}
        }

        public void SaveGamePrefsToFile()
        {
            PlayerPrefs.SetFloat("MusicVolume", _gamePrefsData.MusicVolume);
            PlayerPrefs.SetFloat("SoundEffectVolume", _gamePrefsData.SoundEffectVolume);
            PlayerPrefs.Save();
            //var gamePrefsSerialized = JsonSerializer.Serialize(_gamePrefsData);
            //using (var sw = new StreamWriter(_filePath))
            //{
            //    sw.Write(gamePrefsSerialized);
            //    sw.Close();
            //}
        }

        public GamePrefsData GetGamePrefs()
        {
            return _gamePrefsData;
        }

        private GamePrefsData CreateDefaultPrefs()
        {
            var gamePrefsData = new GamePrefsData();
            //gamePrefsData.InputCommands = InputManager.Instance.GetDefaultInputs();
            //gamePrefsData.ScreenWidth = 864;
            //gamePrefsData.ScreenHeight = 648;
            //gamePrefsData.IsFullScreen = false;
            //gamePrefsData.KeyboardInputCommands = InputManager.Instance.GetDefaultKeyboardInputs();

            //UnityEngine.Input.
            return gamePrefsData;
        }

        /// <summary>
        /// Loads the data for a new game
        /// </summary>
        public void NewGame()
        {
            GameDataController.Instance.CreateNewGameData();
        }

        public void StartInit()
        {
            GameDataController.Instance.LoadFromConfig(GameController.Instance.GameConfig);
            GameDataController.Instance.Data.SetStoryProgress(null, true);            
        }

        public void SaveGame(string fileName)
        {
            var gameData = GameController.Instance.PersistenceController.CreateGameSerializedData();

            Persistence.SaveGame(fileName, gameData);
        }

        public void LoadGame(string fileName)
        {
            if (GameDataController.Instance.LoadingGame)
                return;

            GameController.Instance.PersistenceController.LoadGame(fileName);
        }

        public override bool HandleMessage(Telegram msg)
        {
            base.HandleMessage(msg);

            switch (msg.Msg)
            {
                case Telegrams.ChangeSceneRequest:
                    var changeSceneRequestData = msg.ExtraInfo as ChangeSceneRequestData;
                    ChangeScene(changeSceneRequestData.SceneConfigUniqueId,
                        changeSceneRequestData.SpawnPointUniqueId);
                    break;
            }

            return false;
        }

        public void LoadScene(string sceneName)
        {
            ChangingScene = true;
            // Log the next Spawnpoint before ClearScene deletes it
            if (GameDataController.Instance.Data != null)
            {
                GameDataController.Instance.Data.SpawnpointUniqueId =
                    GameDataController.Instance.Data.NextSpawnpointUniqueId;
            }
            //ClearScene(false);

            //MessageDispatcher.Instance.RemoveByEarlyTermination(Enums.TelegramEarlyTermination.ChangeScenes);
            //Debug.Log("LoadLevel being called");
            //Debug.Log("Loading scene, entity count = " + EntityContainer._instance.EntityInstanceMap.Count);
            //Application.LoadLevel(sceneName);

            _sceneController.FadeAndLoadScene(sceneName);
            //Debug.Log("(GameStateController) FadeAndLoadScene called");
           

        }

        /// <summary>
        /// Adds all entities, disabled or not, to the Container
        /// </summary>
        private List<IComponentRepository> tempEntityList = new List<IComponentRepository>(5);
        public void AddAllEntitiesToContainer()
        {
            //var entities = GameObject.FindObjectsOfType<SpriteBaseComponent>().Cast<IComponentRepository>();
            var actorsRoot = GameController.Instance.GetSceneSetup().GetActorsRoot();
            // Reusing this list as to avoid unnessesary allocations
            tempEntityList.Clear();
            actorsRoot.GetComponentsInChildren<IComponentRepository>(true, tempEntityList);
            foreach (var entity in tempEntityList)
            {
                EntityContainer._instance.AddEntity(entity);
            }
            //var entities = actorsRoot.GetComponentsInChildren<IComponentRepository>(true).Cast<IEntity>();
            //entities = entities.Where(i => i.AddToEntityContainer);
            //EntityContainer._instance.AddEntities(entities);
        }

        public void InitAllEntities()
        {
            //Debug.Log(this.name + " - InitAllEntities called");
            var actorsRoot = GameController.Instance.GetSceneSetup().GetActorsRoot();
            var entities = actorsRoot.GetComponentsInChildren<IComponentRepository>(true);
            foreach (var entity in entities)
            {
                try
                {
                    if (!entity.isActiveAndEnabled)
                        entity.Init();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        //public void ClearScene(bool destroyAllEntities)
        //{
        //    Debug.Log("EntityController.ClearScene called");
        //    if (destroyAllEntities)
        //        EntityController.Instance.DestroyAllEntities();
        //    else
        //        EntityController.Instance.DestroyReceatedEntities();
        //    EntityController.Instance.Cleanup();
        //    //Cleanup();
        //    EntityContainer._instance.SetMainCharacter(null);
        //    EntityContainer._instance.SetCompanionCharacter(null);
        //    if (GameDataController.Instance.Data != null)
        //    {
        //        GameDataController.Instance.Data.NextSpawnpointUniqueId = null;
        //        GameDataController.Instance.Data.UsableContainer.ClearList();
        //        GameDataController.Instance.Data.UsableContainer.SetCurrentUsable(null);
        //    }

        //    //InputManager.Instance.RemoveAllEntities();
        //    // Each scene has its own Action Controller, make sure we don't use one from a previous scene in the new scene
        //    GameController.Instance.ActionController = null;
        //}



        public void ProcessSceneLoad()
        {
            var data = GameDataController.Instance.Data;
            SetCurrentScene(data.CurrentSceneUniqueId);
            GameDataController.Instance.AddSceneToDeathPersistence(data.CurrentSceneUniqueId);
            GameController.Instance.ActionController = GameObject.FindObjectOfType<ActionController>();
            //_isChangingScenes = false;
            //GameData.Instance.NextSceneName = string.Empty;
            //GameData.Instance.NextSceneUniqueId = string.Empty;
            //GameData.Instance.NextSpawnpointUniqueId = string.Empty;
            //LoadingScene = true;
        }

        public void SetCurrentScene(string sceneConfigUniqueId)
        {
            SetCurrentScene(GameDataController.Instance.GetGameConfig().GetAsset<SceneConfig>(sceneConfigUniqueId));
            //SetCurrentScene(ConfigsContainer.Instance.GetConfig<SceneConfig>(sceneConfigUniqueId));
        }

        public void SetCurrentScene(SceneConfig sceneConfig)
        {
            GameDataController.Instance.Data.CurrentSceneUniqueId = sceneConfig.UniqueId; // sceneSetup.SceneConfig.UniqueId;
            GameDataController.Instance.CurrentSceneConfig = sceneConfig;
        }

        public void LoadCurrentSceneData()
        {
            GameDataController.Instance.SetCurrentSceneData();
        }

        public void ChangeScene(string sceneConfigUniqueId, string spawnpointUniqueId)
        {
            var sceneConfig = GameDataController.Instance.GetSceneConfig(sceneConfigUniqueId);
            ChangeScene(sceneConfig);
        }

        public void ChangeScene(SceneConfig sceneConfig)
        {
            BeginChangeScene(sceneConfig, string.Empty);
        }

        public void ChangeScene(SceneConfig sceneConfig, string spawnpointUniqueId)
        {
            BeginChangeScene(sceneConfig, spawnpointUniqueId);
        }

        private void BeginChangeScene(SceneConfig sceneConfig, string spawnpointUniqueId)
        {
            if (ChangingScene)
                return;
            
            GameDataController.Instance.NextSceneConfig = sceneConfig;
            RecordDataForSceneChange(spawnpointUniqueId);
            //EntityController.Instance.SendMessageToAllEntities(0f, this.UniqueId,
            //    Telegrams.ChangeScene, null);
            MessageDispatcher2.Instance.DispatchMsg("ChangeScene", 0f, this.UniqueId, "Game Controller", null);
        }

        public void RecordDataForSceneChange(string spawnpointUniqueId)
        {
            var data = GetGameData();
            Debug.Log($"Setting next spawn point to {spawnpointUniqueId}");
            data.NextSpawnpointUniqueId = spawnpointUniqueId;
            var mainCharacter = EntityContainer._instance.GetMainCharacter();            
            if (mainCharacter != null)
            {
                // Record the player stats to be used for the next scene load, as well as the snapshot to come
                var entityStats = mainCharacter.Components.GetComponent<EntityStatsComponent>().GetEntityStats();
                GameDataController.Instance.Data.CurrentEntityStats = entityStats.Clone();
            }
        }

        public GameData GetGameData()
        {
            return GameDataController.Instance.Data;
        }

        public SceneController GetSceneController()
        {
            return _sceneController;
        }

        private static string GetUnityDataPath()
        {
            return Application.persistentDataPath;
        }
    }
}
