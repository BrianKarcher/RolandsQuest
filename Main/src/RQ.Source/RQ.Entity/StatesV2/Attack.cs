using RQ.Common;
using RQ.Messaging;
using RQ.Model.Item;
using RQ.Model.Physics;
using RQ.Physics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Attack")]
    public class Attack : AnimatorState
    {
        [SerializeField]
        private float _strikeDelay = 0f;
        [SerializeField]
        private bool _stopMovingDuringAttack = true;
        [SerializeField]
        private Vector2D _offset = Vector2D.Zero();
        [SerializeField]
        private Vector2D _size = Vector2D.Zero();
        [SerializeField]
        private float _angle = 0;
        [SerializeField]
        private float _distance = 0;
        [SerializeField]
        private float _damage = 0;
        [SerializeField]
        [Tag]
        private string[] _targetTags = null;
        [SerializeField]
        private bool _sameLayer = true;
        [SerializeField]
        private SingleUnityLayer _layer;
        [SerializeField]
        private ItemConfig _skill;

        private List<ColliderSearchData> tempScrubbedColliders;

        public override void Enter()
        {
            base.Enter();
            if (_skill != null)
                MessageDispatcher2.Instance.DispatchMsg("SkillUsed", 0f, this.UniqueId, _componentRepository.UniqueId, _skill);
            if (_stopMovingDuringAttack)
            {
                //var theSprite = _entity as ISprite;
                if (_physicsComponent != null)
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                        Enums.Telegrams.StopMovement, null);
                    //_physicsComponent.Stop();
            }
            SendMessageToSelf(_strikeDelay, Enums.Telegrams.ProcessAttack, 
                null, Enums.TelegramEarlyTermination.ChangeScenes);
            //SendLocalMessageToSelf(_strikeDelay, Enums.Telegrams.ProcessAttack, null, Enums.TelegramEarlyTermination.ChangeScenes);
            //MessageDispatcher.Instance.DispatchMsg(_strikeDelay, _entity.MessageHandlerID(), _entity.MessageHandlerID(),
            //    Enums.Telegrams.ProcessAttack, null, Enums.TelegramEarlyTermination.ChangeScenes);
        }

        public override void Exit()
        {
            base.Exit();
            //MessageDispatcher.Instance.RemoveFromQueue(i => i.ReceiverId == this.UniqueId
            //    && i.Msg == Enums.Telegrams.ProcessAttack);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.ProcessAttack:
                    ProcessAttack();
                    //_sprite.ProcessAttack();
                    return true;
            }
            return false;
        }

        public virtual void ProcessAttack()
        {
            // TODO THIS IS TEMPORARY, FIX ASAP
            //if (_physicsComponent == null)
            //    return;

            if (_damageComponent == null)
                throw new Exception("Damage component is required for the Attack state in entity " + _spriteBase.GetName());
            //base.ProcessAttack();

            MessageDispatcher2.Instance.DispatchMsg("AttackPerformed", 0f, this.UniqueId, _damageComponent.UniqueId, null);
            var colliderSearch = new ColliderSearch();
            var pos = _spriteBaseComponent.transform.position;
            //var animationComponent = entity.Components.GetComponent<IAnimationComponent>();
            var facingDirection = _animationComponent.GetFacingDirectionVector();
            //var pos = entity.transform.position;

            //GameDataController.Instance.GetLayerMask(LevelLayer.LevelOne);
            //var itemsHit = Physics2D.BoxCastAll(pos + _offset, _size, _angle, facingDirection, _distance, layerMask/*, GetEntityUI().GetTransform().gameObject.layer*/);
            var itemsHit = UnityEngine.Physics.BoxCastAll(pos + _offset.ToVector3(0), _size.ToVector3(0), facingDirection, Quaternion.identity, _distance);

            //var itemsHit = colliderSearch.BoxCastAll(_spriteBaseComponent, pos + _offset, _size, _angle, _distance, _sameLayer, _layer.LayerIndex);
            //colliderSearch.BoxCastAll(_spriteBaseComponent, _layer.LayerIndex);

            if (itemsHit == null)
                return;

            var skillUsed = _skill == null ? null : _skill.UniqueId;

            if (tempScrubbedColliders == null)
                tempScrubbedColliders = new List<ColliderSearchData>();
            colliderSearch.ColliderScrub(_collisionComponent, itemsHit, _targetTags, tempScrubbedColliders);

            foreach (var collider in tempScrubbedColliders)
            {
                _damageComponent.DamageExternalEntity(collider.EntityUniqueId, _damage, _spriteBase.GetTag(), collider.Point, collider.CollisionComponent, skillUsed);
            }
        }
    }
}
