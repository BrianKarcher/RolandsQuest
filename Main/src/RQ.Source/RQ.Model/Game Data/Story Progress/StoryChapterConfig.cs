using RQ.Common.UniqueId;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.GameData.StoryProgress
{
    public class StoryChapterConfig : ScriptableObject
    {
        [UniqueIdentifier]
        public string UniqueId;
        public int Id;
        public string Name;
        public List<StorySceneConfig> SceneConfigs;
        private Dictionary<string, StorySceneConfig> _sceneConfigsDic;
        public StoryActConfig StoryActConfig { get; set; }

        public StoryChapterConfig()
        {
            _sceneConfigsDic = new Dictionary<string, StorySceneConfig>();
            if (SceneConfigs != null)
            {
                SceneConfigs.ForEach(i => _sceneConfigsDic.Add(i.UniqueId, i));
            }
        }

        public StorySceneConfig FindScene(string uniqueId)
        {
            return _sceneConfigsDic[uniqueId];
        }

        public void Init()
        {
            for (int i = 0; i < SceneConfigs.Count; i++)
            {
                var sceneConfig = SceneConfigs[i];
                sceneConfig.Id = i;
                sceneConfig.StoryChapterConfig = this;
                sceneConfig.Init();
            }
        }
    }
}
