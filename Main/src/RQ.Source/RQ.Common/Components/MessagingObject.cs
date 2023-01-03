using RQ.Common;
using System;
using UnityEngine;

namespace RQ.Messaging
{
    public class MessagingObject : BaseObject, IMessagingObject
    {
        [SerializeField]
        private string _rqName;
        [SerializeField]
        private string _rqName2;
        //[SerializeField]
        //private MessageDispatcher _messageDispatcher;
        private bool _isListening = false;

        protected string RQName { get { return _rqName; } set { _rqName = value; } }
        protected string RQName2 { get { return _rqName2; } set { _rqName2 = value; } }
        public override void Awake()
        {
            Init();
            base.Awake();
            if (String.IsNullOrEmpty(_rqName))
                _rqName = name + " " + this.GetType().ToString();
            // New Guids have a higher chance of uniqueness, and this only needs to be unique within the 
            // scope of the component repository. (another repo with this same uniqueId is ok).
            if (String.IsNullOrEmpty(_rqName2))
                _rqName2 = Guid.NewGuid().ToString();   
        }

        public override void Init()
        {
            base.Init();
            if (!Application.isPlaying)
                return;
        }

        public virtual bool HandleMessage(Telegram msg)
        {
            return false;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            //if (!Application.isPlaying)
            //{
            //    if (String.IsNullOrEmpty(this.ID))
            //        ID = Guid.NewGuid().ToString();
            //    UniqueIdRegistry.Register(this.ID, this.GetInstanceID());
            //}

            if (!Application.isPlaying)
                return;

            if (!_isListening)
            {
                StartListening();
                _isListening = true;
            }

            RegisterWithMessageDispatcher();
        }

        public virtual void StartListening()
        {
            //Debug.Log("StartListening called on " + this.name);
            RegisterWithMessageDispatcher();
        }

        public virtual void StopListening()
        {
            //Debug.Log("StopListening called on " + this.name);
            MessageDispatcher.Instance.UnregisterMessageHandler(this);
        }

        protected bool IsListening()
        {
            return _isListening;
        }

        protected void RegisterWithMessageDispatcher()
        {
            MessageDispatcher.Instance.RegisterMessageHandler(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;
            if (_isListening)
            {
                StopListening();
                _isListening = false;
            }
            MessageDispatcher.Instance.UnregisterMessageHandler(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            // This is getting destroyed, any messages being sent its way are deprecated
            // Remove them so future object with same Unique Id does not accidently receive them
            MessageDispatcher2.Instance.RemoveMessagesForReceiverId(this.UniqueId);
        }

        /// <summary>
        /// Unline OnDestroy, this gets called whether or not the object has awoken
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            if (_isListening)
            {
                StopListening();
                _isListening = false;
            }
        }

        public override void SetUniqueId(string uniqueId, bool force = false)
        {
            // The Unique Id is changing, need to deregister and reregister this component
            MessageDispatcher.Instance.UnregisterMessageHandler(this);
            if (force)
                MessageDispatcher.Instance.UnregisterMessageHandler(uniqueId);
            //StopListening();
            MessageDispatcher2.Instance.ChangeListener(this.UniqueId, uniqueId);
            base.SetUniqueId(uniqueId, force);
            MessageDispatcher.Instance.RegisterMessageHandler(this);
            //StartListening();
        }

        public override string GetName()
        {
            return string.IsNullOrEmpty(_rqName2) ? name : _rqName2;
        }

        public void SetRqName2(string rqName2)
        {
            _rqName2 = rqName2;
        }
    }
}
