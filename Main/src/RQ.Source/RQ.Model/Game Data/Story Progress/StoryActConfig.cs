using RQ.Common.UniqueId;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Model.GameData.StoryProgress
{
    public class StoryActConfig : ScriptableObject
    {
        [UniqueIdentifier]
        public string UniqueId;
        public int Id;
        public string Name;
        public List<StoryChapterConfig> ChapterConfigs;
        private Dictionary<string, StoryChapterConfig> _chapterConfigsDic;
        public StoryConfig StoryConfig { get; set; }

        public StoryActConfig()
        {
            _chapterConfigsDic = new Dictionary<string, StoryChapterConfig>();
            if (ChapterConfigs != null)
            {
                ChapterConfigs.ForEach(i => _chapterConfigsDic.Add(i.UniqueId, i));
            }
        }

        public StoryChapterConfig Get(string uniqueId)
        {
            return _chapterConfigsDic[uniqueId];
        }

        public void Init()
        {
            for (int i = 0; i < ChapterConfigs.Count; i++)
            {
                var chapterConfig = ChapterConfigs[i];
                chapterConfig.Id = i;
                chapterConfig.StoryActConfig = this;
                chapterConfig.Init();
            }
        }
    }
}
