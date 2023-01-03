using RQ.Common.UniqueId;
using RQ.Model;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.ManageScene
{
    public class AreaConfig : ScriptableObject
    {
        [UniqueIdentifier]
        public string UniqueId;
        public List<SceneConfig> Scenes;
        public string Name;
        //public WorldAreas Areas;

        [HideInInspector]
        public List<Variable> Variables;
    }
}
