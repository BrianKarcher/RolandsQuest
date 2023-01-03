using System.Collections.Generic;
using System.Linq;
using RQ.Audio;
using RQ.Common;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Controller.Actions;
using RQ.Controller.ManageScene;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.GameData.StoryProgress;
using RQ.Render;
using UnityEngine;
using RQ.Entity.Common;
using RQ.Model;
using RQ.Model.Item;
using RQ.Controller.UI;
using RQ.Common.UI;
using RQ.Logging;
using RQ.Model.Audio;
using RQ.Model.ObjectPool;

namespace RQ
{
    // We are keeping this a MonoBehavior because it not only keeps this alive, but it keeps alive
    // everything within this gameObject, such as the AudioSource object so the sounds and music don't stop or skip
    // between scene loads
    public class GameController : ComponentRepository, IMessagingObject//, IStateEntity
    {
        [SerializeField]
        private string _name;
        public string Name { get { return _name; } set { _name = value; } }
        public override string UniqueId { get { return "Game Controller"; } set { } }
        //private DialogueDatabase _dialogueDatabase;
        [SerializeField]
        private GraphicsEngine _graphicsEngine;
        public bool StartInFullScreen = false;
        private CameraClass _camera;
        public GameObject goUIManager;
        public IUIManager UIManager;
        public GameConfig GameConfig;
        public string AutoSaveFileName;
        public PersistenceController PersistenceController { get; private set; }

        [SerializeField]
        private AudioComponent _musicTrack;

        [SerializeField]
        private AudioComponent _soundEffectTrack;
        //public bool SkipTitleScreen { get; set; }

        [SerializeField]
        private StoryConfig _storyConfig = null;

        public ActionController ActionController { get; set; }

