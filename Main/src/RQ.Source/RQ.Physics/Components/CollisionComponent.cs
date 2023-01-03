using System;
using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Enum;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Physics.Data;
using System.Collections.Generic;
using RQ.Common.Container;
using UnityEngine;

namespace RQ.Physics.Components
{
    /// <summary>
    /// Component for taking and receiving damage and collision/trigger hits
    /// </summary>
    [AddComponentMenu("RQ/Components/Collision Component")]
    public class CollisionComponent : ComponentPersistence<CollisionComponent>, ICollisionComponent
    {
        [SerializeField]
        private CollisionData _collisionData = null;
        //[SerializeField]
        //private SingleUnityLayer _levelOneLayer;
        //[SerializeField]
        //private SingleUnityLayer _levelTwoLayer;
        //[SerializeField]
        //private SingleUnityLayer _levelThreeLayer;
        //[SerializeField]
        //private SingleUnityLayer _sharedLayer;
        [SerializeField]
        private Transform _mainCollider = null;
        [SerializeField]
        private bool _autoChangeLayers = false;

        private bool _isFirstPass = true;

        private PhysicsComponent _physicsComponent;
        private List<Collider2D> _colliders2D;
        private List<Collider> _colliders3D;
        //private long _enableColliderId;
        //private long _enableColliderId2;
        private long _setDeflectingId, _enableCollisionComponentId;
        private Action<Telegram2> _setDeflectingDelegate, _enableCollisionComponentDelegate;

        private PhysicsComponent PhysicsComponent
        {
            get
            {
                if (_physicsComponent == null)
                    _physicsComponent = base._componentRepository.Components.GetComponent<PhysicsComponent>();
                return _physicsComponent;
            }
        }

        public override void Awake()
        {
            base.Awake();
            _setDeflectingDelegate = (data) =>
            {
                bool value;
                if (data.ExtraInfo is string)
                    value = (string) data.ExtraInfo == "1" ? true : false;
                else
                    value = (bool) data.ExtraInfo;

                if (_collisionData.DeflectingCollider)
                    _collisionData.CurrentlyDeflecting = value;
            };
            _enableCollisionComponentDelegate = (data) =>
            {
                bool value;
                if (data.ExtraInfo is string)
                    value = (string) data.ExtraInfo == "1" ? true : false;
                else
                    value = (bool) data.ExtraInfo;

                _collisionData.IsEnabled = value;
            };
            //_enableColliderDelegate = (data) =>
            //{
            //    var enable = (bool) data.ExtraInfo;
            //    _collisionData.IsEnabled = enable;
            //    EnableColliders(enable);
            //};
        }

        public override void Init()
        {
            base.Init();
            if (_mainCollider == null)
                _mainCollider = this.transform;
            if (!Application.isPlaying)
                return;
            //if (_collisionData._mainCollider == null)
            //    _collisionData._mainCollider = this.transform;
            CreateColliderList();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (!Application.isPlaying)
                return;
            _collisionData.IsEnabled = true;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (!Application.isPlaying)
                return;
            _collisionData.IsEnabled = false;
        }

        private void CreateColliderList()
        {
            _colliders2D = new List<Collider2D>();
            _colliders3D = new List<Collider>();

            var colliders = _mainCollider.GetComponents<Collider2D>();
            _colliders2D.AddRange(colliders);
            var colliders3D = _mainCollider.GetComponents<Collider>();
            _colliders3D.AddRange(colliders3D);

        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            if (_collisionData == null)
                _collisionData = new CollisionData();

            //if (!GameDataController.Instance.LoadingGame)
            //    SetLevel(this._collisionData._levelLayer);
        }

