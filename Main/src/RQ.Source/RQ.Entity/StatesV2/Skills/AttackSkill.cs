using RQ.Common;
using RQ.Model.Interfaces;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Entity.StatesV2.Skills
{
    public class AttackSkill : Skill
    {
        [SerializeField]
        private float _damage = 0;
        [SerializeField]
        [Tag]
        private string[] _targetTags = null;


        public override void ProcessSkill()
        {
            base.ProcessSkill();
            var collidersHit = CollidersHit();
            if (collidersHit == null)
                return;
            ProcessAttack(collidersHit);
        }

        private void ProcessAttack(RaycastHit[] itemsHit)
        {
            // Start with the closest collisions
            Array.Sort(itemsHit, PhysicsHelper.RaycastDistanceCompareDel);
            //itemsHit = itemsHit.OrderBy(i => i.distance);
            var entitiesHit = ObjectPool.Instance.PullFromPool<HashSet<string>>(ObjectPoolType.HashSetString);
            entitiesHit.Clear();
            //HashSet<string> entitiesHit = new HashSet<string>();

            foreach (var item in itemsHit)
            {
                if (item.collider != null && item.transform != null)
                {
                    if (!_collisionComponent.CheckCollisionBasedOnLayer(item.collider))
                        continue;
                    var otherCollisionComponent = item.collider.GetComponent<CollisionComponent>();
                    if (otherCollisionComponent == null)
                        continue;
                    var entity = otherCollisionComponent.GetComponentRepository();
                    //var entity = item.collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
                    if (entity != null)
                    {
                        // Make sure each entity gets the message only once
                        if (entitiesHit.Contains(entity.UniqueId))
                            continue;
                        //if (_targetTags.Contains(entity.GetTag()))
                        var containsTargetTag = Array.IndexOf(_targetTags, item.collider.tag) > -1;
                        //if (_targetTags.Contains(item.collider.tag))
                        if (containsTargetTag)
                        {
                            var collisionComponent = item.collider.GetComponent<ICollisionComponent>();
                            //item.point
                            Debug.LogWarning("Hit " + item.collider.name);
                            var skillUsed = GetSkill == null ? null : GetSkill.UniqueId;
                            _damageComponent.DamageExternalEntityBySkill(entity.UniqueId, _damage, _spriteBase.GetTag(), item.point, collisionComponent, skillUsed);
                            entitiesHit.Add(entity.UniqueId);
                            //Debug.Break();
                        }
                    }
                }
            }

            ObjectPool.Instance.ReleaseToPool(ObjectPoolType.HashSetString, entitiesHit);
        }
        
        /// <summary>
        /// Derived class expected to override this to implement their hit detection
        /// </summary>
        /// <returns></returns>
        public virtual RaycastHit[] CollidersHit()
        {
            return null;
        }
    }
}
