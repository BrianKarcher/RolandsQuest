using RQ.Common;
using RQ.Common.Controllers;
using RQ.Common.UniqueId;
using RQ.Controller.ManageScene;
using RQ.Controllers;
using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Audio;
using RQ.Model.GameData.StoryProgress;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using System.Collections;
using RQ.Model.ObjectPool;
using UnityEngine;
using UnityEngine.SceneManagement;
using RQ.Audio;

namespace RQ
{
    [AddComponentMenu("RQ/Common/Scene Setup")]
    public class SceneSetup : BaseObject
    {
        public SceneConfig SceneConfig;

        public Transform SpawnPoints = null;

        [SerializeField]
        private PlaySoundInfo _soundInfo = null;

        public SpawnPointComponent[] SpawnPointComponents { get; set; }
        [SerializeField]
        private bool _sceneLoadPerformFadeIn = true;
        [SerializeField]
        private TweenToColorInfo _sceneLoadColorInfo = null;
        [SerializeField]
        private bool _sceneExitPerformFadeOut = true;
        [SerializeField]
        private TweenToColorInfo _sceneExitColorInfo = null;

        [SerializeField]
        private StorySceneConfig _beginGameStoryScene = null;

        [SerializeField]
        private GameController _gameController = null;

        [SerializeField]
        private GameConfig _gameConfig = null;
        public GameConfig GameConfig { get { return _gameConfig; } }

        [SerializeField]
        private bool _autoSaveOnStart = false;
        public bool AutoSaveOnStart { get { return _autoSaveOnStart; } }

        private bool _firstUpdate = true;
        private bool _gameControllerInstantiated = false;

