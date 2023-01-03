using RQ.Common;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Extensions;
using RQ.Messaging;
using RQ.Model.Containers;
using RQ.Model.Interfaces;
using RQ.Physics;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.Common
{
    [AddComponentMenu("RQ/Components/Sprite Base")]
    [Serializable]
    public class SpriteBaseComponent : ComponentRepository, ISpriteBase, ISerializableObject
    {
        //every entity has a type associated with it (health, troll, ammo etc)
        public Enums.EntityType EntityType;
        public string EntityPrefabUniqueId;
        [SerializeField]
        private SpriteBaseComponent _parentRepo;
        private HashSet<SpriteBaseComponent> _childSpriteBaseComponents = new HashSet<SpriteBaseComponent>();

        [HideInInspector]
        public bool JustLoaded { get; set; }

        private Action<Telegram2> _killDelegate;

        public override void Awake()
        {
            if (!Application.isPlaying)
            {
                // Make sure basic setup is performed on the object like setting the RqName
                // and Unique Id Registration
                base.Awake();
                return;
            }
            
            base.Awake();
            _killDelegate = (data) =>
            {
                Destroy();
            };

            if (_addToEntityContainer)
                EntityContainer._instance.AddEntity(this);
        }

        public override void Start()
        {
            base.Start();
            // We can safely say this was no longer just loaded from a Save file
            JustLoaded = false;
        }

        /// <summary>
        /// Call this manually since Awake does not get called if an object is not enabled in the scene
        /// </summary>
        public override void Init()
        {
            base.Init();
            //Debug.Log("Init called on " + this.name);
            if (_parentRepo != null)
                _parentRepo.AddChildSpriteBaseComponent(this);
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

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.Serialize:
                    var serializeData = msg.ExtraInfo as EntitySerializedData;
                    this.Serialize(serializeData);
                    break;
                case Telegrams.Deserialize:
                    var deserializeData = msg.ExtraInfo as EntitySerializedData;
                    this.Deserialize(deserializeData);
                    break;
                case Telegrams.Kill:
                    Destroy();
                    break;
                case Telegrams.GetPos:
                    msg.Act((Vector2D)GetPos());
                    break;
                case Telegrams.GetWorldPos:
                    msg.Act((Vector2D)GetWorldPos());
                    break;
                //case Telegrams.GetClosestObjectFromSuppliedList:
                //    var list = msg.ExtraInfo as IEnumerable<string>;
                //    var closestObject = GetClosestObject(list);
                //    msg.Act(closestObject);
                //    return true;
            }

            return false;
        }

        public string GetClosestObject(IList<UsableObjectInfo> uniqueIds)
        {
            string closestUniqueId = string.Empty;
            float closestDistance = 10000000f;

            var thisPos = (Vector2D) GetWorldPos();

            //foreach (var usableObject in uniqueIds)
            for (int i = 0; i < uniqueIds.Count; i++)
            {
                var usableObject = uniqueIds[i];
                //Vector2D usableObjectLocation = Vector2D.Zero();
                var entity = EntityContainer._instance.GetEntity(usableObject.UniqueId);
                var physicsComponent = entity.Components.GetComponent<IBasicPhysicsComponent>();
                var usableObjectLocation = physicsComponent == null ? (Vector2)entity.transform.position : physicsComponent.GetWorldPos2D();
                //MessageDispatcher.Instance.DispatchMsg(0f,
                //    string.Empty, usableObject, RQ.Enums.Telegrams.GetPos, null,
                //    (location) => usableObjectLocation = (Vector2D)location);

                var distanceSq = thisPos.DistanceSq(usableObjectLocation);
                if (distanceSq < closestDistance)
                {
                    // Found a closer object
                    closestDistance = distanceSq;
                    closestUniqueId = usableObject.UniqueId;
                }
            }

            return closestUniqueId;
        }

        public Vector3 GetPos()
        {
            return this.transform.localPosition;
        }

        public Vector3 GetWorldPos()
        {
            return this.transform.position;
        }

        public virtual Transform Instantiate(Transform original, Vector3 position, Quaternion rotation)
        {
            return (Transform) GameObject.Instantiate(original, position, rotation);
        }

        public EntityType GetEntityType()
        {
            return EntityType;
        }

        public void AddChildSpriteBaseComponent(SpriteBaseComponent spriteBaseComponent)
        {
            if (!_childSpriteBaseComponents.Contains(spriteBaseComponent))
                _childSpriteBaseComponents.Add(spriteBaseComponent);
        }

        /// <summary>
        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
        /// may populate different parts of this object
        /// </summary>
        /// <param name="entityData"></param>
        public void Serialize(EntitySerializedData entityData)
        {
            if (this == null)
                throw new Exception(this.UniqueId + " has been destroyed but you are trying to serialize it");
            entityData.Name = name;
            entityData.RecreateOnGameLoad = base.GetRecreateOnLoadGame();
            entityData.EntityType = EntityType;
            entityData.EntityPrefabUniqueId = EntityPrefabUniqueId;
            entityData.TransformData = transform.Serialize();
            entityData.UniqueId = this.UniqueId;
            entityData.IsEnabled = gameObject.activeSelf;
            // Recursively record all repo Id's for the parent repo
            if (_parentRepo == null)
                SerializeUniqueIds(entityData);
            
            //var entitySerializedData = new EntitySerializedData();
            //SendMessageToAllButThis(0f, this.UniqueId, Telegrams.Serialize, entityData);
        }

        public void SerializeUniqueIds(EntitySerializedData entityData)
        {
            var name = GetName();
            entityData.UniqueIdMappings.Add(name, this.UniqueId);
            if (_childSpriteBaseComponents == null)
                return;
            foreach (var child in _childSpriteBaseComponents)
            {
                child.SerializeUniqueIds(entityData);
            }
        }

        public void DeserializeChildUniqueIds(EntitySerializedData entityData)
        {
            var name = GetName();
            var newUniqueId = entityData.UniqueIdMappings[name];
            SetUniqueId(newUniqueId, true);
            //SetUniqueId(newUniqueId, true);
            //entityData.UniqueIdMappings.Add(GetName(), this.UniqueId);
            if (_childSpriteBaseComponents == null)
                return;
            foreach (var child in _childSpriteBaseComponents)
            {
                child.DeserializeChildUniqueIds(entityData);
            }
        }

        public virtual void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        {

            //var name = GetName();
            //if (!entitySerializedData.UniqueIdMappings.ContainsKey(name))
            //    return;
            //var mapping = entitySerializedData.UniqueIdMappings[name];
            //SetUniqueId(mapping, true);
        }

        public override void Destroy()
        {
            if (_childSpriteBaseComponents != null)
            {
                foreach (var child in _childSpriteBaseComponents)
                {
                    child.Destroy();
                }
            }
            base.Destroy();

        }

        public void Deserialize(EntitySerializedData entityData)
        {
            EntityType = entityData.EntityType;            
            transform.Deserialize(entityData.TransformData);
            name = entityData.Name;
            gameObject.SetActive(entityData.IsEnabled);
            JustLoaded = true;
        }

        public override void SetUniqueId(string uniqueId, bool force = false)
        {
            // The Unique Id is changing, need to deregister and reregister this component
            if (_addToEntityContainer)
                EntityContainer._instance.RemoveEntity(this);
            base.SetUniqueId(uniqueId, force);
            if (_addToEntityContainer)
                EntityContainer._instance.AddEntity(this);
        }
    }
}
