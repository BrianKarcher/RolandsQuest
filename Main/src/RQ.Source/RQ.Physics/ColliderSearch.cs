using System;
using RQ.Common.Container;
using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Physics.Components;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics
{
    public class ColliderSearch
    {
        //public RaycastHit2D[] BoxCastAll(IComponentRepository entity, Vector2 pos, Vector2 size, float angle, float distance)
        //{
        //    var animationComponent = entity.Components.GetComponent<IAnimationComponent>();
        //    var facingDirection = animationComponent.GetFacingDirectionVector();
        //    //var pos = entity.transform.position;

        //    //GameDataController.Instance.GetLayerMask(LevelLayer.LevelOne);
        //    //var itemsHit = Physics2D.BoxCastAll(pos + _offset, _size, _angle, facingDirection, _distance, layerMask/*, GetEntityUI().GetTransform().gameObject.layer*/);
        //    var itemsHit = Physics2D.BoxCastAll(pos, size, angle, facingDirection, distance);
        //    return itemsHit;
        //}

        //public int CalculateLayerMask(IComponentRepository entity, bool sameLayer, int layerIndex)
        //{
        //    if (sameLayer)
        //    {
        //        var collisionComponent = entity.Components.GetComponent<CollisionComponent>();
        //        layerIndex = collisionComponent.transform.gameObject.layer;
        //        //layerMask = ((LayerMask)).value;
        //    }
        //    var layerMask = 1 << layerIndex;
        //    return layerMask;
        //}

        private HashSet<string> _tempEntitiesHitHashSet = new HashSet<string>();

        public void ColliderScrub(CollisionComponent collisionComponent, RaycastHit[] raycasts, string[] _targetTags,
            List<ColliderSearchData> colliderSearchDatas)
        {
            colliderSearchDatas.Clear();
            Array.Sort(raycasts, PhysicsHelper.RaycastDistanceCompareDel);
            // Start with the closest collisions
            //var itemsHit = raycasts.OrderBy(i => i.distance);
            //var entitiesHit = new HashSet<string>();
            _tempEntitiesHitHashSet.Clear();
            //var colliderSearchDatas = new List<ColliderSearchData>();

            foreach (var item in raycasts)
            {
                if (item.collider != null && item.collider.attachedRigidbody != null)
                {
                    if (!collisionComponent.CheckCollisionBasedOnLayer(item.collider))
                        continue;
                    var entity = item.collider.attachedRigidbody.transform.GetComponent<ComponentRepository>();
                    if (entity != null)
                    {
                        // Make sure each entity gets the message only once
                        if (_tempEntitiesHitHashSet.Contains(entity.UniqueId))
                            continue;
                        //if (_targetTags.Contains(entity.GetTag()))
                        bool tagFound = Array.IndexOf(_targetTags, item.collider.tag) > -1;
                        if (tagFound)
                        {
                            var otherCollisionComponent = item.collider.GetComponent<ICollisionComponent>();
                            //_damageComponent.DamageExternalEntity(uniqueId, _damage, _spriteBase.GetTag(), item.point, otherCollisionComponent, skillUsed);
                            //item.point
                            Debug.LogWarning("Hit " + item.collider.name);
                            var colliderSearchData = new ColliderSearchData()
                            {
                                EntityUniqueId = entity.UniqueId,
                                Point = item.point,
                                CollisionComponent = otherCollisionComponent
                            };
                            colliderSearchDatas.Add(colliderSearchData);
                            _tempEntitiesHitHashSet.Add(entity.UniqueId);
                            //Debug.Break();
                        }
                    }
                }
            }
        }
    }
}
