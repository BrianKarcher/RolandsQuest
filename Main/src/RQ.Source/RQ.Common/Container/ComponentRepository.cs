using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Common.Container
{
    public class ComponentRepository : MessagingObject, IMessagingObject, IComponentRepository, IEntity
    {
        [SerializeField]
        private bool _recreateOnLoadGame = true;

        [SerializeField]
        protected bool _addToEntityContainer = true;
        public bool AddToEntityContainer { get { return _addToEntityContainer; } }
        private ComponentRegistrar _components;
        //private Dictionary<string, List<KeyValuePair<string, Action<Telegram2>>>> _messageRelay;
        const string _idToCheck = "8da840b9-43f6-4bae-9e63-a3ceee1cb493";

        [HideInInspector]
        public IComponentRegistrar Components
        {
            get
            {
                if (_components == null)
                {
                    _components = new ComponentRegistrar();
                    //_components.PerformUnregister = (uniqueId => _unregisterComponents.Add(uniqueId));
                    //_components.PerformRegister = (uniqueId, component) =>
                    //    {
                    //        _registerComponents.Add(component);
                    //    };
                }
                return _components;
            }            
        }

        public ComponentRepository()
        {
            Components.SetComponentRepository(this);
        }

        public override void Init()
        {
            if (name.Contains("BombExplosion"))
            {
                //Log.Info("Repo Init (Bomb Explosion)");
                int i = 1;
            }
            base.Init();
            //if (!Application.isPlaying)
            //    return;
            var components = SearchForComponents();
            //Debug.Log(this.name + " Found " + components.Count() + " entities");
            Components.RegisterComponents(components);
            InitComponents(components);
        }

        public void Reset()
        {
            //foreach (var component in Components.GetComponents())
            //{
            for (int i = 0; i < Components.GetComponents().Count; i++)
            {
                var component = Components.GetComponents()[i];
                (component as IComponentBase)?.Reset();
            }
        }

        public void ReAwaken()
        {
            //foreach (var component in Components.GetComponents())
            var components = Components.GetComponents();
            for (int i = 0; i < components.Count; i++)
            {
                (components[i] as IComponentBase)?.ReAwaken();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();           

            if (!Application.isPlaying)
                return;
            var name = this.name;
            if (UniqueId == _idToCheck)
            {
                Debug.LogWarning("OnDestroy called on " + _idToCheck);
            }
            StopListening();
            DestroyComponents();

            if (_addToEntityContainer)
                EntityContainer._instance.RemoveEntity(this);
        }

        /// <summary>
        /// Unline OnDestroy, this gets called whether or not the object has awoken
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();
            if (UniqueId == "8da840b9-43f6-4bae-9e63-a3ceee1cb493")
            {
                int i = 1;
            }
            if (UniqueId == _idToCheck)
            {
                Debug.LogWarning("Destroy called on " + _idToCheck);
            }

            StopListening();
            DestroyComponents();
            if (this.transform.gameObject != null)
            {
                this.transform.gameObject.SetActive(false);
                GameObject.Destroy(this.transform.gameObject);
            }

            if (_addToEntityContainer)
                EntityContainer._instance.RemoveEntity(this);
            // OnDestroy does not get called on objects that were never active, so stop listening now
            //            
        }

        private void DestroyComponents()
        {
            var components = Components.GetComponents();
            //Debug.Log(this.name + " Killing " + components.Count + " entities");
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (MessageDispatcher2.Instance.IsListening("Kill", component.UniqueId))
                    MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, this.UniqueId, component.UniqueId, null);
            }
        }

        public virtual void InitComponents(IEnumerable<IComponentBase> components)
        {
            //Debug.Log(this.name + " Initing " + components.Count() + " entities");
            foreach (var component in components)
            {
                component.Init();
            }
        }

        private IList<IComponentBase> SearchForComponents()
        {
            List<IComponentBase> components = new List<IComponentBase>(20);
            ProcessComponents(components, GetComponentsInChildren<IComponentBase>(true));
            return components;
        }

        private void ProcessComponents(List<IComponentBase> componentList, 
            IList<IComponentBase> components)
        {
            if (components == null)
                return;

            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                if (component.GetComponentRepository() == this)
                    componentList.Add(component);
                //components = components.Where(i => i.GetComponentRepository() == this);
                //if (components == null)
                //    return;

                //componentList.AddRange(components);
            }


        }

        //public Dictionary<string, List<KeyValuePair<string, Action<Telegram2>>>> MessageRelay
        //{
        //    get
        //    {
        //        if (_messageRelay == null)
        //            _messageRelay = new Dictionary<string, List<KeyValuePair<string, Action<Telegram2>>>>();
        //        return _messageRelay;
        //    }
        //}

        //public override void Awake()
        //{
        //    base.Awake();
        //}

        //public virtual void StartListening(string eventName, string id, Action<Telegram2> callbackMethod, bool addLocal = false)
        //{
        //    if (!Application.isPlaying)
        //        return;
        //    List<KeyValuePair<string, Action<Telegram2>>> itemsInEvent;
        //    if (!MessageRelay.TryGetValue(eventName, out itemsInEvent))
        //    {
        //        itemsInEvent = new List<KeyValuePair<string, Action<Telegram2>>>();
        //        MessageRelay.Add(eventName, itemsInEvent);
        //    }

        //    // First item added?  Start listening so we can relay the message
        //    // and StartListening has already been called, manually start the listener
        //    if (itemsInEvent.Count == 0 && base.IsListening())
        //        AddToMessenger(eventName);

        //    bool found = false;
        //    for (int i = 0; i < itemsInEvent.Count; i++)
        //    {
        //        var itemInEvent = itemsInEvent[i];
        //        if (itemInEvent.Key == id)
        //        {
        //            found = true;
        //            break;
        //        }
        //    }
        //    if (!found)
        //        itemsInEvent.Add(new KeyValuePair<string, Action<Telegram2>>(id, callbackMethod));
        //    if (addLocal)
        //        MessageDispatcher2.Instance.StartListening(eventName, id, callbackMethod);
        //}

        //public void StopListening(string eventName, string id, bool removeLocal = false)
        //{
        //    if (!Application.isPlaying)
        //        return;
        //    if (removeLocal)
        //        MessageDispatcher2.Instance.StopListening(eventName, id, -1);
        //    //List<KeyValuePair<string, Action<Telegram2>>> itemsInEvent;
        //    if (!MessageRelay.TryGetValue(eventName, out var itemsInEvent))
        //    {
        //        return;
        //        //throw new Exception("Could not locate event " + eventName);
        //    }
        //    //if (itemsInEvent.Any(i => i.Key == id))
        //    //    itemsInEvent.Remove(id);
        //    //var intList = ObjectPool.Instance.PullFromPool<List<int>>(ObjectPoolType.IntList);
        //    //for (int i = 0; i < itemsInEvent.Count; i++)
        //    //{
                
        //    //}
        //    for (int i = itemsInEvent.Count - 1; i >= 0; i--)
        //    {
        //        var item = itemsInEvent[i];
        //        if (item.Key == id)
        //        {
        //            itemsInEvent.RemoveAt(i);
        //        }
        //    }
        //    //itemsInEvent.RemoveAll(i => i.Key == id);
        //    if (itemsInEvent.Count == 0)
        //    {
        //        RemoveFromMessenger(eventName);
        //    }
        //}

        //public override void StartListening()
        //{
        //    base.StartListening();
        //    //if (name == "UI Manager")
        //    //{
        //    //    int i = 1;
        //    //}
        //    foreach (var message in MessageRelay)
        //    {
        //        AddToMessenger(message.Key);
        //    }
        //}

        //public override void StopListening()
        //{
        //    base.StopListening();
        //    //if (name == "UI Manager")
        //    //{
        //    //    int i = 1;
        //    //}
        //    foreach (var message in MessageRelay)
        //    {
        //        RemoveFromMessenger(message.Key);
        //    }
        //}

        //private void AddToMessenger(string eventName)
        //{
        //    MessageDispatcher2.Instance.StartListening(eventName, this.UniqueId, (data) =>
        //    {
        //        //if (this == null)
        //        //    Debug.LogError(name + " is null in the Messaging System");
        //        var itemsInEvent = MessageRelay[eventName];
        //        if (itemsInEvent == null)
        //            return;
        //        for (int i = itemsInEvent.Count - 1; i >= 0; i--)
        //        {
        //            itemsInEvent[i].Value(data);
        //        }
        //    });
        //}

        //private void RemoveFromMessenger(string eventName)
        //{
        //    MessageDispatcher2.Instance.StopListening(eventName, this.UniqueId, -1);
        //}

        public void SendMessageToAllButThis(float delay, string senderId, Telegrams msg, object extraInfo)
        {
            var allComponents = Components.GetComponents();
            for (int i = 0; i < allComponents.Count; i++)
            {
                var component = allComponents[i];
                if (component.UniqueId == senderId)
                    continue;
                if (component.IsActive)
                    MessageDispatcher.Instance.DispatchMsg(delay, senderId, component.UniqueId, msg, extraInfo);
            }


            //var components = Components.GetComponents().Where(i => i.UniqueId != senderId);
            //foreach (var component in components)
            //{
            //    if (component.IsActive)
            //        MessageDispatcher.Instance.DispatchMsg(delay, senderId, component.UniqueId, msg, extraInfo);
            //}
        }

        public void SendMessageToComponents<T>(float delay, string senderId, Telegrams msg, object extraInfo)
             where T : class, IBaseObject
        {
            var components = Components.GetComponents<T>();
            foreach (var component in components)
            {
                if (component.IsActive)
                    MessageDispatcher.Instance.DispatchMsg(delay, senderId, component.UniqueId, msg, extraInfo);
            }
        }

        public override bool HandleMessage(Telegram msg)
        {
            base.HandleMessage(msg);

                        // Component repositories relay all of the messages around to the components
            var components = Components.GetComponents();
            // TODO Inactive objects still do not have components!!!!!!!!!!!!!!!
            // TODO How can we deserialize that?!?!?!?!?
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
            //var components = _components.GetComponents();
            //foreach (var component in components)
            //{
                // We cannot send messages to disabled comopnents - they are not in
                // the message system
                if (component.IsActive)
                {
                    MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(0f, msg.SenderId,
                        component.UniqueId, msg.Msg, msg.ExtraInfo, msg.Act, msg.EarlyTermination);
                }
            }

            return false;
        }

        public bool GetRecreateOnLoadGame()
        {
            return _recreateOnLoadGame;
        }

        public override void SetUniqueId(string uniqueId, bool force = false)
        {
            if (uniqueId == _idToCheck)
            {
                Debug.LogWarning("SetUniqueId called, setting id from " + _uniqueId + " to " + uniqueId);
            }
            base.SetUniqueId(uniqueId, force);
        }
    }
}
