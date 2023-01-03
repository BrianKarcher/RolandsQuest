using System;
using System.Collections.Generic;

namespace RQ.Controller.ManageScene
{
    [Serializable]
    public class AreaData
    {
        //public string Name { get; set; }
        //public WorldAreas Areas { get; set; }
        public Dictionary<string, SceneData> SceneDatas { get; set; }
        public string AreaConfigUniqueId { get; set; }
    }
}
