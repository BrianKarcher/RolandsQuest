using RQ.Common;
using RQ.Model.Item;
using RQ.Model.Physics;
using UnityEngine;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.AI;

namespace RQ.Entity.AtomAction.Action
{
    public class AttackAtom : AtomActionBase
    {
        [SerializeField]
        public ItemConfig _skill;
        public string AttackComponentName;
        private AttackComponent _attackComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _attackComponent = entity.Components.GetComponent<AttackComponent>(AttackComponentName);

            if (_skill != null)
                MessageDispatcher2.Instance.DispatchMsg("SkillUsed", 0f, entity.UniqueId, entity.UniqueId, _skill);
            _attackComponent.Attacked += _attackComponent_Attacked;
            _attackComponent.ProcessAttackDelayed();
        }

        private void _attackComponent_Attacked()
        {
            _isRunning = false;
        }

        public override void End()
        {
            base.End();
            _attackComponent.Attacked -= _attackComponent_Attacked;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}