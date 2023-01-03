using RQ.Common;
using RQ.Enum;
using RQ.Enums;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Spawn Point")]
    public class SpawnPointComponent : BaseObject
    {
        //[HideInInspector]
        public string SpawnPointUniqueId;
        public EntityType EntityType;
        [SerializeField]
        private LevelLayer _levelLayer = LevelLayer.LevelOne;
        public LevelLayer LevelLayer { get { return _levelLayer; } set { _levelLayer = value; } }
        //public Vector3 Register()
        //{

        //}
    }
}
