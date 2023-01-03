using RQ.Common.UniqueId;
using RQ.Model.Game_Data.StoryProgress;
using System;
using UnityEngine;

namespace RQ.Model.GameData.StoryProgress
{
    public class StorySceneConfig : ScriptableObject, IComparable<StorySceneConfig>
    {
        [UniqueIdentifier]
        public string UniqueId;
        public int Id;
        public string Name;
        public StoryChapterConfig StoryChapterConfig { get; set; }
        public int Act
        {
            get
            {
                return 0;
                //if (StoryChapterConfig == null || StoryChapterConfig.StoryActConfig == null)
                //    return 0;
                //return StoryChapterConfig.StoryActConfig.Id;
            }
        }

        public int Chapter 
        { 
            get 
            {
                if (StoryChapterConfig == null)
                    return 0;
                return StoryChapterConfig.Id; 
            } 
        }

        public static bool operator <(StorySceneConfig s1, StorySceneConfig s2)
        {
            if (s1.Act < s2.Act)
                return true;
            if (s1.Act > s2.Act)
                return false;
            if (s1.Chapter < s2.Chapter)
                return true;
            if (s1.Chapter > s2.Chapter)
                return false;
            if (s1.Id < s2.Id)
                return true;

            return false;
        }

        public static bool operator <(StorySceneConfig s1, StoryProgressData s2)
        {
            if (s1.Act < s2.Act)
                return true;
            if (s1.Act > s2.Act)
                return false;
            if (s1.Chapter < s2.Chapter)
                return true;
            if (s1.Chapter > s2.Chapter)
                return false;
            if (s1.Id < s2.Scene)
                return true;

            return false;
        }

        public static bool operator >(StorySceneConfig s1, StorySceneConfig s2)
        {
            if (s1.Act < s2.Act)
                return false;
            if (s1.Act > s2.Act)
                return true;
            if (s1.Chapter < s2.Chapter)
                return false;
            if (s1.Chapter > s2.Chapter)
                return true;
            if (s1.Id > s2.Id)
                return true;

            return false;
        }

        public static bool operator >(StorySceneConfig s1, StoryProgressData s2)
        {
            if (s1.Act < s2.Act)
                return false;
            if (s1.Act > s2.Act)
                return true;
            if (s1.Chapter < s2.Chapter)
                return false;
            if (s1.Chapter > s2.Chapter)
                return true;
            if (s1.Id > s2.Scene)
                return true;

            return false;
        }

        //public static bool operator ==(StorySceneConfig s1, StorySceneConfig s2)
        //{
        //    if (s1.Equals(null) && s1.Equals(null))
        //        return true;
        //    if (s1.Equals(null))
        //        return false;
        //    if (s2.Equals(null))
        //        return false;
        //    //if (s1 == null && s2 == null)
        //    //    return true;
        //    //if (s1 == null)
        //    //    return false;
        //    //if (s2 == null)
        //    //    return false;
        //    return s1.Act == s2.Act && s1.Chapter == s2.Chapter && s1.Id == s2.Id;
        //}

        public static bool operator ==(StorySceneConfig s1, StoryProgressData s2)
        {
            if ((object)s1 == null && (object)s1 == null)
                return true;
            if ((object)s1 == null)
                return false;
            if ((object)s2 == null)
                return false;

            //if (s1 == null && s2 == null)
            //    return true;
            //if (s1 == null)
            //    return false;
            //if (s2 == null)
            //    return false;
            return s1.Act == s2.Act && s1.Chapter == s2.Chapter && s1.Id == s2.Scene;
        }

        //public static bool operator !=(StorySceneConfig s1, StorySceneConfig s2)
        //{
        //    return !(s1 == s2);
        //}

        public static bool operator !=(StorySceneConfig s1, StoryProgressData s2)
        {
            return !(s1 == s2);
        }

        public int CompareTo(StorySceneConfig other)
        {
            if (this == other)
                return 0;
            if (this < other)
                return -1;
            return 1;
        }

        public void Init()
        {

            //for (int i = 0; i < SceneConfigs.Count; i++)
            //{
            //    var sceneConfig = SceneConfigs[i];
            //    sceneConfig.Id = i;
            //    sceneConfig.StoryChapterConfig = this;
            //    sceneConfig.Init();
            //}
        }
    }
}
