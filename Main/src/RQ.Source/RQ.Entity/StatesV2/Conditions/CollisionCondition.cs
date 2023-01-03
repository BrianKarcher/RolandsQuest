using System;
using RQ.Common;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.FSM.V2.Conditionals;
using RQ.Messaging;
using RQ.Model.Messaging;
using RQ.Physics.Collision;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Conditions
{
    [AddComponentMenu("RQ/States/Conditions/Collision")]
    public class CollisionCondition : StateTransitionConditionBase
    {
        protected PhysicsComponent _physicsComponent;
        [SerializeField]
        private CollisionComponent _collisionComponent;
        private FloorComponent _floorComponent;
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
        //private bool _collided = false;
        //private EntityTarget Target = EntityTarget.Enemy;

        //[SerializeField]
        //private float _valueSquared = 0f;
        //[SerializeField]
        //private float _value2Squared = 0f;

        public override void SetEntity(IComponentRepository entity, string stateMachineId, StateInfo stateInfo)
        {
            base.SetEntity(entity, stateMachineId, stateInfo);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            if (_collisionComponent == null)
                _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _floorComponent = entity.Components.GetComponent<FloorComponent>();
        }

        public override bool TestCondition(IStateMachine stateMachine)
        {
            //var collision = _damageComponent.GetCollisionInfo();
            //if (collision == null)
            //    return false;
            //return collision.NearbyEntities.Any();
            //return _collided;
            return GetIsConditionSatisfied();
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            //var data = agent as ISprite;
            switch (telegram.Msg)
            {
                case Enums.Telegrams.TriggerEnter:
                    if (telegram.ExtraInfo != null)
                    {
                        var collider2D = telegram.ExtraInfo as CollisionMessageData;
                        // Is it the collider we are tracking?
                        if (collider2D.CollisionComponentUniqueId != _collisionComponent.UniqueId)
                            return false;
                        return ProcessCollision(collider2D.OtherCollider, true);
                    }
                    break;
                case Enums.Telegrams.CollisionEnter:
                    if (telegram.ExtraInfo != null)
                    {
                        var collision2D = telegram.ExtraInfo as CollisionMessageData;
                        // Is it the collider we are tracking?
                        if (collision2D.CollisionComponentUniqueId != _collisionComponent.UniqueId)
                            return false;
                        return ProcessCollision(collision2D.CollisionCollider.collider, false);
                    }
                    // TODO FIX THIS - The state transition table should be determing the next state
                    //data.GetFSM().RevertToPreviousState();
                    break;
            }
            return false;
        }

        private bool ProcessCollision(Collider collider, bool isTrigger)
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
            var collisionData = _collisionComponent.GetCollisionData();

            // Level check
            //if (!PhysicsCollision.LevelMatch(collisionData.Level, _floorComponent, targetEntityFloorComponent))
            //    return false;

            // Tag check between this entity and other tag
            var tagInCollisionDataTags = Array.IndexOf(collisionData.Tags, collider.tag) > -1;
            //if (!collisionData.Tags.Contains(collider.tag))
            if (!tagInCollisionDataTags)
                return false;

            var tagInAnythingBut = Array.IndexOf(_anythingBut, collider.tag) > -1;
            //if (_anythingBut.Any() && _anythingBut.Contains(collider.tag))
            if (_anythingBut.Length != 0 && tagInAnythingBut)
            return false;

            var entity = collider.attachedRigidbody.transform.GetComponent<SpriteBaseComponent>();
            if (entity != null)
            {
                var tagInTargetTags = Array.IndexOf(_targetTags, entity.GetTag()) > -1;
                if (!tagInTargetTags)
                {
                    return false;
                }
            }

            SetIsConditionSatisfied(true);
            return true;
        }
    }
}
