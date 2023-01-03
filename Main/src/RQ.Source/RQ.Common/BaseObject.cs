using RQ.Common.UniqueId;
using System;
using UnityEngine;

namespace RQ.Common
{
    [ExecuteInEditMode]
    public class BaseObject : MonoBehaviour, RQ.Common.IBaseObject
    {
        /// <summary>
        /// This ID does NOT persist past the scene
        /// </summary>
        public virtual int Id { get { return GetInstanceID(); } }

        /// <summary>
        /// UniqueId persists forever
        /// </summary>
        [SerializeField]
        [UniqueIdentifier] // Treat this special in the editor.
        public string _uniqueId; // A String representing our Guid
        public virtual string UniqueId { get { return _uniqueId; } set { _uniqueId = value; } }
        [SerializeField]
        private string componentName;
        public string ComponentName { get { return componentName; } set { componentName = value; } }

        private bool _isStarted = false;

        public event Action<string, string> UniqueIdChanged;

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        { }

        //public virtual void Awake()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        if (String.IsNullOrEmpty(this.UniqueId))
        //            UniqueId = Guid.NewGuid().ToString();
        //        UniqueIdRegistry.Register(this.UniqueId, this.GetInstanceID());
                
        //    }
        //}


        //public virtual void OnDestroy()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        UniqueIdRegistry.Deregister(this.UniqueId);
        //    }
        //}


        //public virtual void Update()
        //{
        //    if (!Application.isPlaying)
        //    {
        //        if (this.GetInstanceID() != UniqueIdRegistry.GetInstanceId(this.UniqueId))
        //        {
        //            //var instanceId = this.GetInstanceID();
        //            //UniqueIdRegistry.GetInstanceId(this.ID);
        //            UniqueId = Guid.NewGuid().ToString();
        //            UniqueIdRegistry.Register(this.UniqueId, this.GetInstanceID());
        //        }
        //    }
        //}

        /// <summary>
        /// Unline OnDestroy, this gets called whether or not the object has awoken
        /// </summary>
        public virtual void Destroy()
        {
            UnregisterUniqueId();
        }

        public virtual void OnDestroy()
        {
            if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
            {
                int i = 1;
            }
            UnregisterUniqueId();
            //if (!Application.isPlaying)
            //{
            
            //}
        }

        public void UnregisterUniqueId()
        {
            if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
            {
                int i = 1;
            }
            UniqueIdRegistry.Deregister(this.UniqueId);
        }

        public virtual void SetUniqueId(string uniqueId, bool force = false)
        {
            if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
            {
                int i = 1;
            }
            // Nothing else to do if they are the same
            if (uniqueId == UniqueId)
                return;
            var oldUniqueId = UniqueId;
            //UnregisterUniqueId();
            if (force)
                UniqueIdRegistry.Deregister(uniqueId);            
            UniqueId = uniqueId;
            if (UniqueIdChanged != null)
                UniqueIdChanged(oldUniqueId, UniqueId);
            RegisterUniqueId();
        }

        /// <summary>
        /// Removes any previous owner of this Id and forces this one in its place
        /// </summary>
        /// <param name="uniqueId"></param>
        //public virtual void SetUniqueIdForce(string uniqueId)
        //{
        //    if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
        //    {
        //        int i = 1;
        //    }
        //    UnregisterUniqueId();
        //    UniqueIdRegistry.Deregister(uniqueId);
        //    UniqueId = uniqueId;
        //    RegisterUniqueId();
        //}

        public virtual void Update()
        {
            if (!Application.isPlaying)
            {
                var currentId = UniqueIdRegistry.GetInstanceId(this.UniqueId);
                // The compare to 0 is done because on a rebuild, Unity resets the internal variables so UniqueIdRegistry
                // is blanked out.  In this case we do not want to issue a new Unique Id.
                if (currentId == 0)
                {
                    UniqueIdRegistry.Register(this.UniqueId, this.Id);
                }
                else if (this.Id != currentId)
                {
                    var newId = Guid.NewGuid().ToString();
                    Debug.Log("Unique Id Registered Count " + UniqueIdRegistry.Count());
                    Debug.LogError(this.name + " Update assigning new Unique Id from " + UniqueId + " to " + newId);
                    UniqueId = newId;
                    UniqueIdRegistry.Register(this.UniqueId, this.Id);
                }
            }
        }

        public virtual void Awake()
        {
            RegisterUniqueId();
        }

        public virtual void Init()
        {
            RegisterUniqueId();
        }

        protected void RegisterUniqueId()
        {
            if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
            {
                int i = 1;
            }
            //if (!Application.isPlaying)
            //{
            // The ContainsKey check is for cloned objects - they need a new Unique Id
            //bool createNewId = false;

            //if (createNewId)
            PerformUniqueIdDupCheck();
            UniqueIdRegistry.Register(this.UniqueId, this.Id);
            //}
        }

        protected void PerformUniqueIdDupCheck()
        {
            if (String.IsNullOrEmpty(this.UniqueId))
                UniqueId = Guid.NewGuid().ToString();
            //if (UniqueId == "f79a4e98-1ca8-4137-bd58-0b9ed084ff2e")
            //{
            //    int i = 1;
            //}
            if (UniqueIdRegistry.Mapping.ContainsKey(this.UniqueId))
            {
                var oldUniqueId = this.UniqueId;
                var value = UniqueIdRegistry.GetInstanceId(this.UniqueId);
                // If this Unique Id is already registered to a different instance, create a new Id
                if (value != this.Id)
                {
                    //SetUniqueId(Guid.NewGuid().ToString());
                    UniqueId = Guid.NewGuid().ToString();
                    if (UniqueIdChanged != null)
                        UniqueIdChanged(oldUniqueId, UniqueId);
                    //if (!Application.isPlaying)
                        //Log.Info("(" + name + ") Id " + oldUniqueId + " already mapped to different instance, assigning " + UniqueId);
                }
            }
        }

        public virtual void Start()
        {
            _isStarted = true;
        }

        public virtual void FixedUpdate()
        { }

        //public virtual void Start()
        //{

        //}

        //public virtual void FixedUpdate()
        //{

        //}

        public virtual string GetName()
        {
            return name;
        }

        public string GetTag()
        {
            if (this.gameObject == null)
                return null;
            return tag;
        }

        public bool GetIsStarted()
        {
            return _isStarted;
        }

        public bool IsActive { get { return base.isActiveAndEnabled; } }
    }
}
