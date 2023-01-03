using RQ.Common;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Enums;
using RQ.Model.Serialization;
using RQ.Physics;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Entity.Common
{
    public class EntityController
    {
        public static readonly EntityController Instance = new EntityController(); 
        private Dictionary<string, Transform> _entityTransforms;
        public event Action<Transform> EntityKilled;

        private EntityController()
        {
        }

        //[Obsolete]
        //public void DestroyEntitiesNotSerialized(List<EntitySerializedData> entityDatas)
        //{
        //    //var entitiesThatDoNotExist = entityDatas.Where(i => !existingEntities.Contains(i.UniqueId));
        //    var existingEntities = EntityContainer._instance.EntityInstanceMap.Keys;
        //    var realEntities = entityDatas.Select(i => i.UniqueId);
        //    var entitiesThatDoNotExist = existingEntities.Where(i => !realEntities.Contains(i)).ToList();
        //    Debug.Log("Destroying " + entitiesThatDoNotExist.Count + " entities - not found in save file");
        //    for (int i = 0; i < entitiesThatDoNotExist.Count; i++)
        //    {
        //        var entity = EntityContainer._instance.GetEntity(entitiesThatDoNotExist[i]);
        //        EntityContainer._instance.RemoveEntity(entity);
        //        GameObject.Destroy((entity as SpriteBaseComponent).gameObject);
        //    }
        //}

        //[Obsolete]
        //public void CreateEntitiesThatDoNotExist(List<EntitySerializedData> entityDatas)
        //{
        //    var existingEntities = EntityContainer._instance.EntityInstanceMap.Keys;
        //    var entitiesThatDoNotExist = entityDatas.Where(i => !existingEntities.Contains(i.UniqueId)).ToList();
        //    var names = entitiesThatDoNotExist.Select(i => i.Name + ", ").ToArray();
        //    var namestring = string.Empty;
        //    if (names != null)
        //        namestring  = String.Concat(names);
        //    Debug.Log("Creating " + entitiesThatDoNotExist.Count + " entities that were in the save file but not in scene " + namestring);
        //    CreateEntities(entitiesThatDoNotExist);
        //}

        //public void CreateEntities(IEnumerable<EntitySerializedData> entityDatas, Transform parent)
        //{
        //    foreach (EntitySerializedData entityData in entityDatas)
        //    {
        //        try
        //        {
        //            GameDataController.Instance.LoadingEntity = entityData;
        //            var newEntity = EntityFactory.Create(entityData.EntityPrefabUniqueId);
        //            if (newEntity == null)
        //            {
        //                Log.Error("Cannot create " + entityData.Name + "(" + entityData.EntityPrefabUniqueId + ")");
        //                continue;
        //            }
        //            newEntity.transform.parent = parent;
        //            //newEntity.Deserialize(entityData.TransformData);

        //            var spriteBaseComponent = newEntity.GetComponent<SpriteBaseComponent>();
        //            if (spriteBaseComponent != null)
        //            {
        //                spriteBaseComponent.SetUniqueId(entityData.UniqueId);
        //                spriteBaseComponent.DeserializeChildUniqueIds(entityData);
        //                //spriteBaseComponent.name = entityData.Name;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.LogError(ex);
        //        }
        //    }
        //    GameDataController.Instance.LoadingEntity = null;
        //}

        public void CreateEntityTransforms(List<EntityPrefab> entityPrefabsList)
        {
            _entityTransforms = new Dictionary<string, Transform>();
            foreach (var entityTransform in entityPrefabsList)
            {
                _entityTransforms.Add(entityTransform.UniqueId, entityTransform.Prefab);
            }
        }

        public Transform GetEntityPrefab(string uniqueId)
        {
            Transform transform;
            _entityTransforms.TryGetValue(uniqueId, out transform);
            return transform;
        }

        public BaseObject ClosestEntityToPosition(Vector2D pos, EntityType[] type)
        {
            BaseObject closestEntity = null;
            float closestDistanceSq = float.MaxValue;

            foreach (var entity in EntityContainer._instance.EntityInstanceMap)
            {
                var baseGameEntity = entity.Value as SpriteBaseComponent;
                if (baseGameEntity == null)
                    continue;
                if (Array.IndexOf(type, baseGameEntity.GetEntityType()) > -1)
                //if (type.Contains(baseGameEntity.GetEntityType()))
                {
                    // Found a new closer entity?
                    var distanceSq = Vector2D.Vec2DDistanceSq(baseGameEntity.transform.localPosition, pos);
                    if (distanceSq < closestDistanceSq)
                    {
                        // This is the new closest entity
                        closestEntity = baseGameEntity;
                        closestDistanceSq = distanceSq;
                    }
                }
            }

            return closestEntity;
        }

        public BaseObject ClosestEntityToPosition(Vector2D pos, EntityType type)
        {
            return ClosestEntityToPosition(pos, new EntityType[] { type });
        }

        public BaseObject ClosestEntityToPosition(BaseObject entity, EntityType type)
        {
            return ClosestEntityToPosition(entity.transform.position, type);
        }

        public BaseObject ClosestEntityToPosition(BaseObject entity, EntityType[] type)
        {
            return ClosestEntityToPosition(entity.transform.position, type);
        }

        public Transform ClosestEntityToPosition(BaseObject entity, EntityTarget target)
        {
            Transform closestEntity;
            switch (target)
            {
                case EntityTarget.Enemy:
                    closestEntity = ClosestEntityToPosition(entity, new EntityType[] {EntityType.Enemy, EntityType.Boss}).transform;
                    break;
                case EntityTarget.Player:
                    closestEntity = EntityContainer._instance.GetMainCharacter().transform;
                    break;
                case EntityTarget.Companion:
                    closestEntity = EntityContainer._instance.GetCompanionCharacter().transform;
                    break;
                case EntityTarget.NPC:
                    closestEntity = ClosestEntityToPosition(entity, EntityType.NPC).transform;
                    break;
                case EntityTarget.Follow:
                    // There is only one follower, so they are automatically the closest
                    closestEntity = entity.GetComponent<AIComponent>().GetFollow();
                    break;
                case EntityTarget.Target:
                    // There is only one target, so they are automatically the closest
                    closestEntity = entity.GetComponent<AIComponent>().Target;
                    break;
                case EntityTarget.PlayerOrCompanion:
                    closestEntity = ClosestEntityToPosition(entity, new EntityType[] {
                        EntityType.Player, EntityType.Companion}).transform;
                    break;
                //case EntityTarget.Camera:
                //    closestEntity = 
                //    break;
                default:
                    closestEntity = null;
                    break;
            }
            return closestEntity;
        }

        // A pool doesn't really benefit us when we can just keep it here, where it lives forever anyway
        private List<string> tempEntities = new List<string>();
        public void Cleanup()
        {
            //var tempEntitiesToDelete = ObjectPool.Instance.PullFromPool<List<string>>(ObjectPoolType.StringList);
            //tempEntitiesToDelete.Clear();
            tempEntities.Clear();
            foreach (var entity in EntityContainer._instance.EntityInstanceMap)
            {
                if (entity.Value as UnityEngine.Object == null)
                    tempEntities.Add(entity.Key);
            }
            //var entitiesToDelete = EntityContainer._instance.EntityInstanceMap.Where(i => (i.Value as UnityEngine.Object == null)).ToList();
            //Debug.LogWarning("Cleaning up " + entitiesToDelete.Count() + " entities that got destroyed but are still in the Entity List");
            for (int i = 0; i < tempEntities.Count; i++)
            {
                Debug.LogError("Cleaning " + tempEntities[i]);
                EntityContainer._instance.EntityInstanceMap.Remove(tempEntities[i]);
            }
            //ObjectPool.Instance.ReleaseToPool(ObjectPoolType.StringList, tempEntitiesToDelete);
        }

        public void SendMessageToAllEntities(float delay, string senderId, Telegrams msg, object extraInfo, TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            foreach (var entityKvp in EntityContainer._instance.EntityInstanceMap)
            {
                var entity = entityKvp.Value as IComponentRepository;
                if (entity != null)
                    MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, senderId, entity.UniqueId, msg, extraInfo, null, earlyTermination);
            }
        }

        public IList<IEntity> GetByEntityType(EntityType entityType)
        {
            List<IEntity> sprites = ObjectPool.Instance.PullFromPool<List<IEntity>>(ObjectPoolType.IEntityList);
            sprites.Clear();
            //List<IEntity> sprites = new List<IEntity>();
            foreach (var component in EntityContainer._instance.EntityInstanceMap)
            {
                var sprite = component.Value as ISpriteBase;
                if (sprite != null)
                {
                    if (sprite.GetEntityType() == entityType)
                        sprites.Add(component.Value);
                }
            }
            return sprites;
        }

        public IEntity GetFirstEntityByType(EntityType entityType)
        {
            foreach (var component in EntityContainer._instance.EntityInstanceMap)
            {
                var sprite = component.Value as ISpriteBase;
                if (sprite != null && sprite.GetEntityType() == entityType)
                {
                    return component.Value;
                }
            }
            return null;
        }

        public EntitySerializedData[] Serialize()
        {
            List<EntitySerializedData> entityData = new List<EntitySerializedData>();

            var entityMap = EntityContainer._instance.EntityInstanceMap;

            if (entityMap != null)
            {
                foreach (var item in entityMap)
                {
                    try
                    {
                        EntitySerializedData data = new EntitySerializedData();
                        // Serialization needs to happen to all components, enabled or not
                        var serializeableRepository = item.Value as ISerializableObject;
                        if (serializeableRepository != null)
                            serializeableRepository.Serialize(data);

                        var entity = item.Value as IComponentRepository;
                        if (entity == null)
                            continue;
                        var allComponents = entity.Components.GetComponents();
                        foreach (var component in allComponents)
                        {
                            var serializableObject = component as ISerializableObject;
                            if (serializableObject == null)
                                continue;
                            serializableObject.Serialize(data);
                        }

                        if (data != null)
                            entityData.Add(data);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }

            return entityData.ToArray();
        }

        //public void DeserializeUniqueIds(IEnumerable<EntitySerializedData> entityDatas)
        //{
        //    Debug.Log("EntityController DeserializeUniqueIds called");

        //    // Now that the EntityMap is filled out, Deserialize can find the new ID's
        //    foreach (EntitySerializedData entityData in entityDatas)
        //    {
        //        if (!EntityContainer._instance.Contains(entityData.UniqueId))
        //        {
        //            Debug.LogError("Entity DeserializeUniqueIds - Could not locate id " + entityData.UniqueId + " for " + entityData.Name);
        //        }
        //        else
        //        {
        //            var entity = EntityContainer._instance.GetEntity<IComponentRepository>(entityData.UniqueId);
        //            if (entity == null) continue;
        //            var allComponents = entity.Components.GetComponents().ToList();

        //            //foreach (var component in allComponents)
        //            //{
        //            Debug.Log("Setting uniqueId on " + allComponents.Count + " components.");
        //            for (int i = 0; i < allComponents.Count; i++)
        //            {
        //                var component = allComponents[i];
        //                var serializableObject = component as ISerializableObject;
        //                if (serializableObject == null)
        //                    continue;
        //                serializableObject.DeserializeUniqueIds(entityData);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Deserialize all entities in a scene, typically called on Game Load
        /// Must be called after RecordInstanceID's and after loading the scene
        /// </summary>
        /// <param name="entityData"></param>
        //public void Deserialize(IEnumerable<EntitySerializedData> entityDatas)
        //{
        //    Debug.Log("EntityController Deserialize called");

        //    // Now that the EntityMap is filled out, Deserialize can find the new ID's
        //    foreach (EntitySerializedData entityData in entityDatas)
        //    {
        //        try
        //        {
        //            if (!EntityContainer._instance.Contains(entityData.UniqueId))
        //            {
        //                Debug.LogError("Entity Deserialize - Could not locate id " + entityData.UniqueId + " for " + entityData.Name);
        //            }
        //            else
        //            {
        //                var entity = EntityContainer._instance.GetEntity(entityData.UniqueId);
        //                var serializeableRepository = entity as ISerializableObject;
        //                if (serializeableRepository != null)
        //                {
        //                    //if (serializeableRepository.gameObject.Equals(null))
        //                    //{

        //                    //}
        //                    serializeableRepository.Deserialize(entityData);
        //                }

        //                var repo = entity as IComponentRepository;
        //                if (repo == null)
        //                    continue;
        //                var allComponents = repo.Components.GetComponents().ToList();

        //                //foreach (var component in allComponents)
        //                //{
        //                for (int i = 0; i < allComponents.Count; i++)
        //                {
        //                    try
        //                    {
        //                        var component = allComponents[i];
        //                        var serializableObject = component as ISerializableObject;
        //                        if (serializableObject == null)
        //                            continue;
        //                        serializableObject.Deserialize(entityData);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Debug.LogErrorFormat("Error deserializing {0}({1}) in {2} {3}", allComponents[i].name, allComponents[i].GetName(), entityData.Name, ex.StackTrace);
        //                        Debug.LogError(ex);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.LogError("Entity " + entityData.Name + " was destroyed prior to deserialization");
        //            Debug.LogError(ex);
        //            //continue;
        //        }
        //    }
        //    Debug.Log("EntityController Deserialize completed");
        //}

        public void DestroyAllEntities()
        {
            var tempStringList = ObjectPool.Instance.PullFromPool<List<string>>(ObjectPoolType.StringList);
            tempStringList.Clear();
            foreach (var entity in EntityContainer._instance.EntityInstanceMap)
            {
                tempStringList.Add(entity.Value.UniqueId);
            }
            //var entities = EntityContainer._instance.EntityInstanceMap.Values.ToList();
            Debug.Log("Game Objects to destroy: " + tempStringList.Count);
            // We go in reverse so removed items don't intefere with the index
            for (int i = tempStringList.Count - 1; i >= 0; i--)
            {
                // TODO Lulu is not active, and is thus NOT IN THE MESSAGING SYSTEM!!!!
                //GameObject.DestroyImmediate(entities[i].transform.gameObject);
                //MessageDispatcher.Instance.DispatchMsg(0f, "Entity Controller",
                //    entities[i].UniqueId, Enums.Telegrams.Kill, null);
                try
                {
                    MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, null, tempStringList[i], null);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            ObjectPool.Instance.ReleaseToPool(ObjectPoolType.StringList, tempEntities);
            EntityContainer._instance.ResetEntityList();
        }

        //public void DestroyReceatedEntities()
        //{
        //    var tempEntityList = ObjectPool.Instance.PullFromPool<List<IEntity>>(ObjectPoolType.IEntityList);
        //    tempEntityList.Clear();
        //    foreach (var entity in EntityContainer._instance.EntityInstanceMap)
        //    {
        //        tempEntityList.Add(entity.Value);
        //    }
        //    //var entities = EntityContainer._instance.EntityInstanceMap.Values.ToList();
        //    var recreateList = entities.Where(i => !i.GetRecreateOnLoadGame());
        //    var count = recreateList == null ? 0 : recreateList.Count();
        //    Debug.Log("Game Objects to destroy: " + count);
        //    // We go in reverse so removed items don't intefere with the index
        //    for (int i = entities.Count - 1; i >= 0; i--)
        //    {
        //        if (!entities[i].GetRecreateOnLoadGame())
        //            continue;
        //        // TODO Lulu is not active, and is thus NOT IN THE MESSAGING SYSTEM!!!!
        //        //GameObject.DestroyImmediate(entities[i].transform.gameObject);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, "Entity Controller",
        //        //    entities[i].UniqueId, Enums.Telegrams.Kill, null);
        //        try
        //        {
        //            MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, null, entities[i].UniqueId, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.LogError(ex);
        //        }
        //    }

        //    //EntityContainer._instance.ResetEntityList();
        //}
    }
}
