using System;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class ChangeScenesData
    {
        public string SceneConfigUniqueId { get; set; }
        public string SpawnPointUniqueId { get; set; }
    }
}
