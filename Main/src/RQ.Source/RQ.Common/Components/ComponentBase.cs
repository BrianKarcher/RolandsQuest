using System;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using UnityEngine;

namespace RQ.Common.Components
{
    public class ComponentBase<T> : MessagingObject, IComponentBase where T : class, IComponentBase
    {        
        [SerializeField]
        protected ComponentRepository _componentRepository = null;

        protected string _componentRepositoryId;
        private bool _hasAwakened = false;
        private Action<Telegram2> _killDelegate;

        public override void Awake()
        {
            base.Awake();
            _hasAwakened = true;
            _killDelegate = (data) =>
            {
                Destroy();
            };
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("Kill", this.UniqueId, _killDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("Kill", this.UniqueId, -1);
        }

        public virtual void Reset()
        { }

        /// <summary>
        /// This gets called right before a pooled entity is enabled
        /// </summary>
        public virtual void ReAwaken()
        {
            if (!_hasAwakened)
                return;
            // OnEnable causes a State Enter to immediaely fire. Start listening to events prior to this happening to avoid bugs.
            StartListening();
        }

        protected void SetComponentRepository()
        {
            var currentTransform = transform;
            while (_componentRepository == null)
            {
                // Search up the tree to find the component repository. This is a time saver.
                _componentRepository = currentTransform.GetComponent<IComponentRepository>() as ComponentRepository;
                currentTransform = currentTransform.parent;
                // Reached the top of the tree
                if (currentTransform == null)
                    break;
            }
        }

        public override void Start()
        {
            base.Start();
            if (_componentRepository != null)
                _componentRepositoryId = _componentRepository.UniqueId;
        }

        public override void OnEnable()
        {
            if (_componentRepository == null)
                return;
            base.OnEnable();
        }

        public override void OnDisable()
        {
            if (_componentRepository == null)
                return;
            base.OnDisable();
        }

        public override void Init()
        {
            //base.UniqueIdChanged += (a, b) =>
            //{

            //};
            base.UniqueIdChanged += UniqueIdChange;
            base.Init();
            SetComponentRepository();

            if (_componentRepository != null)
                _componentRepository.Components.RegisterComponent<T>(this as T);
        }

        public void UniqueIdChange(string old, string newString)
        {
            _componentRepository.Components.UnregisterComponentById<T>(old);
            _componentRepository.Components.RegisterComponent<T>(this as T);
        }

        public override void Destroy()
        {            
            base.Destroy();
            base.UniqueIdChanged -= UniqueIdChange;
            UnregisterComponent();
        }

        public override void OnDestroy()
        {            
            base.OnDestroy();
            UnregisterComponent();
        }

        private void UnregisterComponent()
        {
            if (_componentRepository?.Components == null)
                return;
            _componentRepository.Components.UnRegisterComponent<T>(this as T);
        }

        public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo, 
            TelegramEarlyTermination earlyTermination)
        {
            if (_componentRepositoryId == null)
            {
                //int i = 1;
                return;
            }
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _componentRepositoryId, 
                msg, extraInfo, null, earlyTermination);
        }

        public Vector3 GetLocalPos()
        {
            return this.transform.localPosition;
        }

        public Vector3 GetWorldPos()
        {
            return this.transform.position;
        }

        public Vector2 GetWorldPos2D()
        {
            return this.transform.position;
        }

        public IComponentRepository GetComponentRepository()
        {
            return _componentRepository;
        }
    }
}
