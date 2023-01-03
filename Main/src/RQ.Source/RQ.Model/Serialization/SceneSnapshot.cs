using RQ.Controller.ManageScene;
using RQ.Entity.Data;
using RQ.Model.Inventory;
using System;
using System.Collections.Generic;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class SceneSnapshot
    {
        public string SceneConfigUniqueId { get; set; }
        public string SpawnpointUniqueId { get; set; }
        public EntityStatsData EntityStats { get; set; }
        //public Dictionary<string, AreaData> AreaDatas { get; set; }
        // Key is SceneUniqueId
        public Dictionary<string, SceneData> Scenes { get; set; }
        //public Dictionary<string, List<Variable>> SceneVariables { get; set; }
        /// <summary>
        /// Global variables
        /// </summary>
        public Dictionary<string, Variable> GlobalVariables { get; set; }
        public InventoryData Inventory { get; set; }
        /// <summary>
        /// List of previous scenes visited so we can determine whether to persist enemy states
        /// </summary>
        //public List<string> PreviousScenes { get; set; }
        public LinkedList<SceneDeathData> SceneDeathDatas { get; set; }
        public string SelectedSkill { get; set; }
    }
}
