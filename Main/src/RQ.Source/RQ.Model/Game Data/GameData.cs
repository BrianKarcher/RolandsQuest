using Newtonsoft.Json;
using RQ.Controller.ManageScene;
using RQ.Entity.Data;
using RQ.Model.Containers;
using RQ.Model.Game_Data.Quest;
using RQ.Model.Game_Data.StoryProgress;
using RQ.Model.Inventory;
using System;
using System.Collections.Generic;

namespace RQ.Model.Game_Data
{
    /// <summary>
    /// Contains information regarding the player's progress through the game. This object gets destroyed and recreated when the 
    /// player starts a new game or loads a game
    /// </summary>
    [Serializable]
    public class GameData
    {
        //public GameProgress GameProgress { get; set; }

        //public Dictionary<string, AreaData> AreaDatas { get; set; }
        public Dictionary<string, SceneData> SceneDatas { get; set; }

        [JsonProperty]
        //public StorySceneConfig StoryScene { get; set; }
        private StoryProgressData StoryProgress { get; set; }

        public Dictionary<string, QuestData> QuestDatas { get; set; }

        /// <summary>
        /// Global variables
        /// </summary>
        public Dictionary<string, Variable> GlobalVariables { get; set; }

        public InventoryData Inventory { get; set; }
        public EntityStatsData CurrentEntityStats { get; set; }
        //public EntityStatsData SceneEnterStats { get; set; }

        public Dictionary<string, List<string>> ItemUseEvents { get; set; }

        //private LayerMask _levelOneLayerMask;
        //private LayerMask _levelOneLayerMask { get; set; } //{ get { return _levelOneLayerMask; } set { _levelOneLayerMask = value; } }
        //private LayerMask _levelTwoLayerMask;
        //private LayerMask _levelTwoLayerMask { get; set; } //{ get { return _levelTwoLayerMask; } set { _levelTwoLayerMask = value; } }


        public SceneData CurrentScene { get; set; }
        //public string CurrentSceneString { get; set; }
        //public int CurrentSceneId { get; set; }
        public string CurrentSceneUniqueId { get; set; }
        public string SpawnpointUniqueId { get; set; }
        // Used when loading a game to set the player back at the spawnpoint where the scene got entered
        //public string CurrentSceneEnteredSpawnpointUniqueId { get; set; }
        public string NextSpawnpointUniqueId { get; set; }
        
        //public UsableContainer UsableContainer { get; set; }
        
        /// <summary>
        /// List of previous scenes visited so we can determine whether to persist enemy states
        /// </summary>
        //public List<string> PreviousScenes { get; set; }
        public LinkedList<SceneDeathData> SceneDeathDatas { get; set; }

        // TODO Move this to the GameStateController, it does not belong in GameData
        // It is state data, not game data
        public string RunningSequenceUniqueId { get; set; }
        public string SelectedSkill { get; set; }

        public GameData()
        {
            SceneDeathDatas = new LinkedList<SceneDeathData>();
            //UsableContainer = new UsableContainer();
            Inventory = new InventoryData();
            ItemUseEvents = new Dictionary<string, List<string>>();
        }

        public void SetStoryProgress(StoryProgressData storyProgress, bool force = false)
        {
            if (force || storyProgress > StoryProgress)
                StoryProgress = storyProgress;
        }

        public StoryProgressData GetStoryProgress()
        {
            return StoryProgress;
        }
    }
}
