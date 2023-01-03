using System;
using UnityEngine;

namespace RQ.Model.Item
{
    [Serializable]
    //[CustomEditor(typeof(AreaConfig), true)]
    public class ItemConfigAction
    {
        [SerializeField]
        private ItemConfig _config;

        [SerializeField]
        private Transform _actionSequence;

        public Transform ActionSequence { get { return _actionSequence; } }
        public ItemConfig Config { get { return _config; } }
    }
}
