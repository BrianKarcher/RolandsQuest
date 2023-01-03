using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    [Serializable]
    public class DamageEntityAtom : AtomActionBase
    {
        public float Amount;
        private GameObject _entityToDamage;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);

            var damageEntity = new DamageEntityInfo()
            {
                DamagedByEntity = entity,
                DamageAmount = Amount,
                DamagedBy = entity.name,
                HitPosition = entity.transform.position,
                Tag = entity.GetTag(),
                DamageSourceLocation = entity.transform.position,
                IsDamaged = true,
                CollisionDamageType = Model.Enums.CollisionActionType.Normal
            };

            var externalRepo = _entityToDamage.GetComponent<IComponentRepository>();
            MessageDispatcher2.Instance.DispatchMsg("ApplyDamage", 0f, entity.UniqueId, externalRepo.UniqueId, damageEntity);
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetEntityToDamage(GameObject entity)
        {
            _entityToDamage = entity;
        }
    }
}