        public override void StartListening()
        {
            base.StartListening();
            _setDeflectingId = MessageDispatcher2.Instance.StartListening("SetDeflecting", _componentRepository.UniqueId, _setDeflectingDelegate);
            //_componentRepository.StartListening("SetDeflecting", this.UniqueId, );
            _enableCollisionComponentId = MessageDispatcher2.Instance.StartListening("EnableCollisionComponent", _componentRepository.UniqueId, 
                _enableCollisionComponentDelegate);
            //_componentRepository.StartListening("EnableCollisionComponent", this.UniqueId, );
            //_enableColliderId = MessageDispatcher2.Instance.StartListening("EnableCollider", GetComponentRepository().UniqueId, _enableColliderDelegate);
            //_enableColliderId2 = MessageDispatcher2.Instance.StartListening("EnableCollider", this.UniqueId, _enableColliderDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetDeflecting", _componentRepository.UniqueId, _setDeflectingId);
            //_componentRepository.StopListening("SetDeflecting", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("EnableCollisionComponent", _componentRepository.UniqueId, _enableCollisionComponentId);
            //_componentRepository.StopListening("EnableCollisionComponent", this.UniqueId);
            //MessageDispatcher2.Instance.StopListening("EnableCollider", GetComponentRepository().UniqueId, _enableColliderId);
            //MessageDispatcher2.Instance.StopListening("EnableCollider", this.UniqueId, _enableColliderId2);
        }

        //public override void Update()
        //{
        //    base.Update();

        //    if (_isFirstPass)
        //    {

        //        _isFirstPass = false;
        //    }
        //}        
        
        public CollisionData GetCollisionData()
        {
            return _collisionData;
        }

        //private void SetLevel(LevelLayer levelLayer)
        //{
        //    //base.SetLevel(levelLayer);
        //    _collisionData._levelLayer = levelLayer;

        //    //string layerName;
        //    SingleUnityLayer layer;

        //    //float newZ = 0;
        //    //var entityUI = GetEntityUI();
        //    //var adjustDepthByY = GetComponent<AdjustDepthByY>();
        //    if (GameDataController.Instance.Data == null)
        //        return;
        //    var currentScene = GameDataController.Instance.Data.CurrentScene;

        //    if (currentScene == null)
        //        throw new Exception("No current scene, cannot set level height.");

        //    //float newZIndex = 0f;
        //    int level = 0;

        //    switch (levelLayer)
        //    {
        //        case LevelLayer.LevelOne:
        //            //layerName = "Level 1 NC";
        //            layer = _levelOneLayer;
        //            level = 1;
        //            //newZIndex = currentScene.LevelOneZIndex;
        //            //newZ = _levelOneZIndex;
        //            break;
        //        case LevelLayer.LevelTwo:
        //            level = 2;
        //            //newZIndex = currentScene.LevelTwoZIndex;
        //            layer = _levelTwoLayer;
        //            //layerName = "Level 2 NC";
        //            //newZ = _levelTwoZIndex;
        //            break;
        //        case LevelLayer.LevelThree:
        //            level = 3;
        //            //newZIndex = currentScene.LevelThreeZIndex;
        //            layer = _levelThreeLayer;
        //            break;
        //        case LevelLayer.Shared:
        //            level = 2;
        //            // TODO Improve this by separating the physical level to the collision level
        //            //newZIndex = currentScene.LevelTwoZIndex;
        //            layer = _sharedLayer;
        //            break;
        //        default:
        //            layer = _levelOneLayer;
        //            //layerName = string.Empty;
        //            break;
        //    }

        //    // So, level 1 = 0
        //    // leve 2 = -1
        //    // etc
        //    var newZIndex = -(level - 1);

        //    if (PhysicsComponent != null)
        //    {
        //        PhysicsComponent.SetZOffsetByLevel(newZIndex);
        //    }
        //    else
        //    {
        //        var pos = _componentRepository.transform.position;
        //        _componentRepository.transform.position = new Vector3(pos.x, pos.y, newZIndex);
        //    }

        //    //SetZIndex(newZ);
        //    //if (adjustDepthByY != null)
        //    //    adjustDepthByY.SetOriginalZ(newZ);

        //    //Rigidbody2D.
        //    var mainCollider = _mainCollider;
        //    if (_autoChangeLayers && mainCollider != null && mainCollider.gameObject != null)
        //        mainCollider.gameObject.layer = layer.LayerIndex;
        //        //mainCollider.gameObject.layer = LayerMask.NameToLayer(layerName);
        //}

