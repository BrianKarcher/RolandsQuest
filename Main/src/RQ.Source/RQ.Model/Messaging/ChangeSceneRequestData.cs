using System;

namespace RQ.Model.Messaging
{
    [Serializable]
    public class ChangeSceneRequestData
    {
        public string SceneConfigUniqueId { get; set; }
        public string SpawnPointUniqueId { get; set; }
    }
}