        private static SceneSetup _instance;
        [HideInInspector]
        public static SceneSetup Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<SceneSetup>();
                return _instance;
            }
        }

        //[SerializeField]
        //private DialogueDatabase _dialogueDatabase;
        //public DialogueDatabase DialogueDatabase { get { return _dialogueDatabase; } set { _dialogueDatabase = value; } }

        private Transform _actorsRoot;

        public override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
            {
                // Make sure when the player hits Play in the Editor, it starts at the current scene.
                if (_gameConfig != null)
                    _gameConfig.AutoStartSceneConfig = SceneConfig;
                return;
            }

            if (GameController.Instance == null)
            {
                Debug.LogError("Game Controller not found!");
                //Debug.Log("Loading Persistent Scene since it does not exist.");
                //yield return SceneManager.LoadSceneAsync("Persistent Scene", LoadSceneMode.Additive);
                //_gameControllerInstantiated = true;
                //if (SceneConfig == null)
                //    Debug.LogError("No Scene Config set in SceneSetup");
                ////GameDataController.Instance.CurrentSceneConfig = ConfigsContainer.Instance.GetConfig<SceneConfig>(SceneConfig.UniqueId);
                ////GameDataController.Instance.CurrentSceneConfig = GameDataController.Instance.GetSceneConfig(SceneConfig.UniqueId);
                //GameDataController.Instance.CurrentSceneConfig = SceneConfig;

                //// Loading directly into a scene in the Editor, so skip the Title screen
                ////GameStateController.Instance.AutoStart = true;
                //MessageDispatcher2.Instance.DispatchMsg("AutoStart", 0f, this.UniqueId, "Game Controller", null);
                //// We are starting fresh on this scene, so notify the system we are loading this scene
                ////GameDataController.Instance.NextSceneConfig = SceneConfig;
                //GameController.Instance.AppStart();
                //GameController.Instance.BeginNewGame();
            }
            if (GameDataController.Instance.Data.GetStoryProgress() == null)
                GameDataController.Instance.Data.SetStoryProgress(_beginGameStoryScene, true);

            GameController.Instance.SetSceneSetup(this);
            MessageDispatcher2.Instance.DispatchMsg("ClearSaveRequest", 0f, this.UniqueId, "UI Manager", null);
            //GameController.Instance.SetCamera(_camera);
            GameStateController.Instance.SetCurrentScene(SceneConfig.UniqueId);
            GameStateController.Instance.LoadCurrentSceneData();

            //yield return LoadAllSubscenes();
            // The Actors root can be in a subscene, so load after the sub scenes are loaded
            _actorsRoot = GameObject.Find("Actors").transform;

            Debug.LogWarning("(SceneSetup) Adding all entities to Container");
            GameStateController.Instance.AddAllEntitiesToContainer();

            GameStateController.Instance.InitAllEntities();

            RegisterSpawnPoints();

            //StartCoroutine(ProcessAwake());
        }

        //public IEnumerator ProcessAwake()
        //{

        //    StopCoroutine(ProcessAwake());
        //}

        public IEnumerator LoadAllSubscenes()
        {
            var sceneController = GameObject.FindObjectOfType<SceneController>();

            foreach (var subscene in SceneConfig.SubsceneData)
            {
                // Games being loaded do not load the Sprite sub scene - it would bring in all of the initial sprites
                // in the scene.
                if (GameDataController.Instance.LoadingGame && subscene.SubsceneType == Model.Game_Data.SubsceneType.Sprite)
                    continue;
                if (sceneController.IsSceneLoaded(subscene.Scene.name))
                    continue;
                //sceneController.LoadScene(subscene.Scene.name);
                yield return SceneManager.LoadSceneAsync(subscene.Scene.name, LoadSceneMode.Additive);
            }
        }

        //public void ClearEntities()
        //{
        //    //GameStateController.Instance.AddAllEntitiesToContainer();
        //    Debug.LogWarning("LoadGameEntitiesState - ClearEntities called");
        //    Debug.LogWarning("ClearEntities called, entity count = " + EntityContainer._instance.EntityInstanceMap.Count);
        //    GameStateController.Instance.ClearScene(false);
        //    Debug.LogWarning("ClearEntities finished, entity count = " + EntityContainer._instance.EntityInstanceMap.Count);
        //}

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            if (!GameDataController.Instance.LoadingGame)
            {
                GameController.Instance.PersistenceController.CreateSceneSnapshot();
            }

            if (GameDataController.Instance.LoadingGame)
            {
                //LoadGameEntities();
                try
                {
                    //GameController.Instance.PersistenceController.LoadSnapshot();
                    //GameStateController.Instance.Deserialize();
                    //GameController.Instance.PersistenceController.LoadSnapshot();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
                GameDataController.Instance.LoadingGame = false;
            }

            //GameController.Instance.SetDialogueDatabase(_dialogueDatabase);
            SetupScene();
            GameStateController.Instance.ChangingScene = false;
        }

        public void LevelLoaded()
        {
            // Can only set the active scene after the level is FULLY loaded, which it is not in Awake or Start :(
            Debug.Log($"Setting Active scene to {SceneConfig.SceneName}");
            var scene = SceneManager.GetSceneByPath(SceneConfig.SceneName);
            SceneManager.SetActiveScene(scene);

            // And, can only create objects after the scene is fully loaded to be loaded into that scene.... :(
            var objectPool = GameObject.FindObjectOfType<ObjectPool>();
            objectPool.AddPools(SceneConfig.Pools);
        }

        /// <summary>
        /// The game has been loaded and the level refreshed.
        /// Since the game was just loaded, we want to destroy every sprite in the scene and reload them
        /// from the loaded data to make sure everything starts fresh.
        /// This process also makes debugging easier since every variable in the sprite is reset.
        /// </summary>
        //public void LoadGameEntities()
        //{
        //    Debug.LogWarning("LoadGameEntitiesState - Load Game Entities called");

        //    Debug.Log("Loading game entities - current count" + EntityContainer._instance.EntityInstanceMap.Count);
        //    //EntityController.Instance.CreateEntities(GameStateController.Instance.GameSerializedData.EntityData.Where(
        //    //    i => i.RecreateOnGameLoad), _actorsRoot);
        //    Debug.LogWarning("Loaded game entities - current count" + EntityContainer._instance.EntityInstanceMap.Count);
        //    //GameStateController.Instance.AddAllEntitiesToContainer();
        //    //GameStateController.Instance.InitAllEntities();
        //    //EntityController.Instance.Deserialize(GameStateController.Instance.GameSerializedData.EntityData);
        //}

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!Application.isPlaying)
            {
                UniqueIdRegistry.Clear();
                return;
            }
            ObjectPool.Instance?.DeletePools(SceneConfig.Pools);
        }

        private void SetupScene()
        {
            GameStateController.Instance.ProcessSceneLoad();
            
            PlacePlayersAtSpawnPoints();
            EntityController.Instance.Cleanup();
            var loadingGame = GameDataController.Instance.LoadingGame;
            if (AutoSaveOnStart && !loadingGame)
                ProcessAutoSave();
            var soundInfo = GetSoundInfo();
            if (soundInfo.AudioClip != null)
                GameController.Instance.GetMusicTrack().PlayAudioClip(soundInfo);
            //MessageDispatcher2.Instance.DispatchMsg("PlaySoundInfo", 0f, this.UniqueId,
            //    "Game Controller"/*_audioComponent.UniqueId*/, soundInfo);
            MessageDispatcher2.Instance.DispatchMsg("Complete", 0f, this.UniqueId, "Game Controller", null);
            bool goToCutscene = false;
            if (GameController.Instance.ActionController != null)
            {
                GameController.Instance.ActionController.RunStartSceneActions();
                if (GameController.Instance.ActionController.CheckAndRunActionSequences("Start"))
                    goToCutscene = true;
            }
        }

        private void ProcessAutoSave()
        {
            try
            {
                var fileName = GameController.Instance.AutoSaveFileName;
                GameStateController.Instance.SaveGame(fileName);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private void PlacePlayersAtSpawnPoints()
        {
            if (!String.IsNullOrEmpty(GameDataController.Instance.Data.SpawnpointUniqueId))
            {
                var sceneSetup = GameController.Instance.GetSceneSetup();

                //for (int i = 0; i < sceneSetup.SpawnPointComponents.Count; i++)
                //{
                    
                //}
                //var spawnPoints = sceneSetup.SpawnPointComponents.Where(i =>
                    //i.SpawnPointUniqueId == GameDataController.Instance.Data.SpawnpointUniqueId);

                PlaceEntityAtSpawnPoint(sceneSetup.SpawnPointComponents, Enums.EntityType.Player);
                PlaceEntityAtSpawnPoint(sceneSetup.SpawnPointComponents, Enums.EntityType.Companion);
                GameDataController.Instance.Data.SpawnpointUniqueId = null;
            }
        }

        private void PlaceEntityAtSpawnPoint(SpawnPointComponent[] spawnPoints, Enums.EntityType entityType)
        {
            SpawnPointComponent spawnPoint = null;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].SpawnPointUniqueId == GameDataController.Instance.Data.SpawnpointUniqueId 
                    && spawnPoints[i].EntityType == entityType)
                {
                    spawnPoint = spawnPoints[i];
                    break;
                }
            }

            //var spawnPoint = spawnPoints.FirstOrDefault(i => i.EntityType == entityType);
            if (spawnPoint == null)
            {
                Debug.LogError($"(PlaceEntityAtSpawnPoint) Could not locate spawn point for {entityType}");
                return;
            }

            var pos = spawnPoint.transform.position;

            var locatedEntity = EntityController.Instance.GetFirstEntityByType(entityType);
            if (locatedEntity == null)
            {
                Debug.LogWarning($"(PlaceEntityAtSpawnPoint) Could not locate entity {entityType}");
                return;
            }

            string uniqueId = locatedEntity.UniqueId;

            // Place the main character at the spawn point
            //var spawnPoint = GameData.Instance.CurrentScene.SpawnPoints[GameData.Instance.SpawnpointUniqueId];
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                uniqueId, Enums.Telegrams.SetPos,
                new Vector2D(pos.x, pos.y));
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, uniqueId,
                Enums.Telegrams.SetLevelHeight, spawnPoint.LevelLayer);


        }

        public void RegisterSpawnPoints()
        {
            if (SpawnPoints == null)
                return;

            SpawnPointComponents = SpawnPoints.GetComponentsInChildren<SpawnPointComponent>();

            //if (spawnPointComponents != null)
            //{
            //    SpawnPointComponents.AddRange(spawnPointComponents);
            //}
        }

        public bool GetSceneLoadPerformFadeIn()
        {
            return _sceneLoadPerformFadeIn;
        }

        public bool GetSceneExitPerformFadeOut()
        {
            return _sceneExitPerformFadeOut;
        }

        public TweenToColorInfo GetSceneLoadColorInfo()
        {
            return _sceneLoadColorInfo;
        }

        public TweenToColorInfo GetSceneExitColorInfo()
        {
            return _sceneExitColorInfo;
        }

        public StorySceneConfig GetBeginGameStoryScene()
        {
            return _beginGameStoryScene;
        }

        public Transform GetActorsRoot()
        {
            return _actorsRoot;
        }

        public AudioClipInfo GetSoundInfo()
        {
            if (_soundInfo.AudioClips.Count == 0)
                return new AudioClipInfo();

            var currentClipIndex = UnityEngine.Random.Range(0, _soundInfo.AudioClips.Count - 1);
            var soundInfo = new AudioClipInfo()
            {
                AudioClip = _soundInfo.AudioClips[currentClipIndex],
                Loop = _soundInfo.LoopSound,
                ForcePlay = _soundInfo.PlaySound,
                //PlaySound = _soundInfo.PlaySound,
                //PlayOnMusicTrack = _soundInfo.PlayOnMusicTrack,
                //PlayAsOneShot = _soundInfo.PlayAsOneShot,
                Volume = _soundInfo.Volume,
                //LoopSound = _soundInfo.LoopSound
            };
            return soundInfo;
        }
    }
}
