using System;
using RQ.Common;
using RQ.Common.Components;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Messaging;
using RQ.Physics.Collision;
using RQ.Physics.Data;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.Components
{
    /// <summary>
    /// Component for taking and receiving damage and collision/trigger hits
    /// </summary>
    [AddComponentMenu("RQ/Components/Damage Component")]
    public class DamageComponent : ComponentPersistence<DamageComponent>
    {
        [SerializeField]
        private DamageData _damageData = null;

        [SerializeField]
        [Tag]
        // TODO - Remove, seems to be duplicate of TagsYouDamage
        private string[] _ourDamageTags;

        // TODO Allow user to enter colliders that cause damage.

        [SerializeField]
        private bool _useDamageColor;
        public bool UseDamageColor { get { return _useDamageColor; } set { _useDamageColor = value; } }

        [SerializeField]
        private Color _damageColor;
        public Color DamageColor { get { return _damageColor; } set { _damageColor = value; } }

        private CollisionComponent _collisionComponent;
        private PhysicsComponent _physicsComponent;

        private DamageEntityInfo DamageInfo { get; set; }

        [SerializeField]
        [Tag]
        private string[] _tagsYouDamage;

        [SerializeField]
        private GameObject _hitEffect;

        [SerializeField]
        private AudioClip _killSound;

        [SerializeField]
        private AudioClip _damageSound;

        [SerializeField]
        private GameObject _death;

        [SerializeField]
        private GameObject _deflectPrefab;

        private long _applyDamageId, _entityDiedId, _addEntityDeathNotificationId, _removeEntityDeathNotificationId, 
            _externalDamageForcedMessageId, _externalSkillDamageId, _setDamageSourceLocationId, _attackPerformedId, _attackPerformedId2;

        private Action<Telegram2> _applyDamageDelegate, _entityDiedDelegate, _addEntityDeathNotificationDelegate, 
            _removeEntityDeathNotificationDelegate, _externalDamageForcedMessageDelegate, _externalSkillDamageDelegate, 
            _setDamageSourceLocationDelegate, _attackPerformedDelegate;

        public override void Awake()
        {
            base.Awake();
            DamageInfo = new DamageEntityInfo();
            _applyDamageDelegate = (data) =>
            {
                // The previous damage has not been reacted to yet. Do not accept more damage until
                // previous has been reacted to.
                if (DamageInfo.IsDamaged)
                    return;
                //if (_sprite == null)
                //    return false;
                var damageInfo = (DamageEntityInfo) data.ExtraInfo;

                if (damageInfo.CollisionHit != null)
                {
                    if (damageInfo.CollisionHit.GetCollisionData().CurrentlyDeflecting)
                    {
                        var attackerId = damageInfo.DamagedByEntity.UniqueId;
                        if (_hitEffect != null)
                            CreateHitEffect(damageInfo.HitPosition);
                        MessageDispatcher2.Instance.DispatchMsg("SetDamageSourceLocation", 0f, this.UniqueId,
                            attackerId, (Vector2D) _componentRepository.transform.position);
                        MessageDispatcher2.Instance.DispatchMsg("Deflect", 0f, this.UniqueId,
                            attackerId, null);
                    }
                    if (!damageInfo.CollisionHit.ReceivesDamage())
                        return;
                }
                if (!_damageData.TakesDamage)
                    return;

                if (!_damageData.Vulnerable)
                {
                    MessageDispatcher2.Instance.DispatchMsg("AttackedButInvulnerable", 0f, this.UniqueId,
                        _componentRepository.UniqueId, null);
                    return;
                }
                //var damageInfo = msg.ExtraInfo as DamageEntityInfo;
                //var entityStats = _sprite.GetEntityStats();
                //entityStats.DamageInfo = damageInfo;
                //damageInfo.IsDamaged = true;
                //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
                //    Enums.Telegrams.SetDamageEntityInfo, damageInfo);
                DamageSelf(damageInfo);

                //_physicsComponent.DamageInfo = damageInfo;
                //_physicsComponent.IsDamaged = true;
                //StateMachine.GetStateInfo().IsDamaged = true;
                //_sprite.ProcessAttack();
            };
            _entityDiedDelegate = (data) =>
            {
                Debug.LogWarning(_componentRepository.name + " (DamageComponent) Entity Died");
                // Entity died?  Notify whoever may be listening
                if (_damageData.EntityDeathNotification == null)
                    return;
                MessageDispatcher2.Instance.DispatchMsgToList("EntityDied", 0f, this.UniqueId, _damageData.EntityDeathNotification, _componentRepository.UniqueId);
            };
            _addEntityDeathNotificationDelegate = (data) =>
            {
                var entityUniqueId = (string)data.ExtraInfo;
                Debug.LogWarning(_componentRepository.name + " (DamageComponent) Added entity death notification " + entityUniqueId);
                if (_damageData.EntityDeathNotification == null)
                    _damageData.EntityDeathNotification = new HashSet<string>();
                _damageData.EntityDeathNotification.Add(entityUniqueId);
            };
            _removeEntityDeathNotificationDelegate = (data) =>
            {
                var entityUniqueId = (string)data.ExtraInfo;
                Debug.LogWarning(_componentRepository.name + " (DamageComponent) Removed entity death notification " + entityUniqueId);
                if (_damageData.EntityDeathNotification.Contains(entityUniqueId))
                    _damageData.EntityDeathNotification.Remove(entityUniqueId);
            };
            _externalDamageForcedMessageDelegate = (data) =>
            {
                var damageString = (string)data.ExtraInfo;
                float damage;
                if (!float.TryParse(damageString, out damage))
                    return;
                var damageInfo = new DamageEntityInfo()
                {
                    DamageAmount = damage,
                    DamagedBy = string.Empty,
                    DamageSourceLocation = _componentRepository.transform.position,
                    IsDamaged = true,
                    HitPosition = _componentRepository.transform.position
                };
                DamageSelf(damageInfo);
            };
            _setDamageSourceLocationDelegate = (data) =>
            {
                DamageInfo.DamageSourceLocation = (Vector2D)data.ExtraInfo;
            };
            _externalSkillDamageDelegate = (data) =>
            {
                var damageInfo = (DamageEntityInfo)data.ExtraInfo;
                if (!_damageData.TakesSkillDamage)
                    return;
                if (!_damageData.Vulnerable)
                {
                    MessageDispatcher2.Instance.DispatchMsg("AttackedButInvulnerable", 0f, this.UniqueId,
                        _componentRepository.UniqueId, null);
                    return;
                }
                _damageData.AttackedBySkill = damageInfo.SkillUsed;

                DamageSelf(damageInfo);
            };
            _attackPerformedDelegate = (data) =>
            {
                _damageData.AttackCount++;
            };
        }

        //public override void Init()
        //{
        //    base.Init();            
        //}

        public override void Start()
        {
            base.Start();
            _collisionComponent = _componentRepository.Components.GetComponent<CollisionComponent>();
            _physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();
        }

        public override void StartListening()
        {
            base.StartListening();
            _applyDamageId = MessageDispatcher2.Instance.StartListening("ApplyDamage", _componentRepository.UniqueId, _applyDamageDelegate);

            _entityDiedId = MessageDispatcher2.Instance.StartListening("EntityDied", _componentRepository.UniqueId, _entityDiedDelegate);
            //_componentRepository.StartListening("EntityDied", this.UniqueId, );

            _addEntityDeathNotificationId = MessageDispatcher2.Instance.StartListening("AddEntityDeathNotification", _componentRepository.UniqueId, _addEntityDeathNotificationDelegate);
            //_componentRepository.StartListening("AddEntityDeathNotification", this.UniqueId, _addEntityDeathNotificationDelegate);
            _removeEntityDeathNotificationId = MessageDispatcher2.Instance.StartListening("RemoveEntityDeathNotification", 
                _componentRepository.UniqueId, _removeEntityDeathNotificationDelegate);

            //_componentRepository.StartListening("RemoveEntityDeathNotification", this.UniqueId, );
            _externalDamageForcedMessageId = MessageDispatcher2.Instance.StartListening("ExternalDamageForcedMessage",
                _componentRepository.UniqueId, _externalDamageForcedMessageDelegate);
            //_componentRepository.StartListening("ExternalDamageForcedMessage", this.UniqueId, );
            _setDamageSourceLocationId = MessageDispatcher2.Instance.StartListening("SetDamageSourceLocation",
                _componentRepository.UniqueId, _setDamageSourceLocationDelegate);
            //_componentRepository.StartListening("SetDamageSourceLocation", this.UniqueId, );
            _externalSkillDamageId = MessageDispatcher2.Instance.StartListening("ExternalSkillDamage",
                _componentRepository.UniqueId, _externalSkillDamageDelegate);
            //_componentRepository.StartListening("ExternalSkillDamage", this.UniqueId, );
            _attackPerformedId = MessageDispatcher2.Instance.StartListening("AttackPerformed",
                _componentRepository.UniqueId, _attackPerformedDelegate);
            _attackPerformedId2 = MessageDispatcher2.Instance.StartListening("AttackPerformed",
                this.UniqueId, _attackPerformedDelegate);
            //_componentRepository.StartListening("AttackPerformed", this.UniqueId, , true);
            //_componentRepository.StartListening("AddDamageNotify", this.UniqueId, (data) =>
            //{
            //    _damageData.DamageNotify.Add(data.ReceiverId);
            //});
            //_componentRepository.StartListening("RemoveDamageNotify", this.UniqueId, (data) =>
            //{
            //    _damageData.DamageNotify.Remove(data.ReceiverId);
            //});
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("ApplyDamage", _componentRepository.UniqueId, _applyDamageId);
            MessageDispatcher2.Instance.StopListening("EntityDied", _componentRepository.UniqueId, _entityDiedId);
            //_componentRepository.StopListening("EntityDied", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("AddEntityDeathNotification", _componentRepository.UniqueId, _addEntityDeathNotificationId);
            //_componentRepository.StopListening("AddEntityDeathNotification", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("RemoveEntityDeathNotification", _componentRepository.UniqueId, _removeEntityDeathNotificationId);

            //_componentRepository.StopListening("RemoveEntityDeathNotification", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("ExternalDamageForcedMessage", _componentRepository.UniqueId, _externalDamageForcedMessageId);

            //_componentRepository.StopListening("ExternalDamageForcedMessage", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetDamageSourceLocation", _componentRepository.UniqueId, _setDamageSourceLocationId);

            MessageDispatcher2.Instance.StopListening("ExternalSkillDamage", _componentRepository.UniqueId, _externalSkillDamageId);


            //_componentRepository.StopListening("ExternalSkillDamage", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("AttackPerformed", _componentRepository.UniqueId, _attackPerformedId);
            MessageDispatcher2.Instance.StopListening("AttackPerformed", _componentRepository.UniqueId, _attackPerformedId2);

            //_componentRepository.StopListening("AttackPerformed", this.UniqueId, true);
            //_componentRepository.StopListening("RemoveDamageNotify", this.UniqueId);
        }

        public override bool HandleMessage(Telegram msg)
        {
            switch (msg.Msg)
            {
                case Enums.Telegrams.ExternalDamage:
                    // The previous damage has not been reacted to yet. Do not accept more damage until
                    // previous has been reacted to.
                    if (DamageInfo.IsDamaged)
                        return true;
                    //if (_sprite == null)
                    //    return false;
                    var damageInfo = (DamageEntityInfo)msg.ExtraInfo;
                    if (damageInfo == null)
                    {
                        Debug.LogError("No DamageEntityInfo in ExternalDamage");
                        return true;
                    }

                    if (damageInfo.CollisionHit != null)
                    {
                        if (damageInfo.CollisionHit.GetCollisionData().CurrentlyDeflecting)
                        {
                            var attackerId = damageInfo.DamagedByEntity.UniqueId;

                            var vectorToDamageSource = damageInfo.DamageSourceLocation - (Vector2)_componentRepository.transform.position;
                            //var rotation = new Quaternion(vectorToDamageSource.x, vectorToDamageSource.y, 0f, 0f);
                            //var vector = 
                            //var angle = Vector2.SignedAngle(_componentRepository.transform.position, damageInfo.DamageSourceLocation);
                            var angle = Vector2.SignedAngle(Vector2.right, vectorToDamageSource);
                            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                            if (_deflectPrefab != null)
                            {
                                var deflectGO = GameObject.Instantiate(_deflectPrefab, _componentRepository.transform.position, rotation);
                                deflectGO.transform.SetParent(_componentRepository.transform);
                            }
                            MessageDispatcher2.Instance.DispatchMsg("SetDamageSourceLocation", 0f, this.UniqueId,
                                attackerId, (Vector2D)_componentRepository.transform.position);
                            MessageDispatcher2.Instance.DispatchMsg("Deflect", 0f, this.UniqueId,
                                attackerId, null);
                        }
                    }

                    if (!_damageData.TakesDamage)
                        return false;

                    if (damageInfo.CollisionHit != null && !damageInfo.CollisionHit.ReceivesDamage())
                        return false;

                    if (!_damageData.Vulnerable)
                    {
                        MessageDispatcher2.Instance.DispatchMsg("AttackedButInvulnerable", 0f, this.UniqueId,
                            _componentRepository.UniqueId, null);
                        return false;
                    }
                    //var damageInfo = msg.ExtraInfo as DamageEntityInfo;
                    //var entityStats = _sprite.GetEntityStats();
                    //entityStats.DamageInfo = damageInfo;
                    //damageInfo.IsDamaged = true;
                    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
                    //    Enums.Telegrams.SetDamageEntityInfo, damageInfo);
                    DamageSelf(damageInfo);

                    //_physicsComponent.DamageInfo = damageInfo;
                    //_physicsComponent.IsDamaged = true;
                    //StateMachine.GetStateInfo().IsDamaged = true;
                    //_sprite.ProcessAttack();
                    return false;
                case Telegrams.GetDamageEntityInfo:
                    //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, msg.SenderId,
                    //    Telegrams.SetDamageEntityInfo, DamageInfo);
                    msg.Act(DamageInfo);
                    break;
                //case Telegrams.SetDamageEntityInfo:
                //    DamageInfo = (DamageEntityInfo)msg.ExtraInfo;
                //    //_componentRepository.SendMessageToAllButThis(0f, this.UniqueId, Telegrams.SetDamageEntityInfo, DamageInfo);
                //    //SendMessageToAllButThis();
                //    break;
                case Enums.Telegrams.TriggerEnter:
                    if (msg.ExtraInfo != null)
                    {
                        var collider2D = msg.ExtraInfo as CollisionMessageData;
                        return ProcessCollision(collider2D, true);
                    }
                    break;
                case Enums.Telegrams.CollisionEnter:
                    if (msg.ExtraInfo != null)
                    {
                        var collision2D = msg.ExtraInfo as CollisionMessageData;
                        return ProcessCollision(collision2D, false);
                    }
                    // TODO FIX THIS - The state transition table should be determing the next state
                    //data.GetFSM().RevertToPreviousState();
                    break;
                case Telegrams.SetTakesDamage:
                    _damageData.TakesDamage = (bool)msg.ExtraInfo;
                    break;
            }
            return false;
        }

        private void DamageSelf(DamageEntityInfo damageInfo)
        {
            DamageInfo.DamageAmount = damageInfo.DamageAmount;
            //Debug.LogError("Damaged " + damageInfo.DamageAmount);
            DamageInfo.DamagedByEntity = damageInfo.DamagedByEntity;
            DamageInfo.DamagedBy = damageInfo.DamagedBy;
            DamageInfo.DamageSourceLocation = damageInfo.DamageSourceLocation;
            DamageInfo.Tag = damageInfo.Tag;
            DamageInfo.IsDamaged = true;
            DamageInfo.CollisionDamageType = damageInfo.CollisionDamageType;
            //SendDamageNotification();
            MessageDispatcher2.Instance.DispatchMsg("Damaged", 0f, this.UniqueId,
                _componentRepository.UniqueId, DamageInfo);
            _componentRepository.SendMessageToAllButThis(0f, this.UniqueId, Telegrams.Damaged, DamageInfo);
            if (_hitEffect != null)
                CreateHitEffect(damageInfo.HitPosition);
            
        }

        private void CreateHitEffect(Vector2D hitPosition)
        {
            var hitPosition3 = hitPosition.ToVector3(this.transform.position.z + -0.1f);
            //Debug.LogWarning("Hit position " + hitPosition);
            GameObject.Instantiate(_hitEffect, hitPosition3, Quaternion.identity);
        }

        //private void SendDamageNotification()
        //{
        //    foreach (var destId in _damageData.DamageNotify)
        //        MessageDispatcher2.Instance.DispatchMsg("Damaged", 0f, this.UniqueId, destId, _damageData);
        //}

        // Damage an external entity
        public void DamageExternalEntity(string receiverId, float damageAmount, string tag, Vector2 hitPosition, ICollisionComponent hitCollision, string skillUsed)
        {
            var damageInfo = new DamageEntityInfo()
            {
                DamageAmount = damageAmount,
                DamagedBy = _componentRepository.UniqueId,
                DamagedByEntity = _componentRepository,
                //damageInfo.DamagedBy = transform.GetComponent<IComponentRepository>().UniqueId;
                DamageSourceLocation = this.transform.position,
                HitPosition = hitPosition,
                Tag = tag,
                CollisionHit = hitCollision,
                SkillUsed = skillUsed,
                CollisionDamageType = this._damageData.CollisionDamageType
            };

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, receiverId, Enums.Telegrams.ExternalDamage, damageInfo);
        }

        public void DamageExternalEntityBySkill(string receiverId, float damageAmount, string tag, Vector2 hitPosition, ICollisionComponent hitCollision, string skillUsed)
        {
            var damageInfo = new DamageEntityInfo()
            {
                DamageAmount = damageAmount,
                DamagedBy = _componentRepository.UniqueId,
                DamagedByEntity = _componentRepository,
                //damageInfo.DamagedBy = transform.GetComponent<IComponentRepository>().UniqueId;
                DamageSourceLocation = this.transform.position,
                HitPosition = hitPosition,
                Tag = tag,
                CollisionHit = hitCollision,
                SkillUsed = skillUsed
            };

            MessageDispatcher2.Instance.DispatchMsg("ExternalSkillDamage", 0f, this.UniqueId, receiverId, damageInfo);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, receiverId, Enums.Telegrams.ExternalDamage, damageInfo);
        }

        public DamageData GetDamageData()
        {
            return _damageData;
        }

        //private bool ProcessCollision(Collider collider, string colliderTag, bool isTrigger, Vector3 hitPosition)
        //{
        private bool ProcessCollision(CollisionMessageData data, bool isTrigger)
        {
            var doWeDeliverDamage = data.OurCollisionComponent?.GetCollisionData()?.DamageCollider;
            if (doWeDeliverDamage != null && !doWeDeliverDamage.Value)
                return false;

            var isInDamageTags = Array.IndexOf(_ourDamageTags, data.ThisTag) > -1;
            if (_ourDamageTags.Length != 0 && !isInDamageTags)
                return false;

            if (!_damageData.DamageTargetOnCollision && !_damageData.DamageTargetOnTrigger)
                return false;

            if (data.OtherCollider == null || data.OtherCollider.attachedRigidbody == null)
                return false;

            var tags = data.OurCollisionComponent == null
                ? _tagsYouDamage
                    : data.OurCollisionComponent.GetCollisionData().Tags;

            var containsTag = Array.IndexOf(tags, data.OtherCollider.tag) > -1;
            //if (!_tagsYouDamage.Contains(data.OtherCollider.tag))
            if (!containsTag)
                return false;

            //var targetEntity = EntityUIBase.GetEntity<IBaseGameEntity>(
            //    collider.attachedRigidbody.transform);
            var targetEntity = data.OtherCollider.attachedRigidbody.transform.GetComponent<IComponentRepository>();
            if (targetEntity == null)
                return false;
            var targetEntityPhysicsComponent = targetEntity.Components.GetComponent<PhysicsComponent>();
            //var targetEntityCollisionComponent = targetEntity.Components.GetComponents<CollisionComponent>().FirstOrDefault(i => i.ContainsCollider(data.OtherCollider));
            //var targetEntityCollisionComponents = targetEntity.Components.GetComponents<CollisionComponent>();
            var targetEntityCollisionComponent = data.OtherCollider.GetComponent<CollisionComponent>();
            //CollisionComponent targetEntityCollisionComponent = null;
            //foreach (var baseObject in targetEntityCollisionComponents)
            //{
            //    var iteratetargetEntityCollisionComponent = baseObject as CollisionComponent;
            //    if (iteratetargetEntityCollisionComponent.ContainsCollider(data.OtherCollider))
            //    {
            //        targetEntityCollisionComponent = iteratetargetEntityCollisionComponent;
            //        break;
            //    }
            //}
            var targetEntityFloorComponent = targetEntity.Components.GetComponent<FloorComponent>();
            if (targetEntityCollisionComponent == null || !targetEntityCollisionComponent.ReceivesDamage())
                return false;
            //var targetEntityDamageComponent = targetEntity.Components.GetComponents<CollisionComponent>();

            //var entity = GetEntity() as IBaseGameEntity;
            var damageData = GetDamageData();
            //var collisionData = _collisionComponent.GetCollisionData();

            var floorComponent = _componentRepository.Components.GetComponent<FloorComponent>();

            // Level check
            //if (!PhysicsCollision.LevelMatch(collisionData.Level, floorComponent, targetEntityFloorComponent))
            //    return false;

            // Tag check between this entity and other tag
            //if (!collisionData.Tags.Contains(collider.tag))
            //    return false;

            // Tag check between this tag and other entity
            //var targetCollisionData = targetEntityDamageComponent.GetCollisionData();
            //if (!targetCollisionData.Tags.Contains(_physicsComponent.GetTag()))
            //    return false;

            // All passed? Set _collided
            var receiverId = targetEntity.UniqueId;
            //var id = _componentRepositoryId;
            //if (!collisionData.NearbyEntities.Contains(id))
            //    collisionData.NearbyEntities.Add(id);

            bool causeDamage = false;
            if (damageData.DamageTargetOnCollision && !isTrigger)
                causeDamage = true;

            if (damageData.DamageTargetOnTrigger && isTrigger)
                causeDamage = true;

            if (causeDamage)
            {
                var collisionComponent = data.OtherCollider.GetComponent<ICollisionComponent>();
                DamageExternalEntity(receiverId, damageData.DamageOnTouch, this.tag, data.HitPosition, collisionComponent, null);
            }

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

        public DamageEntityInfo GetDamageInfo()
        {
            return DamageInfo;
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    base.SerializeComponent(entitySerializedData, _damageData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    _damageData = base.DeserializeComponent<DamageData>(entitySerializedData);
        //}

        public AudioClip GetDamageSound()
        {
            return _damageSound;
        }

        public AudioClip GetKillSound()
        {
            return _killSound;
        }

        public GameObject GetDeathPrefab()
        {
            return _death;
        }
    }
}
