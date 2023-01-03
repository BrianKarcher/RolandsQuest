using RQ.Common.Config;
using RQ.Model.Audio;
using UnityEngine;

namespace RQ.Model.Item
{
    //[CustomEditor(typeof(AreaConfig), true)]
    public class ItemConfig : RQBaseConfig, IItemConfig
    {
        [SerializeField]
        private Texture _gridTexture;
        [SerializeField]
        private string _title;
        [SerializeField]
        private string _description;
        [SerializeField]
        private ItemClass _itemClass;
        [SerializeField]
        private ItemType _type;
        [SerializeField]
        private int _value;
        [SerializeField]
        private string _acquireText;
        [SerializeField]
        private PlaySoundInfo _acquireSound = null;
        //[SerializeField]
        //private Transform _actionSequence;

        //public string UniqueId { get { return _uniqueId; } }
        public Texture GridTexture { get { return _gridTexture; } }
        public string Title { get { return _title; } }
        public string Description { get { return _description; } }
        public ItemClass ItemClass { get { return _itemClass; } }
        public ItemType Type { get { return _type; } }
        public int Value { get { return _value; } }
        public string AcquireText { get { return _acquireText; } }
        public PlaySoundInfo AcquireSound { get { return _acquireSound; } }
        //public Transform ActionSequence { get { return _actionSequence; } }

        //void OnEnable()
        //{
        //    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, "Inventory Controller",
        //        Telegrams.AddContainerItem, this);
        //}
    }
}
