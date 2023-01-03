using RQ.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using RQ.Common.Container;
using UnityEngine;

namespace RQ.Common.Components
{
    public class ComponentBase<T> : MessagingObject, IComponentBase where T : class, IComponentBase
    {
        [SerializeField]
        protected ComponentRepository _componentRepository = null;

        [SerializeField]
        private string componentName;
        public string ComponentName { get { return componentName; } set { componentName = value; } }

        private bool _hasAwakened = false;
        private Action<Telegram> _killDelegate;

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
            MessageDispatcher2.Instance.StartListening("Kill", GetId(), _killDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("Kill", GetId(), -1);
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

        public virtual void Init()
        {
            if (_componentRepository == null)
                SetComponentRepository();

            if (_componentRepository != null)
                _componentRepository.Components.RegisterComponent<T>(this as T);
        }

        public override void Destroy()
        {
            base.Destroy();
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

        public IComponentRepository GetComponentRepository()
        {
            return _componentRepository;
        }
    }
}
