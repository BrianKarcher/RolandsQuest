using RQ.Model;
using System;
using System.Collections.Generic;

namespace RQ.Controller.ManageScene
{
    [Serializable]
    public class SceneData
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public Dictionary<string, Variable> Variables { get; set; }
        //public Dictionary<string, SpawnPoint> SpawnPoints { get; set; }
        public string SceneConfigUniqueId { get; set; }
        [Obsolete("Going the way of the Dodo")]
        public float LevelOneZIndex { get; set; }
        [Obsolete("Going the way of the Dodo")]
        public float LevelTwoZIndex { get; set; }
        [Obsolete("Going the way of the Dodo")]
        public float LevelThreeZIndex { get; set; }

        public SceneData Clone()
        {
            var sceneData = new SceneData();
            sceneData.SceneConfigUniqueId = SceneConfigUniqueId;
            sceneData.Variables = new Dictionary<string, Variable>();
            sceneData.LevelOneZIndex = LevelOneZIndex;
            sceneData.LevelTwoZIndex = LevelTwoZIndex;
            sceneData.LevelThreeZIndex = LevelThreeZIndex;
            foreach (var variable in Variables)
            {
                sceneData.Variables[variable.Key] = variable.Value.Clone();
            }
            return sceneData;
        }
    }
}