        public bool CheckCollisionBasedOnLayer(Collider otherCollider)
        {
            var otherFloorComponent = otherCollider.GetComponent<FloorComponent>();
            if (otherFloorComponent == null)
            {
                if (otherCollider.attachedRigidbody == null)
                    return false;
                var otherComponentRepo = otherCollider.attachedRigidbody.transform.GetComponent<IComponentRepository>();
                otherFloorComponent = otherComponentRepo.Components.GetComponent<FloorComponent>();
            }

            // Could not find other Collision Component
            if (otherFloorComponent == null)
                return false;

            var otherLevel = otherFloorComponent.GetLevel();
            var thisFloorComponent = _componentRepository.Components.GetComponent<FloorComponent>();
            var thisLevel = thisFloorComponent.GetLevel();

            if (otherLevel == LevelLayer.Shared || thisLevel == LevelLayer.Shared)
                return true;

            return thisLevel == otherLevel;
        }

        //public IEnumerable<RaycastHit2D> RaycastCheck2D(string spriteBaseComponentUniqueId)
        //{
        //    var thisPos = (Vector2)GetWorldPos();
        //    var otherPos = GetWorldPosition(spriteBaseComponentUniqueId);

        //    var direction = otherPos - thisPos;
        //    //throw new Exception("Physics2D function called - not suppported.");
        //    var raycasts = Physics2D.RaycastAll(thisPos, direction, direction.magnitude).AsEnumerable();
        //    //raycasts = raycasts.Where(i => !_colliders.Contains(i.collider));
        //    List<RaycastHit2D> newList = new List<RaycastHit2D>();
        //    foreach (var raycast in raycasts)
        //    {
        //        bool sameTransform = raycast.transform == gameObject.transform;
        //        if (!sameTransform)
        //            newList.Add(raycast);
        //    }
        //    //raycasts = raycasts.Where(i => i.transform != gameObject.transform);
        //    // TODO Add the layer mask - don't want level one items hitting level two items
        //    return newList;
        //}

        //public IEnumerable<RaycastHit> RaycastCheck(string spriteBaseComponentUniqueId, int mask)
        /// <summary>
        /// Simple raycast check between two entities given the layer mask. For example, you can check for obstacles between the two entites by 
        /// including an Obstacle layer mask
        /// </summary>
        /// <param name="otherSpriteBaseComponentUniqueId"></param>
        /// <param name="mask"></param>
        /// <param name="raycastHit"></param>
        /// <returns></returns>
        public bool RaycastCheck(string otherSpriteBaseComponentUniqueId, int mask, out RaycastHit raycastHit)
        {
            var thisPos = GetWorldPos();
            var otherPos = GetWorldPosition(otherSpriteBaseComponentUniqueId);

            var direction = otherPos - thisPos;
            //throw new Exception("Physics2D function called - not suppported.");
            // TODO Change to Raycast
            //var raycasts = UnityEngine.Physics.RaycastAll(thisPos, direction, direction.magnitude).AsEnumerable();
            //var raycast = UnityEngine.Physics.RaycastNonAlloc()
            var ray = new Ray(thisPos, direction);
            var raycast = UnityEngine.Physics.Raycast(ray, out raycastHit, direction.magnitude, mask);
            //raycasts = raycasts.Where(i => !_colliders.Contains(i.collider));
            //List<RaycastHit> newList = new List<RaycastHit>();
            //foreach (var raycast in raycasts)
            //{
            //    bool sameTransform = raycast.transform == gameObject.transform;
            //    if (!sameTransform)
            //        newList.Add(raycast);
            //}
            //raycasts = raycasts.Where(i => i.transform != gameObject.transform);
            // TODO Add the layer mask - don't want level one items hitting level two items
            //return newList;
            return raycast;
        }

