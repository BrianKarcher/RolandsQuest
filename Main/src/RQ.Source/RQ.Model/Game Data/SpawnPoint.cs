using RQ.Common.UniqueId;
using System;
using UnityEngine;

namespace RQ.Controller.ManageScene
{
    //[Serializable]
    //public class Points2Int : Points2<int> { }
    // This is a hack to get around Unity's inability to serialize generics
    // It's either this or write my own serializer.
    //[Serializable]
    //public class PointsUsingSpawnpoint : Points<SpawnPoint> { }

    //[Serializable]
    //[Obsolete]
    //public class SpawnPoint : NamedPoint
    //{
    //    /// <summary>
    //    /// From what scene did the player come FROM?
    //    /// </summary>
    //    public SceneConfig SceneCameFrom;
    //    [UniqueIdentifier]
    //    public string UniqueId;
    //    //public int SpawnPointInsanceId;
    //    ////public Vector3 Point;
    //    public bool IsInitialSpawnPoint;
    //    //public string Name;
    //    public string ExtraInfo;

    //    public SpawnPoint Clone()
    //    {
    //        return new SpawnPoint()
    //        {
    //            SceneCameFrom = SceneCameFrom,
    //            //Point = Point,
    //            X = X,
    //            Y = Y,
    //            IsInitialSpawnPoint = IsInitialSpawnPoint,
    //            Name = Name,
    //            ExtraInfo = ExtraInfo,
    //            UniqueId = UniqueId
    //            //SpawnPointInsanceId = SpawnPointInsanceId
    //        };
    //    }
    //}

    [Serializable]
    public class SpawnPointInAsset
    {
        [SerializeField]
        public string Name;
        //private string _name;
        //public string Name { get { return _name; } set { _name = value; } }
                
        [SerializeField]
        [UniqueIdentifier]
        //[HideInInspector]
        public string UniqueId;

        public SceneConfig SceneCameFrom;
        //public int SpawnPointInsanceId;
        ////public Vector3 Point;
        public bool IsInitialSpawnPoint;
        //public string Name;
        public string ExtraInfo;
    }

    //[Serializable]
    //public class SpawnPoint : MonoBehaviour
    //{

    //}
}
