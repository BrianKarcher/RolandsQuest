using RQ.Animation;
using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Enums;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action.Damage
{
    [Serializable]
    public class SetCollisionActionTypeAtom : AtomActionBase
    {
        private DamageEntityInfo _damageInfo;
        private DamageComponent _damageComponent;
        public CollisionActionType CollisionDamageType;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _damageComponent = entity.Components.GetComponent<DamageComponent>();
            var damageData = _damageComponent.GetDamageData();
            damageData.CollisionDamageType = CollisionDamageType;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
