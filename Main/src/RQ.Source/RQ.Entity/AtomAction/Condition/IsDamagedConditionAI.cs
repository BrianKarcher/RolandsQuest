using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Model.Enums;
using RQ.Physics.Components;
using System;

namespace RQ.AI.Conditions
{
    public class IsDamagedConditionAI : AtomActionBase
    {
        public CollisionActionType CollisionDamageType = CollisionActionType.Normal;
        protected DamageComponent _damageComponent;
        protected DamageEntityInfo _damageInfo;
        private bool _isDamaged = false;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _damageComponent = entity.Components.GetComponent<DamageComponent>();
            if (_damageComponent == null)
                throw new Exception("Damage Component is required for condition IsDammaged in entity " + entity.GetName());
            _damageInfo = _damageComponent.GetDamageInfo();
            _isDamaged = false;
            // TODO This statement feels weird
            //_damageInfo.IsDamaged = false;
        }

        public override AtomActionResults OnUpdate()
        {
            if (_damageInfo == null)
                throw new Exception("No damage info in IsDamagedCondition");
            if (_damageInfo.IsDamaged)
                _isDamaged = true;
            if (_damageInfo.CollisionDamageType != CollisionDamageType)
                return AtomActionResults.Failure;

            return _isDamaged ? AtomActionResults.Success : AtomActionResults.Failure;
        }
    }
}
