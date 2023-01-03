using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.GameData.StoryProgress
{
    public class StoryConfig : ScriptableObject
    {
        public int Id;
        public string Name;
        public List<StoryActConfig> ActConfigs;
        private Dictionary<string, StoryActConfig> _actConfigsDic;

        public StoryConfig()
        {
            _actConfigsDic = new Dictionary<string, StoryActConfig>();
            if (ActConfigs != null)
            {
                ActConfigs.ForEach(i => _actConfigsDic.Add(i.Name, i));
            }
        }

        public StoryActConfig Get(string actName)
        {
            return _actConfigsDic[actName];
        }

        public void Init()
        {
            for (int i = 0; i < ActConfigs.Count; i++)
            {
                var actConfig = ActConfigs[i];
                actConfig.Id = i;
                actConfig.StoryConfig = this;
                actConfig.Init();
            }
        }

        // TODO Place dict in StoryConfig to hold scenes by uniqueId and fetch from that
        public StorySceneConfig FindScene(string sceneUniqueId)
        {
            foreach (var act in ActConfigs)
            {
                foreach (var chapter in act.ChapterConfigs)
                {
                    return chapter.FindScene(sceneUniqueId);
                }
            }
            return null;
        }
    }
}
