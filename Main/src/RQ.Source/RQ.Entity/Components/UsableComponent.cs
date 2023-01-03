using RQ.Common;
using RQ.Common.Components;
using RQ.Common.Container;
using RQ.Common.Controllers;
using RQ.Common.EventChain;
using RQ.Entity.Common;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.GameData.StoryProgress;
using System;
using System.Collections.Generic;
using RQ.Controller.Contianers;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Usable")]
    public class UsableComponent : ComponentPersistence<UsableComponent>, IUsableComponent
    {
        [SerializeField]
        [Tag]
        private List<string> _tags;

        /// <summary>
        /// Excluded tags can include things like the tilemap if you want to check the raycast between
        /// sprites and want the tilemap to stop the raycast.
        /// </summary>
        [SerializeField]
        [Tag]
        private List<string> _excludedTags;

        [SerializeField]
        private LayerMask _obstacleLayerMask = 0;

        [SerializeField]
        private string _message;

        [SerializeField]
        private string _bubbleText;

        [SerializeField]
        private bool _castRay = false;

        [SerializeField]
        private Transform _usable = null;

        [SerializeField]
        private SpriteBaseComponent[] _usableObjects;

        [SerializeField]
        private bool _autoUse = true;

        [SerializeField]
        private string _messageToUser;
        
        [SerializeField]
        private bool _isEnabled = true;

        //[SerializeField]
        //private bool _isInRange = false;

        private CollisionComponent _collisionComponent;
        private long _enableUsableId;
        private long _disableUsableId;

        [SerializeField]
        private StorySceneConfig _setStorySceneConfig;

        private Transform _requester;

        private long _setGameProgressId, _useUsableId, _enableComponentId, _enableComponentId2, _setGameProgressId2;

        private Action<Telegram2> _enableUsableDelegate, _disableUsableDelegate, _setGameProgressDelegate, _useUsableDelegate, _enableComponentDelegate;

        public override void Awake()
        {
            base.Awake();
            _enableUsableDelegate = (data) =>
            {
                _isEnabled = true;
                //AddToUsableContainer();
            };
            _disableUsableDelegate = (data) =>
            {
                _isEnabled = false;
                RemoveFromUsableContainer();
                //AddToUsableContainer();
            };
            _setGameProgressDelegate = (data) =>
            {
                if (_setStorySceneConfig != null)
                    GameDataController.Instance.Data.SetStoryProgress(_setStorySceneConfig);
            };
            _useUsableDelegate = (data) =>
            {
                if (!_autoUse)
                    return;
                var spriteUniqueId = (string) data.ExtraInfo;
                Trigger(spriteUniqueId);
            };
            _enableComponentDelegate = (data) =>
            {
                bool value;
                if (data.ExtraInfo is string)
                    value = (string) data.ExtraInfo == "1" ? true : false;
                else
                    value = (bool) data.ExtraInfo;

                _isEnabled = value;
                if (!_isEnabled)
                    RemoveFromUsableContainer();
            };
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;

            // First try to find the Collision Component in the same game object.
            _collisionComponent = GetComponent<CollisionComponent>();
            if (_collisionComponent == null)
                _collisionComponent = base._componentRepository.Components.GetComponent<CollisionComponent>();
        }

        public override void StartListening()
        {
            base.StartListening();
            _enableUsableId = MessageDispatcher2.Instance.StartListening("EnableUsable", _componentRepository.UniqueId, _enableUsableDelegate);
            _disableUsableId = MessageDispatcher2.Instance.StartListening("DisableUsable", _componentRepository.UniqueId, _disableUsableDelegate);
            _setGameProgressId = MessageDispatcher2.Instance.StartListening("SetGameProgress", _componentRepository.UniqueId, _setGameProgressDelegate);
            _setGameProgressId2 = MessageDispatcher2.Instance.StartListening("SetGameProgress", this.UniqueId, _setGameProgressDelegate);
            //_componentRepository.StartListening("SetGameProgress", this.UniqueId, , true);
            _useUsableId = MessageDispatcher2.Instance.StartListening("UseUsable", _componentRepository.UniqueId, _useUsableDelegate);
            //_componentRepository.StartListening("UseUsable", this.UniqueId, );
            _enableComponentId = MessageDispatcher2.Instance.StartListening("EnableComponent", _componentRepository.UniqueId, _enableComponentDelegate);
            _enableComponentId2 = MessageDispatcher2.Instance.StartListening("EnableComponent", this.UniqueId, _enableComponentDelegate);
            //_componentRepository.StartListening("EnableComponent", this.UniqueId, , true);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetGameProgress", _componentRepository.UniqueId, _setGameProgressId);
            MessageDispatcher2.Instance.StopListening("SetGameProgress", this.UniqueId, _setGameProgressId2);
            //_componentRepository.StopListening("SetGameProgress", this.UniqueId, true);
            MessageDispatcher2.Instance.StopListening("UseUsable", _componentRepository.UniqueId, _useUsableId);
            //_componentRepository.StopListening("UseUsable", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("EnableComponent", _componentRepository.UniqueId, _enableComponentId);
            MessageDispatcher2.Instance.StopListening("EnableComponent", this.UniqueId, _enableComponentId2);
            //_componentRepository.StopListening("EnableComponent", this.UniqueId, true);
            MessageDispatcher2.Instance.StopListening("EnableUsable", _componentRepository.UniqueId, _enableUsableId);
            MessageDispatcher2.Instance.StopListening("DisableUsable", _componentRepository.UniqueId, _disableUsableId);
        }

        public void Trigger(string requesterUniqueId)
        {
            var user = EntityContainer._instance.GetEntity(requesterUniqueId);
            _requester = user.transform;
            if (!String.IsNullOrEmpty(_messageToUser))
                MessageDispatcher2.Instance.DispatchMsg(_messageToUser, 0f, this.UniqueId, user.UniqueId, _componentRepository.gameObject);
            GameDataController.Instance.LastUsedEntity = _componentRepository;
            gameObject.SendMessage("OnUse", _requester, SendMessageOptions.DontRequireReceiver);
            //MessageDispatcher2.Instance.DispatchMsg("OnUse", 0f, this.UniqueId, requesterUniqueId, null);

            for (int i = 0; i < _usableObjects.Length; i++)
            {
                var usableObject = _usableObjects[i];
                if (usableObject == null)
                    continue;
                MessageDispatcher2.Instance.DispatchMsg("OnUse", 0f, this.UniqueId, usableObject.UniqueId, null);
            }

            if (_usableObjects != null)
            {
                var eventChain = CreateEventChain();
                eventChain.StartChain(_requester);
                //ProcessNextUsable();
            }

            //ConversationTrigger trigger = GetComponent<ConversationTrigger>();
            // @todo implement this with the new dialog system
            //_gameManagerScript = gameManager.GetComponent<GameManager>();
            //_gameManagerScript.StartDialog (DialogName);

            //Lua.IsTrue();

            //DialogueManager.StartConversation(trigger.conversation);
            //trigger.condition.IsTrue(EntityContainer._instance.GetMainCharacter().transform);
        }

        //private void ProcessNextUsable()
        //{
        //    if (_usableIndex > _usableObjects.Length - 1)
        //    {
        //        _usableIndex = 0;
        //        _processingUsables = false;
        //        return;
        //    }
        //    var processUsable = _usableObjects[_usableIndex];
        //    var trigger = processUsable.GetComponent<IEventTrigger>();
        //    trigger.Trigger(_requester);
        //    //foreach (var component in _usableObjects)
        //    //{
        //    //processUsable.SendMessage("OnUse", _requester, SendMessageOptions.DontRequireReceiver);
        //    //}
        //}

        private EventChain CreateEventChain()
        {
            EventChain chain = new EventChain(_usableObjects);
            chain.ChainComplete += (sender, e) => MessageDispatcher2.Instance.DispatchMsg("EventChainComplete", 0f, 
                this.UniqueId, _componentRepository.UniqueId, null);
            return chain;
            //foreach (var usableObject in _usableObjects)
            //{
            //    var eventTrigger = usableObject.GetComponent<IEventTrigger>();
            //    chain.Add(eventTrigger);
            //}
        }

        //public void OnCollisionStay2D(Collision2D other)
        //{
        //    if (other.collider.tag == "Player")
        //    {
        //        int i = 0;
        //    }
        //    Trigger(other.collider);
        //}

        //public void OnCollisionStay(UnityEngine.Collision collision)
        //{
        //    if (!isActiveAndEnabled)
        //        return;
        //    Trigger(collision.collider);
        //}

        //public void OnTriggerStay(Collider other)
        //{
        //    if (!isActiveAndEnabled)
        //        return;
        //    Trigger(other);
        //}

        public void OnTriggerEnter(Collider other)
        {
            if (!isActiveAndEnabled)
                return;
            Trigger(other);
        }

        //public void OnTriggerStay2D(Collider2D other)
        //{
        //    Trigger2D(other);
        //}

        //private void Trigger2D(Collider2D other)
        //{
        //    if (!_isEnabled)
        //        return;

        //    //if (_isInRange)
        //    //    return;

        //    if (other.attachedRigidbody == null)
        //        return;

        //    // Ignore collisions with self
        //    //if (other.name == this.name)
        //    //    return;

        //    if (!_tags.Contains(other.attachedRigidbody.tag))
        //        return;

        //    if (_castRay && !CheckRaycast(other))
        //    {
        //        RemoveFromUsableContainer();
        //        return;
        //    }

        //    AddToUsableContainer();
        //}

        private void Trigger(Collider other)
        {
            if (!_isEnabled)
                return;

            //if (_isInRange)
            //    return;

            if (other.attachedRigidbody == null)
                return;

            // Ignore collisions with self
            //if (other.name == this.name)
            //    return;

            if (!_tags.Contains(other.attachedRigidbody.tag))
                return;

            if (_castRay && !CheckRaycast(other))
            {
                RemoveFromUsableContainer();
                return;
            }

            AddToUsableContainer();
        }

        private bool CheckRaycast(Collider other)
        {

            if (_collisionComponent == null)
                throw new Exception("No physics component found for " + this.name);

            var otherBaseComponent = other.attachedRigidbody.GetComponent<SpriteBaseComponent>();
            if (otherBaseComponent == null)
                return false;
            //var otherPhysicsComponet = otherBaseComponent.Components.GetComponent<PhysicsComponent>();

            var hitObstacle = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId, _obstacleLayerMask, out var raycastHit);

            // Exclude colliders and triggers belonging to this object
            //var includedColliders = raycastObjects.Where(i => i.collider.name != this.name);

            //var includedColliders = raycastObjects.Where(i => _tags.Contains(i.collider.tag));

            //bool hasExcludedColliders = RaycastHasExcludedColliders(raycastObjects);
            //bool hasExcludedColliders = RaycastHasExcludedColliders(raycastHit);

            //var hasExcludedColliders = raycastObjects.Any(i => _excludedTags.Contains(i.collider.tag));

            //var closestIncludedHit = GetClosestRaycastHit(raycastObjects, _tags);

            //var closestExcludedHit = GetClosestRaycastHit(raycastObjects, _excludedTags);

            //var excludedColliders = raycastObjects.Where(i => _excludedTags.Contains(i.collider.tag));

            //if (includedColliders.Any(i => i.collider == other))
            //    return true;

            //return false;

            //return closestIncludedHit.distance < closestExcludedHit.distance;
            //return !hasExcludedColliders;
            return !hitObstacle;
        }

        //private bool RaycastHasExcludedColliders(IEnumerable<RaycastHit> raycastHits)
        //{
        //    foreach (var raycastObject in raycastHits)
        //    {
        //        if (_excludedTags.Contains(raycastObject.collider.tag))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private bool RaycastHasExcludedColliders(RaycastHit raycastHit)
        {
            if (_excludedTags.Contains(raycastHit.collider.tag))
            {
                return true;
            }
            return false;
        }

        //private bool CheckRaycast(Collider2D other)
        //{

        //    if (_collisionComponent == null)
        //        throw new Exception("No physics component found for " + this.name);

        //    var otherBaseComponent = other.attachedRigidbody.GetComponent<SpriteBaseComponent>();
        //    if (otherBaseComponent == null)
        //        return false;
        //    //var otherPhysicsComponet = otherBaseComponent.Components.GetComponent<PhysicsComponent>();

        //    var hit = _collisionComponent.RaycastCheck(otherBaseComponent.UniqueId, _obstacleLayerMask, out var raycastHit);

        //    // Exclude colliders and triggers belonging to this object
        //    //var includedColliders = raycastObjects.Where(i => i.collider.name != this.name);

        //    //var includedColliders = raycastObjects.Where(i => _tags.Contains(i.collider.tag));

        //    bool hasExcludedColliders = RaycastHasExcludedColliders(raycastObjects);

        //    //var hasExcludedColliders = raycastObjects.Any(i => _excludedTags.Contains(i.collider.tag));

        //    //var closestIncludedHit = GetClosestRaycastHit(raycastObjects, _tags);

        //    //var closestExcludedHit = GetClosestRaycastHit(raycastObjects, _excludedTags);

        //    //var excludedColliders = raycastObjects.Where(i => _excludedTags.Contains(i.collider.tag));

        //    //if (includedColliders.Any(i => i.collider == other))
        //    //    return true;

        //    //return false;

        //    //return closestIncludedHit.distance < closestExcludedHit.distance;
        //    return !hasExcludedColliders;
        //}

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.SetEnabled:
                    var enabled = (bool)msg.ExtraInfo;
                    _isEnabled = enabled;
                    if (!enabled)
                    {
                        RemoveFromUsableContainer();
                    }
                    break;
                case Telegrams.GetChild:
                    msg.Act(_usable);
                    break;
            }
            return false;
        }

        //private List<RaycastHit2D> tempRaycastHitList = new List<RaycastHit2D>();

        //public RaycastHit2D GetClosestRaycastHit(IEnumerable<RaycastHit2D> raycasts, List<string> tags)
        //{
        //    var raycastHit2Ds = ObjectPool.Instance.PullFromPool<List<RaycastHit2D>>(ObjectPoolType.RaycastHit2DList);
        //    foreach (var raycast in raycasts)
        //    {
        //        if (tags.Contains(raycast.collider.tag))
        //        {
        //            raycastHit2Ds.Add(raycast);
        //        }
        //    }
        //    //var includedRaycasts = raycasts.Where(i => tags.Contains(i.collider.tag));
        //    var closestRaycastHit2D = GetClosestRaycastHit(raycastHit2Ds);
        //    ObjectPool.Instance.ReleaseToPool(ObjectPoolType.RaycastHit2DList, raycastHit2Ds);
        //    return closestRaycastHit2D;
        //}

        public RaycastHit2D GetClosestRaycastHit(IEnumerable<RaycastHit2D> raycasts)
        {
            //var closestDistance = 10000f; // an impossibly high number
            RaycastHit2D closestRaycast = new RaycastHit2D() { distance = 10000f };
            foreach (var raycast in raycasts)
            {
                if (raycast.distance < closestRaycast.distance)
                {
                    //closestDistance = raycast.distance;
                    closestRaycast = raycast;
                }
            }
            return closestRaycast;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveFromUsableContainer();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            RemoveFromUsableContainer();
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (!isActiveAndEnabled)
                return;
            if (other.attachedRigidbody == null)
                return;
            // Make sure we respond to the correct object exiting the trigger
            if (!_tags.Contains(other.attachedRigidbody.tag))
                return;

            RemoveFromUsableContainer();
        }

        public void OnCollisionExit(UnityEngine.Collision other)
        {
            if (!isActiveAndEnabled)
                return;
            if (other.collider.attachedRigidbody == null)
                return;
            // Make sure we respond to the correct object exiting the trigger
            if (!_tags.Contains(other.collider.attachedRigidbody.tag))
                return;

            RemoveFromUsableContainer();
        }

        public void OnTriggerExit(Collider other)
        {
            if (!isActiveAndEnabled)
                return;
            if (other.attachedRigidbody == null)
                return;
            // Make sure we respond to the correct object exiting the trigger
            if (!_tags.Contains(other.attachedRigidbody.tag))
                return;

            RemoveFromUsableContainer();
        }

        private void AddToUsableContainer()
        {
            var container = UsableContainerController.Instance.UsableContainer;
            container.Add(base._componentRepository.UniqueId, _bubbleText);
        }

        public void RemoveFromUsableContainer()
        {
            if (!Application.isPlaying)
                return;
            // Data is null on scene switches, in this case just return
            if (GameDataController.Instance.Data == null)
                return;
            if (UsableContainerController.Instance == null)
                return;
            var container = UsableContainerController.Instance.UsableContainer;
            container.Remove(base._componentRepository.UniqueId);
        }

        public string GetBubbleText()
        {
            return _bubbleText;
        }

        //public override void Serialize(EntitySerializedData entityData)
        //{
        //    base.Serialize(entityData);
        //    entityData.IsUsableEnabled = _isEnabled;
        //    base.SerializeComponent(entityData, _usableObjects.Select(i => i.UniqueId));
        //    //var conversationTriggerData
        //    //var conversationTriggers = GetComponents<ConversationTrigger>();
        //    //foreach (var conversationTrigger in conversationTriggers)
        //    //{

        //    //}
        //}

        //public override void Deserialize(EntitySerializedData entityData)
        //{
        //    base.Deserialize(entityData);
        //    _isEnabled = entityData.IsUsableEnabled;
        //    var usableObjectUniqueIds = base.DeserializeComponent<string[]>(entityData);
        //    if (usableObjectUniqueIds != null)
        //    {
        //        Array.Resize(ref _usableObjects, usableObjectUniqueIds.Length);
        //        //_usableObjects = new SpriteBaseComponent[usableObjectUniqueIds.Length];
        //        for (int i = 0; i < usableObjectUniqueIds.Length; i++)
        //        {
        //            var usableObjectUniqueId = usableObjectUniqueIds[i];
        //            _usableObjects[i] = EntityContainer._instance.GetEntity(usableObjectUniqueId).transform.GetComponent<SpriteBaseComponent>();
        //        }
        //    }
        //}
    }
}