        private static GameController _instance;
        [HideInInspector]
        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GameObject.FindObjectOfType<GameController>();
                return _instance;
            }
        }

        [SerializeField]
        private StorySceneConfig _beginningStorySceneConfig;

        [SerializeField]
        private string _titleScreenScene;
        public string TitleScreenScene { get { return _titleScreenScene; } }

        [SerializeField]
        private GameObject UICamera;

        private IDisplayDebugInfo _displayDebugInfo { get; set; }

        private SceneSetup _sceneSetup = null;

        private AudioController _audioController;
        

        //private Dictionary<>

        public override void Awake()
        {
            base.Awake();
            PersistenceController = new PersistenceController();
            if (goUIManager != null)
                UIManager = goUIManager.GetComponent<IUIManager>();
            _storyConfig.Init();
            if (!Application.isPlaying)
                return;

            //Debug.Log("Awake called");

            _audioController = GameObject.FindObjectOfType<AudioController>();

            _displayDebugInfo = GetComponent<IDisplayDebugInfo>();
            ProcessAwake();
        }

        //public RawInput GetInput()
        //{
        //    return InputManager.Instance.GetInput();
        //}

        private void ProcessAwake()
        {
            // Track unhandled exceptions
            Application.logMessageReceived += (string condition, string stackTrace, LogType type) =>
                {
                    if (type == LogType.Exception)
                    {
                        Log.Fatal(condition + " " + stackTrace + " " + type.ToString());
                    }
                    else
                    {
                        Log.Info(condition + " " + stackTrace + " " + type.ToString());
                    }
                };

        }

        static public bool isActive
        {
            get
            {
                return _instance != null;
            }
        }

        // Use this for initialization
        //public override void Start()
        //{
        //    base.Start();
        //    if (!Application.isPlaying)
        //        return;
        //    //InputManager.Instance.Start();
        //    //var cam = UICamera.GetComponent<IUICamera>();
        //    //InputManager.Instance.SetCamera(cam);
        //    //SetCamera(cam);
        //}

        //public void SetCamera(IUICamera cam)
        //{
        //    //cam.SetKeys(KeyCode.LeftControl, KeyCode.Joystick1Button0, KeyCode.Escape, KeyCode.Joystick1Button1);
        //}

        public GraphicsEngine GetGraphicsEngine()
        {
            return _graphicsEngine;
        }

        public override void Update()
        {
            base.Update();
            MessageDispatcher.Instance.DispatchDelayedMessages();
            MessageDispatcher2.Instance.DispatchDelayedMessages();
            //var rawInput = InputManager.Instance.Update();
            if (_displayDebugInfo != null)
            {
                //_displayDebugInfo.FirePressCount = rawInput.GetButtonPressCount(Button.Primary);
                if (GameDataController.Instance != null && GameDataController.Instance.Data != null && GameDataController.Instance.Data.GetStoryProgress() != null)
                    _displayDebugInfo.StoryScene = GameDataController.Instance.Data.GetStoryProgress().ToString();
            }
        }

        // TODO Move this to a Global State for the GameController
        //public override void FixedUpdate()
        //{
        //    base.FixedUpdate();
        //    if (!Application.isPlaying)
        //        return;
        //    if (GameDataController.Instance.LoadingGame)
        //        return;
        //    //InputManager.Instance.FixedUpdate();
        //}
        private List<IEntity> _tempBosses = new List<IEntity>();

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("SetCamera", this.UniqueId, (data) => 
            {
                var newCamera = data.ExtraInfo as CameraClass;
                SetCamera(newCamera);
                MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, "UI Manager", newCamera);
            });
            MessageDispatcher2.Instance.StartListening("GetCamera", this.UniqueId, (data) =>
            {
                var camera = GetCamera();
                MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, data.SenderId, (ICameraClass)camera);
                //MessageDispatcher2.Instance.DispatchMsg("SetCamera", 0f, this.UniqueId, "UI Manager", newCamera);
            });
            MessageDispatcher2.Instance.StartListening("KillBosses", this.UniqueId, (data) =>
                {
                    var bossList = EntityContainer._instance.GetBosses();
                    if (bossList == null)
                        return;

                    //var newBossList = ObjectPool.Instance.PullFromPool<List<IEntity>>(ObjectPoolType.IEntityList);
                    //newBossList.Clear();
                    _tempBosses.Clear();

                    foreach (var boss in bossList)
                    {
                        _tempBosses.Add(boss.Value);
                    }

                    foreach (var boss in _tempBosses)
                    {
                        MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, this.UniqueId, boss.UniqueId, null);
                    }
                    //var bosses = bossList.Select(i => i.Key).ToList();
                    
                    //for (int i = bosses.Count - 1; i >= 0; i--)
                    //{
                    //    MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, this.UniqueId, bosses[i], null);
                    //}
                    //ObjectPool.Instance.ReleaseToPool(ObjectPoolType.IEntityList, newBossList);
                });
            MessageDispatcher2.Instance.StartListening("DamageAllEnemies", this.UniqueId, (data) =>
                {
                    var enemies = EntityController.Instance.GetByEntityType(EntityType.Enemy);
                    var bosses = EntityController.Instance.GetByEntityType(EntityType.Boss);
                    if (bosses != null)
                    {
                        for (int i = 0; i < bosses.Count; i++)
                        {
                            enemies.Add(bosses[i]);
                        }
                        //enemies.AddRange(bosses);
                    }
                        
                    foreach (var enemy in enemies)
                    {

                    }
                    ObjectPool.Instance.ReleaseToPool(ObjectPoolType.IEntityList, bosses);
                    ObjectPool.Instance.ReleaseToPool(ObjectPoolType.IEntityList, enemies);
                });
            MessageDispatcher2.Instance.StartListening("SendBossVictoryNotification", this.UniqueId, (data) =>
            {
                ActionController?.CheckAndRunActionSequences("BossVictory");
            });
            MessageDispatcher2.Instance.StartListening("PlaySoundOnSoundEffectTrack", this.UniqueId, (data) =>
            {
                var audioComponent = this.GetSoundEffectTrack();
                audioComponent.PlayOneShot((PlaySoundData)data.ExtraInfo);
            });
            //MessageDispatcher2.Instance.StartListening("GetConfig", this.UniqueId, (data) =>
            //{
            //    MessageDispatcher2.Instance.DispatchMsg("SetConfig", 0f, this.UniqueId, data.SenderId, _configs);
            //});
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetCamera", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("KillBosses", this.UniqueId, -1);
            MessageDispatcher2.Instance.StopListening("SendBossVictoryNotification", this.UniqueId, -1);
            //MessageDispatcher2.Instance.StopListening("GetConfig", this.UniqueId);
        }

        public void SetSceneSetup(SceneSetup sceneSetup)
        {
            _sceneSetup = sceneSetup;
            GameDataController.Instance.Data.CurrentSceneUniqueId = sceneSetup.SceneConfig.UniqueId;
        }

        //public void SetDialogueDatabase(DialogueDatabase dialogueDatabase)
        //{
        //    // Turning this off for now, it is slow.
        //    //return;
        //    //DialogManager.RemoveDatabase
        //    // If the database is changing, remove the old one
        //    if (_dialogueDatabase != null && _dialogueDatabase != dialogueDatabase)
        //    {
        //        Log.Info("Removing database " + _dialogueDatabase.name);
        //        //Debug.Log("Removing database " + _dialogueDatabase.name);
        //        DialogueManager.RemoveDatabase(_dialogueDatabase);
        //    }

        //    // If the database is null or it is changing, add the new one
        //    if (_dialogueDatabase == null || _dialogueDatabase != dialogueDatabase)
        //    {
        //        _dialogueDatabase = dialogueDatabase;
        //        if (_dialogueDatabase == null)
        //            return;
        //        //if (_dialogueDatabase != null)
        //        //{
        //        Log.Info("Adding database " + _dialogueDatabase.name);
        //        //Debug.Log("Adding database " + _dialogueDatabase.name);
        //        DialogueManager.AddDatabase(_dialogueDatabase);
        //    }
        //    //DialogueManager.Instance.PreloadResources();
        //    //DialogueManager.Instance.PreloadMasterDatabase();
        //    //_dialogManager.initialDatabase = dialogueDatabase;
        //    //_dialogManager.PreloadResources();
        //    //.
        //    //PersistentDataManager.Apply();
        //    //DialogueManager.Instance.PreloadResources();
        //}

        public SceneSetup GetSceneSetup()
        {
            return _sceneSetup;
        }

        public CameraClass GetCamera()
        {
            return _camera;
        }

        private void SetCamera(CameraClass camera)
        {
            GameDataController.Instance.Camera = camera;
            _camera = camera;
        }

        public void PauseGameplay()
        {
            Time.timeScale = 0f; // Stops normal gameplay elements
        }

        public void ResumeGameplay()
        {
            Time.timeScale = 1.0f; // Resumes normal gameplay elements
        }

        public override bool HandleMessage(Telegram msg)
        {
            base.HandleMessage(msg);
            switch (msg.Msg)
            {
                case Telegrams.GetCamera:
                    msg.Act(GetCamera());
                    return true;
                case Telegrams.GetObject:
                    msg.Act(this);
                    return true;
            }
            return false;
        }

        public void AppStart()
        {
            // TODO Read in .ini file, process other beginning of game configurations
            //Screen.SetResolution(screenWidth, screenHeight, true);
            // AutoPlay is used by the state machine to determine whether to start at the Title screen
            // or immediately start playing
            //GameStateController.Instance.AutoStart = true;
            // TODO - This is temporary
            QualitySettings.vSyncCount = 1;

            var prefs = GameStateController.Instance.GetGamePrefs();

            //GameController.Instance.GetGraphicsEngine().SetScreenResolution(prefs.ScreenWidth, prefs.ScreenHeight, prefs.IsFullScreen);
            GameDataController.Instance.LoadStoryConfigs(GameController.Instance.GameConfig);
        }

        public void BeginNewGame()
        {
            GameStateController.Instance.NewGame();
            GameStateController.Instance.StartInit();
            MessageDispatcher2.Instance.DispatchMsg("SetGold", 0f, this.UniqueId, "UI Manager", GameDataController.Instance.Data.Inventory.Gold);
            var sceneConfig = GameDataController.Instance.NextSceneConfig;
            if (sceneConfig == null)
                sceneConfig = GameDataController.Instance.CurrentSceneConfig;

            //MessageDispatcher2.Instance.DispatchMsg("SetHUDSkill", 0f, this.UniqueId, "UI Manager", null);
            // Populate starting items

            //Debug.Log("Populating " + sceneConfig.StartingItems.Length + " Starting Items");
            if (sceneConfig.StartingItems != null)
            {
                bool isFirst = true;
                Debug.Log("Adding Starting Items to inventory.");
                foreach (var startingItem in sceneConfig.StartingItems)
                {
                    if (startingItem.Item == null)
                        continue;
                    var addItemData = new ItemAndQuantityData()
                    {
                        ItemConfig = startingItem.Item as IItemConfig,
                        Quantity = startingItem.Quantity
                    };
                    MessageDispatcher2.Instance.DispatchMsg("AddItem", 0f, this.UniqueId, "Inventory Controller",
                        addItemData);
                    if (isFirst)
                    {
                        GameDataController.Instance.Data.SelectedSkill = startingItem.Item.UniqueId;
                        MessageDispatcher2.Instance.DispatchMsg("SetHUDSkill", 0f, UniqueId, "UI Manager", startingItem.Item.UniqueId);
                        isFirst = false;
                    }
                }

                // Extract first starting blueprint and mold and set as active
                for (int i = 0; i < sceneConfig.StartingItems.Length; i++)
                {
                    var startingItem = sceneConfig.StartingItems[i].Item as ItemConfig;
                    if (startingItem == null)
                        continue;
                    if (startingItem.ItemClass == ItemClass.Blueprint)
                        GameDataController.Instance.CurrentBlueprint = startingItem;
                    if (startingItem.ItemClass == ItemClass.Mold)
                    {
                        Debug.Log("Setting Mold");
                        MessageDispatcher2.Instance.DispatchMsg("SetMold", 0f, this.UniqueId, "UI Manager", startingItem);
                    }
                    //GameDataController.Instance.CurrentMold = startingItem;
                }
            }
        }

        public AudioComponent GetMusicTrack()
        {
            return _musicTrack;
        }

        public AudioComponent GetSoundEffectTrack()
        {
            return _soundEffectTrack;
        }
    }
}
