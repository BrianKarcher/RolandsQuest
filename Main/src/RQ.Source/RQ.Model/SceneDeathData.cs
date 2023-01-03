using System;
using System.Collections.Generic;

namespace RQ.Model
{
    [Serializable]
    public class SceneDeathData
    {
        public string SceneUniqueId { get; set; }
        public List<string> DeadEntities { get; set; }

        public SceneDeathData()
        {
            DeadEntities = new List<string>();
        }

        public SceneDeathData Clone()
        {
            return new SceneDeathData()
            {
                SceneUniqueId = SceneUniqueId,
                // Create a copy of the list
                DeadEntities = new List<string>(DeadEntities)
            };
        }
    }
}