        public Vector3 GetWorldPosition(string spriteBaseComponentUniqueId)
        {
            var entity = EntityContainer._instance.GetEntity(spriteBaseComponentUniqueId);
            //Vector2D otherPos = Vector2D.Zero();
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, spriteBaseComponentUniqueId,
            //    Enums.Telegrams.GetWorldPos, null, (pos) => otherPos = (Vector2D)pos);
            //return otherPos;
            return entity.transform.position;
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;
            switch (msg.Msg)
            {
                case Telegrams.SetEnabled:
                    var enable = (bool)msg.ExtraInfo;
                    _collisionData.IsEnabled = enable;
                    EnableColliders(enable);
                    if (!enable)
                    {
                        var usableComponent = GetComponent<IUsableComponent>();
                        usableComponent?.RemoveFromUsableContainer();
                    }
                    break;
            }
            return false;
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var boxCollider = _mainCollider.GetComponent<BoxCollider2D>();
        //    if (boxCollider != null)
        //    {
        //        _collisionData.ColliderOffset = boxCollider.offset;
        //        _collisionData.ColliderSize = boxCollider.size;
        //        _collisionData.IsColliderTrigger = boxCollider.isTrigger;
        //    }
        //    var circleCollider = _mainCollider.GetComponent<CircleCollider2D>();
        //    if (circleCollider != null)
        //    {
        //        _collisionData.ColliderOffset = circleCollider.offset;
        //        _collisionData.ColliderRadius = circleCollider.radius;
        //        _collisionData.IsColliderTrigger = circleCollider.isTrigger;
        //    }
        //    base.SerializeComponent(entitySerializedData, _collisionData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    // In case the object is disabled, we want this list created in all cases.
        //    //CreateColliderList();
        //    _collisionData = base.DeserializeComponent<CollisionData>(entitySerializedData);
        //    var boxCollider = _mainCollider.GetComponent<BoxCollider2D>();
        //    if (boxCollider != null)
        //    {
        //        boxCollider.offset = _collisionData.ColliderOffset;
        //        boxCollider.size = _collisionData.ColliderSize;
        //        boxCollider.isTrigger = _collisionData.IsColliderTrigger;
        //    }
        //    var circleCollider = _mainCollider.GetComponent<CircleCollider2D>();
        //    if (circleCollider != null)
        //    {
        //        circleCollider.offset = _collisionData.ColliderOffset;
        //        circleCollider.radius = _collisionData.ColliderRadius;
        //        circleCollider.isTrigger = _collisionData.IsColliderTrigger;
        //    }
        //    SetLevel(this._collisionData._levelLayer);
        //    EnableColliders(_collisionData.IsEnabled);
        //}

        public void EnableColliders(bool enable)
        {
            if (_colliders2D != null)
            {
                //Debug.LogError("No colliders in " + _componentRepository.name + " " + this.name);
                foreach (var collider in _colliders2D)
                {
                    // Not sure why this if statement was here?!
                    //if (!collider.isTrigger)
                    collider.enabled = enable;
                    //collider.gameObject.SetActive(enable);
                }
            }
            if (_colliders3D != null)
            {
                //Debug.LogError("No colliders in " + _componentRepository.name + " " + this.name);
                foreach (var collider in _colliders3D)
                {
                    // Not sure why this if statement was here?!
                    //if (!collider.isTrigger)
                        collider.enabled = enable;
                    //collider.gameObject.SetActive(enable);
                }
            }
        }

        //public LayerMask GetEnvironmentLayerMask()
        //{
        //    var floorComponent = _componentRepository.Components.GetComponent<FloorComponent>();
        //    var level = floorComponent.GetLevel();
        //    return GameDataController.Instance.GetLayerMask(level);
        //}

        public bool ContainsCollider(Collider2D collider)
        {
            return _colliders2D.Contains(collider);
        }

        public bool ContainsCollider(Collider collider)
        {
            return _colliders3D.Contains(collider);
        }

        public float GetColliderRadius()
        {
            if (_colliders3D == null || _colliders3D.Count == 0)
                return 0f;
            SphereCollider sphereCollider = _colliders3D[0] as SphereCollider;
            if (sphereCollider == null)
                return 0f;
            return sphereCollider.radius;
        }

        public bool ReceivesDamage()
        {
            return _collisionData.ReceivesDamage;
        }
    }
}
