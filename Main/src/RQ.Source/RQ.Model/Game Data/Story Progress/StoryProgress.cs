using RQ.Model.GameData.StoryProgress;
using System;

namespace RQ.Model.Game_Data.StoryProgress
{
    [Serializable]
    public class StoryProgressData
    {
        public int Act { get; set; }
        public int Chapter { get; set; }
        public int Scene { get; set; }

        private readonly string _progress;

        public StoryProgressData()
        {
            _progress = Act + "." + Chapter + "." + Scene;
        }

        public StoryProgressData(int act, int chapter, int scene)
        {
            Act = act;
            Chapter = chapter;
            Scene = scene;
        }

        public override string ToString()
        {
            return _progress;
        }

        public static implicit operator StoryProgressData(StorySceneConfig storySceneConfig)
        {
            if (storySceneConfig == null)
                return null;
            StoryProgressData storyProgressData = new StoryProgressData();
            storyProgressData.Act = storySceneConfig.Act;
            storyProgressData.Chapter = storySceneConfig.Chapter;
            storyProgressData.Scene = storySceneConfig.Id;
            return storyProgressData;
        }

        public static bool operator >(StoryProgressData s1, StorySceneConfig s2)
        {
            if (s1 == null && s2 == null)
                return false;
            if (s1 == null)
                return true;
            if (s2 == null)
                return false;
            if (s1.Act < s2.Act)
                return false;
            if (s1.Act > s2.Act)
                return true;
            if (s1.Chapter < s2.Chapter)
                return false;
            if (s1.Chapter > s2.Chapter)
                return true;
            if (s1.Scene > s2.Id)
                return true;

            return false;
        }

        public static bool operator >(StoryProgressData s1, StoryProgressData s2)
        {
            if (s1 == null && s2 == null)
                return false;
            if (s1 == null)
                return true;
            if (s2 == null)
                return false;
            if (s1.Act < s2.Act)
                return false;
            if (s1.Act > s2.Act)
                return true;
            if (s1.Chapter < s2.Chapter)
                return false;
            if (s1.Chapter > s2.Chapter)
                return true;
            if (s1.Scene > s2.Scene)
                return true;

            return false;
        }

        public static bool operator >=(StoryProgressData s1, StorySceneConfig s2)
        {
            if (s1 > s2)
                return true;
            if (s1 == s2)
                return true;
            return false;
        }

        public static bool operator <(StoryProgressData s1, StorySceneConfig s2)
        {
            if (s1 == null && s2 == null)
                return false;
            if (s1 == null)
                return false;
            if (s2 == null)
                return true;
            if (s1.Act < s2.Act)
                return true;
            if (s1.Act > s2.Act)
                return false;
            if (s1.Chapter < s2.Chapter)
                return true;
            if (s1.Chapter > s2.Chapter)
                return false;
            if (s1.Scene < s2.Id)
                return true;

            return false;
        }

        public static bool operator <(StoryProgressData s1, StoryProgressData s2)
        {
            if (s1 == null && s2 == null)
                return false;
            if (s1 == null)
                return false;
            if (s2 == null)
                return true;
            if (s1.Act < s2.Act)
                return true;
            if (s1.Act > s2.Act)
                return false;
            if (s1.Chapter < s2.Chapter)
                return true;
            if (s1.Chapter > s2.Chapter)
                return false;
            if (s1.Scene < s2.Scene)
                return true;

            return false;
        }

        public static bool operator <=(StoryProgressData s1, StorySceneConfig s2)
        {
            if (s1 < s2)
                return true;
            if (s1 == s2)
                return true;
            return false;
        }

        public static bool operator ==(StoryProgressData s1, StorySceneConfig s2)
        {
            if ((object)s1 == null && (object)s2 == null)
                return true;
            if ((object)s1 == null || (object)s2 == null)
                return false;
            return s1.Act == s2.Act && s1.Chapter == s2.Chapter && s1.Scene == s2.Id;
        }

        public static bool operator !=(StoryProgressData s1, StorySceneConfig s2)
        {
            return !(s1 == s2);
        }
    }
}
