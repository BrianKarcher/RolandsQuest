using RQ.Common;
using RQ.Entity.UI;
using RQ.Enums;
using UnityEngine;

namespace RQ.Entity.Common
{
    public static class EntityFactory
    {
        public static Transform Create(string entityPrefabUniqueId)
        {
            return Create(entityPrefabUniqueId, Vector3.zero, Quaternion.identity);
        }
        public static Transform Create(string entityPrefabUniqueId, Vector3 position)
        {
            return Create(entityPrefabUniqueId, position, Quaternion.identity);
        }
        public static Transform Create(string entityPrefabUniqueId, Vector3 position, Quaternion rotation)
        {
            var entity = EntityController.Instance.GetEntityPrefab(entityPrefabUniqueId);
            if (entity == null)
                return null;

            return Instantiate(entity, position, rotation);
        }

        private static Transform Instantiate(Transform newObject, Vector3 position, Quaternion rotation)
        {
            var entity = (Transform)GameObject.Instantiate(newObject, position, rotation);
            return entity;
        }
    }
}
