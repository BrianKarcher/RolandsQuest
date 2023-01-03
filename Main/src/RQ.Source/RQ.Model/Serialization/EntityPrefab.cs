using RQ.Common.UniqueId;
using RQ.Enums;
using System;
using UnityEngine;

namespace RQ.Model.Serialization
{
    [Serializable]
    public class EntityPrefab
    {
        [SerializeField]
        [UniqueIdentifier]
        private string _uniqueId;

        [SerializeField]
        private string _name;

        [SerializeField]
        private EntityType _type;

        [SerializeField]
        private Transform _prefab;

        public string UniqueId { get { return _uniqueId; } set { _uniqueId = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public EntityType Type { get { return _type; } set { _type = value; } }
        public Transform Prefab { get { return _prefab; } set { _prefab = value; } }
    }
}
