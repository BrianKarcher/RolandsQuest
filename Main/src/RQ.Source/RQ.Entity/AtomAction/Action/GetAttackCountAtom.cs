using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class GetAttackCountAtom : AtomActionBase
    {
        private DamageComponent _damageComponent;
        public int _attackCountTrigger;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _damageComponent = entity.Components.GetComponent<DamageComponent>();
            //_physicsData = _physicsComponent.GetPhysicsData();            
        }

        public override AtomActionResults OnUpdate()
        {
            if (_damageComponent != null)
            {
                var attackCount = _damageComponent.GetDamageData().AttackCount;
                if (attackCount == 0 && _attackCountTrigger != 0)
                    return AtomActionResults.Failure;
                var result = attackCount % _attackCountTrigger == 0;
                return result ? AtomActionResults.Success : AtomActionResults.Failure;
                //var animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                //return animationComponent.Data.IsAnimationComplete;
            }
            return AtomActionResults.Failure;
        }
    }
}
