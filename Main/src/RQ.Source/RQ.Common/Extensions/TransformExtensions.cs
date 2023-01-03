using RQ.Common.Components;
using RQ.Entity.Components;
using RQ.Model.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Using lists in this fashion makes them reusable - reduces memory allocations
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="objects"></param>
        public static void GetComponentsInChildrenOneDeep<T>(this Transform transform, List<T> objects) where T : class
        {
            objects.Clear();
            foreach (Transform trans in transform.transform)
            {
                var obj = trans.GetComponent<T>();
                if (obj != null)
                    objects.Add(obj);
            }

            //return objects;
            //return (obj ?? string.Empty).ToString();
        }

        public static void GetPositionsInChildrenOneDeep(this Transform transform, List<Vector3> objects)
        {
            objects.Clear();
            foreach (Transform trans in transform.transform)
            {
                var obj = trans.GetComponent<Transform>();
                if (obj != null)
                {
                    objects.Add(obj.transform.position);
                }
                    
            }

            //return objects;
            //return (obj ?? string.Empty).ToString();
        }

        public static string GetRepoId(this Transform transform)
        {
            if (transform == null)
                return null;
            var spriteBase = transform.GetComponent<ISpriteBase>();
            if (spriteBase == null)
                return null;
            return spriteBase.UniqueId;
        }

        public static string GetRepoId(this Location source)
        {
            if (source == null)
                return null;
            return source.transform.GetRepoId();
        }

        public static TransformData Serialize(this Transform transform)
        {
            if (transform == null)
                return null;
            var transformData = new TransformData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                Scale = transform.localScale
            };
            return transformData;
        }

        public static void Deserialize(this Transform transform, TransformData transformData)
        {
            if (transform == null)
                return;
            transform.position = transformData.Position;
            transform.rotation = Quaternion.Euler(transformData.Rotation);
            transform.localScale = transformData.Scale;
        }
    }
}
