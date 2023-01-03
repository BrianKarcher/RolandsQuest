using RQ.Animation;
using RQ.Common.Components;
using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Interfaces;
using RQ.Model.Item;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using Assets.Source;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Entity.Components
{
    [AddComponentMenu("RQ/Components/Attack Component")]
    public class AttackComponent : ComponentPersistence<AttackComponent>
    {
        [SerializeField]
        public ItemConfig _skill;
        [SerializeField]
        public string _messageToSend;
        public AttackData AttackData;
        private long _processAttackId;
        private PhysicsComponent _physicsComponent;
        private DamageComponent _damageComponent;
        private AnimationComponent _animationComponent;
        private CollisionComponent _collisionComponent;
        public event Action Attacked;

        private DateTime? _lastAttackTime;
        private float _hitDistance;
        private float _maxDistance;
        private Vector3 _testPosition;
        private Vector3 _testDirection;
        private Vector3 _size;
        private bool _hitTarget = false;

        public override void Start()
        {
            base.Start();
            _physicsComponent = GetComponentRepository().Components.GetComponent<PhysicsComponent>();
            _damageComponent = GetComponentRepository().Components.GetComponent<DamageComponent>();
            var animations = GetComponentRepository().Components.GetComponents<AnimationComponent>();
            for (int i = 0; i < animations.Count; i++)
            {
                var animationComponent = animations[i] as AnimationComponent;
                if (animationComponent.IsMain())
                {
                    _animationComponent = animationComponent;
                    break;
                }
            }
            //_animationComponent = GetComponentRepository().Components.GetComponents<AnimationComponent>().FirstOrDefault(i => i.IsMain());
            _collisionComponent = GetComponentRepository().Components.GetComponent<CollisionComponent>();

            //if (_skill != null)
            //    MessageDispatcher2.Instance.DispatchMsg("SkillUsed", 0f, entity.UniqueId, entity.UniqueId, _skill);
            //ProcessAttackDelayed();
        }

        public override void StartListening()
        {
            base.StartListening();
            _processAttackId = MessageDispatcher2.Instance.StartListening("ProcessAttack", GetComponentRepository().UniqueId, (data) =>
            {
                string attackComponentName = data.ExtraInfo.ToString();
                if (this.GetName()  == attackComponentName)
                {
                    Debug.Log("Processing Attack");
                    ProcessAttackNow();
                }
                //_isRunning = false;
            });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("ProcessAttack", GetComponentRepository().UniqueId, _processAttackId);
        }

        public void ProcessAttackDelayed()
        {
            MessageDispatcher2.Instance.DispatchMsg("ProcessAttack", AttackData.StrikeDelay, GetComponentRepository().UniqueId, GetComponentRepository().UniqueId, this.GetName());
        }

        public virtual void ProcessAttackNow()
        {
            Attacked?.Invoke();
            // TODO THIS IS TEMPORARY, FIX ASAP

            if (_damageComponent == null)
                throw new Exception("Damage component is required for the Attack state in entity " + GetComponentRepository().GetName());
            //base.ProcessAttack();
            if (AttackData.StopMovingDuringAttack)
            {
                //var theSprite = _entity as ISprite;
                if (_physicsComponent != null)
                {
                    _physicsComponent.Stop();
                    //MessageDispatcher.Instance.DispatchMsg(0f, entity.UniqueId, _physicsComponent.UniqueId,
                    //    Enums.Telegrams.StopMovement, null);
                }
                //_physicsComponent.Stop();
            }

            MessageDispatcher2.Instance.DispatchMsg("AttackPerformed", 0f, GetComponentRepository().UniqueId, _damageComponent.UniqueId, null);
            var facingDirectionVector = _animationComponent.GetFacingDirectionVector();
            var pos = GetComponentRepository().transform.position;
            int layerMask = 0;
            if (AttackData.SameLayer)
            {
                var environmentLayerMask = LayerMask.NameToLayer("Environment");
                //var layerIndex = _collisionComponent.transform.gameObject.layer;
                //layerMask = 1 << layerIndex;
                layerMask = environmentLayerMask;
                //layerMask = ((LayerMask)).value;
            }
            else
            {
                foreach (var masks in AttackData.Layers)
                {
                    layerMask |= masks.Mask;
                }
                //layerMask = AttackData.Layer.Mask;
            }
            var heading = _physicsComponent.GetPhysicsData().Heading;
            //GameDataController.Instance.GetLayerMask(LevelLayer.LevelOne);
            //var itemsHit = Physics2D.BoxCastAll(pos + _offset, _size, _angle, facingDirection, _distance, layerMask/*, GetEntityUI().GetTransform().gameObject.layer*/);
            //IEnumerable<RaycastHit2D> itemsHit = Physics2D.BoxCastAll((Vector2)pos + AttackData.Offset, AttackData.Size, AttackData.Angle, facingDirection, AttackData.Distance);
            var attackSize = (Vector3)AttackData.Size;
            var facingDirection = _animationComponent.GetFacingDirection();
            //if (facingDirection == Direction.Left || facingDirection == Direction.Right)
            //    attackSize.x = 0.1f;
            //else
            //    attackSize.y = 0.1f;
            attackSize.z = 0.3f;

            var attackPos = pos + (Vector3)AttackData.Offset;
            //Debug.LogError($"Attacker Pos {attackPos}");

            _maxDistance = AttackData.Distance;
            _testDirection = facingDirectionVector;
            _testPosition = attackPos;
            _size = attackSize / 2;
            _lastAttackTime = DateTime.Now;

            _hitTarget = false;
            RaycastHit[] itemHits = UnityEngine.Physics.BoxCastAll(attackPos, attackSize / 2, facingDirectionVector, Quaternion.identity, AttackData.Distance, layerMask);
            //return;
            Array.Sort(itemHits, PhysicsHelper.RaycastDistanceCompareDel);
            //itemHits = itemHits.OrderBy(i => i.distance);
            
            //HashSet<string> entitiesHit = new HashSet<string>();
            var entitiesHit = ObjectPool.Instance.PullFromPool<HashSet<string>>(ObjectPoolType.HashSetString);
            entitiesHit.Clear();
            foreach (var itemHit in itemHits)
            {
                _hitTarget = true;
                _hitDistance = itemHit.distance;

                // This assumes the Rigidbody is on the same game object as the SpriteBaseComponent. Cannot assume!
                //var entity = itemHit.collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
                IComponentRepository otherEntity = null;
                var otherCollisionComponent = itemHit.collider?.GetComponent<IComponentBase>();
                if (otherCollisionComponent != null)
                {
                    otherEntity = otherCollisionComponent.GetComponentRepository();
                }

                if (otherEntity != null)
                {
                    //            // Make sure each entity gets the message only once
                    if (entitiesHit.Contains(otherEntity.UniqueId))
                        continue;
                    //if (_targetTags.Contains(entity.GetTag()))
                    var tagFound = Array.IndexOf(AttackData.TargetTags, itemHit.collider.tag) > -1;

                    //if (AttackData.TargetTags.Contains(itemHit.collider.tag))
                    if (tagFound)
                    {
                        //var collisionComponent = itemHit.collider.GetComponent<ICollisionComponent>();
                        //item.point
                        //Debug.LogWarning("Hit " + itemHit.collider.name);
                        //Debug.LogError($"Hit - Enemy position {otherEntity.transform.position}");
                        //Debug.LogError($"Hit - Enemy Collider position {itemHit.collider.transform.position}");
                        var skillUsed = _skill == null ? null : _skill.UniqueId;
                        _damageComponent.DamageExternalEntity(otherEntity.UniqueId, AttackData.Damage, GetComponentRepository().GetTag(), itemHit.point, otherCollisionComponent as ICollisionComponent, skillUsed);
                        MessageDispatcher2.Instance.DispatchMsg(_messageToSend, 0f, _componentRepository.UniqueId, otherEntity.UniqueId, null);
                        entitiesHit.Add(otherEntity.UniqueId);
                    }
                }
            }

            ObjectPool.Instance.ReleaseToPool(ObjectPoolType.HashSetString, entitiesHit);

            //if (!UnityEngine.Physics.BoxCast(attackPos, attackSize / 2, facingDirectionVector, out var itemHit, Quaternion.identity, AttackData.Distance, layerMask))
            //    return;

            // This assumes the Rigidbody is on the same game object as the SpriteBaseComponent. Cannot assume!
            //var entity = itemHit.collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
            //if (entity != null)
            //{
            //    //if (_targetTags.Contains(entity.GetTag()))
            //    if (AttackData.TargetTags.Contains(itemHit.collider.tag))
            //    {
            //        var collisionComponent = itemHit.collider.GetComponent<ICollisionComponent>();
            //        //item.point
            //        Debug.LogWarning("Hit " + itemHit.collider.name);
            //        Debug.LogError($"Hit - Enemy position {entity.transform.position}");
            //        Debug.LogError($"Hit - Enemy Collider position {itemHit.collider.transform.position}");
            //        var skillUsed = _skill == null ? null : _skill.UniqueId;
            //        _damageComponent.DamageExternalEntity(entity.UniqueId, AttackData.Damage, GetComponentRepository().GetTag(), itemHit.point, collisionComponent, skillUsed);
            //        MessageDispatcher2.Instance.DispatchMsg(_messageToSend, 0f, _componentRepository.UniqueId, entity.UniqueId, null);
            //    }
            //}

            //IEnumerable<RaycastHit> itemsHit = UnityEngine.Physics.BoxCastAll((Vector2)pos + AttackData.Offset, new Vector3(AttackData.Size.x, AttackData.Size.y, .01f), facingDirection, Quaternion.identity, AttackData.Distance);

            //if (itemsHit == null)
            //    return;

            //foreach (var rayCast in itemsHit)
            //{
            //    Debug.DrawLine((Vector2)pos + AttackData.Offset, rayCast.point, Color.blue, 3f);
            //}
            //// Start with the closest collisions
            //itemsHit = itemsHit.OrderBy(i => i.distance);
            //HashSet<string> entitiesHit = new HashSet<string>();

            //foreach (var item in itemsHit)
            //{
            //    if (item.collider != null && item.collider.attachedRigidbody != null)
            //    {
            //        if (!_collisionComponent.CheckCollisionBasedOnLayer(item.collider))
            //            continue;
            //        var entity = item.collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
            //        if (entity != null)
            //        {
            //            // Make sure each entity gets the message only once
            //            if (entitiesHit.Contains(entity.UniqueId))
            //                continue;
            //            //if (_targetTags.Contains(entity.GetTag()))
            //            if (AttackData.TargetTags.Contains(item.collider.tag))
            //            {
            //                var collisionComponent = item.collider.GetComponent<ICollisionComponent>();
            //                //item.point
            //                Debug.LogWarning("Hit " + item.collider.name);
            //                var skillUsed = _skill == null ? null : _skill.UniqueId;
            //                _damageComponent.DamageExternalEntity(entity.UniqueId, AttackData.Damage, GetComponentRepository().GetTag(), item.point, collisionComponent, skillUsed);
            //                entitiesHit.Add(entity.UniqueId);
            //                //Debug.Break();
            //            }
            //        }
            //    }
            //}
        }

        //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
        private void OnDrawGizmos()
        {
            if (_lastAttackTime == null)
                return;

            //bool timeout = DateTime.Now.Subtract(_lastAttackTime.Value) > TimeSpan.FromSeconds(5f);
            //if (timeout)
            //    return;

            //Check if there has been a hit yet
            if (_hitTarget)
            {
                Gizmos.color = Color.red;
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(_testPosition, _testDirection * _hitDistance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(_testPosition + _testDirection * _hitDistance, _size);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                Gizmos.color = Color.white;
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(_testPosition, _testDirection * _maxDistance);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(_testPosition + _testDirection * _maxDistance, _size);
            }
        }
    }
}
