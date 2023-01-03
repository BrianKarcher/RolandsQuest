using RQ.Common;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model.Messaging;
using RQ.Physics.Collision;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV3.Conditions
{
    //[AddComponentMenu("RQ/States/Conditions/Collision")]
    public class CollisionConditionConfig : StateTransitionConditionBaseConfig
    {
        //protected PhysicsComponent _physicsComponent;
        //[SerializeField]
        //private CollisionComponent _collisionComponent;
        [SerializeField]
        private bool _isTrigger = false;
        [SerializeField]
        [Tag]
        private string[] _targetTags = null;
        [SerializeField]
        [Tag]
        private string[] _anythingBut;
        [SerializeField]
        private bool _onlyHitMainCollider = false;

        private Action<Telegram2> _triggerEnterDelegate;
        private Action<Telegram2> _collisionEnterDelegate;
        private Dictionary<string, long> _triggerEnterIds = new Dictionary<string, long>();
        private Dictionary<string, long> _collisionEnterIds = new Dictionary<string, long>();
        //private bool _collided = false;
        //private EntityTarget Target = EntityTarget.Enemy;

        //[SerializeField]
        //private float _valueSquared = 0f;
        //[SerializeField]
        //private float _value2Squared = 0f;

        //public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        //{
        //    base.SetEntity(entity, stateMachineId, stateInfo);
        //    _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
        //    if (_collisionComponent == null)
        //        _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
        //}

        public override bool TestCondition(IStateMachine stateMachine)
        {
            //var collision = _damageComponent.GetCollisionInfo();
            //if (collision == null)
            //    return false;
            //return collision.NearbyEntities.Any();
            //return _collided;
            return GetIsConditionSatisfied(stateMachine);
        }

        public override void ConditionInit(IStateMachine stateMachine)
        {
            base.ConditionInit(stateMachine);
            var entity = stateMachine.GetComponentRepository();
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var collisionComponent = entity.Components.GetComponent<CollisionComponent>();

            if (_triggerEnterDelegate == null)
            {
                _triggerEnterDelegate = (data) =>
                {
                    if (data.ExtraInfo != null)
                    {
                        var collider2D = data.ExtraInfo as CollisionMessageData;
                        // Is it the collider we are tracking?
                        if (collider2D.CollisionComponentUniqueId != collisionComponent.UniqueId)
                            return;
                        ProcessCollision(stateMachine, collisionComponent, collider2D.OtherCollider, true);
                    }
                };
            }
            if (_collisionEnterDelegate == null)
            {
                _collisionEnterDelegate = (data) =>
                {
                    if (data.ExtraInfo != null)
                    {
                        var collision2D = data.ExtraInfo as CollisionMessageData;
                        // Is it the collider we are tracking?
                        if (collision2D.CollisionComponentUniqueId != collisionComponent.UniqueId)
                            return;
                        ProcessCollision(stateMachine, collisionComponent, collision2D.CollisionCollider.collider, false);
                    }
                };
            }

            var id = MessageDispatcher2.Instance.StartListening("TriggerEnter", entity.UniqueId, _triggerEnterDelegate);
            _triggerEnterIds[entity.UniqueId] = id;
            //entity.StartListening("TriggerEnter", this.UniqueId, _triggerEnterDelegate);
            id = MessageDispatcher2.Instance.StartListening("CollisionEnter", entity.UniqueId, _collisionEnterDelegate);
            _collisionEnterIds[entity.UniqueId] = id;
            //entity.StartListening("CollisionEnter", this.UniqueId, _collisionEnterDelegate);
        }

        public override void ConditionExit(IStateMachine stateMachine)
        {
            base.ConditionExit(stateMachine);
            var entity = stateMachine.GetEntity().GetComponent<IComponentRepository>();
            if (entity == null)
                throw new Exception("Collision Condition - Entity is empty");
            MessageDispatcher2.Instance.StopListening("TriggerEnter", entity.UniqueId, _triggerEnterIds[entity.UniqueId]);
            MessageDispatcher2.Instance.StopListening("CollisionEnter", entity.UniqueId, _collisionEnterIds[entity.UniqueId]);
            //entity.StopListening("TriggerEnter", this.UniqueId);
            //entity.StopListening("CollisionEnter", this.UniqueId);
            _triggerEnterIds.Remove(entity.UniqueId);
            _collisionEnterIds.Remove(entity.UniqueId);
        }

        private bool ProcessCollision(IStateMachine stateMachine, CollisionComponent collisionComponent, 
            Collider collider, bool isTrigger)
        {
            //if (_isTrigger != isTrigger)
            //    return false;

            if (collider == null || collider.attachedRigidbody == null)
                return false;

            //var targetEntity = EntityUIBase.GetEntity<IBaseGameEntity>(
            //    collider.attachedRigidbody.transform);
            var targetEntity = collider.attachedRigidbody.transform.GetComponent<IComponentRepository>();
            if (targetEntity == null)
                return false;
            var targetEntityPhysicsComponent = targetEntity.Components.GetComponent<PhysicsComponent>();
            var targetEntityFloorComponent = targetEntity.Components.GetComponent<FloorComponent>();
            // TODO: Do something better than name
            //if (_onlyHitMainCollider && targetEntityCollisionComponent.GetCollisionData()._mainCollider.name != collider.name)
            //    return false;
            var targetEntityDamageComponent = targetEntity.GetComponent<DamageComponent>();

            //var entity = GetEntity() as IBaseGameEntity;
            var collisionData = collisionComponent.GetCollisionData();
            var floorComponent = collisionComponent.GetComponentRepository().Components.GetComponent<FloorComponent>();

            // Level check
            //if (!PhysicsCollision.LevelMatch(collisionData.Level, floorComponent, targetEntityFloorComponent))
            //    return false;

            // Tag check between this entity and other tag
            var containsCollisionTag = Array.IndexOf(collisionData.Tags, collider.tag) > -1;
            //if (!collisionData.Tags.Contains(collider.tag))
            if (!containsCollisionTag)
                return false;

            //if (_anythingBut.Any() && _anythingBut.Contains(collider.tag))
            var containsAnythingButTag = Array.IndexOf(_anythingBut, collider.tag) > -1;
            if (_anythingBut.Length != 0 && containsAnythingButTag)
                return false;

            var entity = collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
            if (entity != null)
            {
                var containsTargetTag = Array.IndexOf(_targetTags, entity.GetTag()) > -1;
                //if (!_targetTags.Contains(entity.GetTag()))
                if (!containsTargetTag)
                {
                    return false;
                }
            }

            SetIsConditionSatisfied(stateMachine, true);

            //_stateInfo.IsConditionSatisfied

            // Tag check between this tag and other entity
            //var targetCollisionData = targetEntityDamageComponent.GetCollisionInfo();
            //if (!targetCollisionData.Tags.Contains(_physicsComponent.GetTag()))
            //    return false;

            // All passed? Set _collided
            //var id = targetEntity.UniqueId;
            //var id = _spriteBaseId;
            //if (!collisionData.NearbyEntities.Contains(id))
            //    collisionData.NearbyEntities.Add(id);

            //bool causeDamage = false;
            //if (collisionData.DamageTargetOnCollision && !isTrigger)
            //    causeDamage = true;

            //if (collisionData.DamageTargetOnTrigger && isTrigger)
            //    causeDamage = true;

            //if (causeDamage)
            //    _damageComponent.DamageEntity(id, collisionData.Damage, this.tag);

            //_collided = true;

            //if ()
            //{
            //    return true;
            //}
            //sprite.ProcessMovementInput(input);
            //agent.ProcessButtonInput(input);
            //var sprite = agent as ISprite;
            //sprite.IsAnimationComplete = true;
            return true;
        }

        //public override void Reset()
        //{
        //    base.Reset();
        //    //_collided = false;
        //}
    }
}
