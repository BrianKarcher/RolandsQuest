using RQ.Entity.Common;
using RQ.FSM.V2;
using RQ.FSM.V2.States;
using RQ.Messaging;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Active")]
    public class Active : PlaySoundState
    {
        protected PhysicsComponent _physicsComponent;
        protected DamageComponent _damageComponent;
        protected CollisionComponent _collisionComponent;
        protected FloorComponent _floorComponent;
        protected SteeringBehaviorManager _steering;
        protected SpriteBaseComponent _spriteBaseComponent;
        protected AIComponent _aiComponent;
        //protected EntityStatsData _entityStats;
        [SerializeField]
        private bool _takesDamage = true;
        [SerializeField]
        private bool _setDeflectStatus = false;
        [SerializeField]
        private bool _deflectValue = false;

        public override void SetupState()
        {
            base.SetupState();
            _physicsComponent = _spriteBase.Components.GetComponent<PhysicsComponent>();
            _damageComponent = _spriteBase.Components.GetComponent<DamageComponent>();
            _collisionComponent = _spriteBase.Components.GetComponent<CollisionComponent>();
            _spriteBaseComponent = _spriteBase as SpriteBaseComponent;
            _aiComponent = _spriteBase.Components.GetComponent<AIComponent>();
            _floorComponent = _spriteBase.Components.GetComponent<FloorComponent>();
            if (_physicsComponent != null)
            {
                _steering = _physicsComponent.GetSteering() as SteeringBehaviorManager;
                //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                //    Enums.Telegrams.GetSteering, null, (steering) => _steering = (SteeringBehaviorManager)steering);
            }
            //MessageDispatcher
            //base.SetEntity(entity);
            //_sprite = EntityUIBase.GetEntity(entity);
            //sprite = entity.GetComponent<ISprite>();
            //if (sprite == )
            //var entityUIBase = entity.GetComponent<EntityUIBase>();
            //_sprite = entityUIBase.GetRQObject() as ISprite;
            //if (_physicsComponent == null)
            //    Log.Info(_spriteBase.name + " - cannot find sprite for state " + this.name);
            //throw new Exception("FSM - Sprite not set.");
        }

        public override void Enter()
        {
            base.Enter();
            //_sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
            if (_damageComponent != null)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
                    Enums.Telegrams.SetTakesDamage, _takesDamage);
            }
            if (_setDeflectStatus)
                MessageDispatcher2.Instance.DispatchMsg("SetDeflecting", 0f, this.UniqueId, _componentRepository.UniqueId, _deflectValue);
            //var collisionComponents = _spriteBase.Components.GetComponents<CollisionComponent>();
            //if (collisionComponents != null)
            //{
            //    foreach (var collisionComponent in collisionComponents)
            //    {
            //        if (!collisionComponent.GetCollisionData().IsDeflectingAttacker)
            //            continue;

            //    }
            //}
        }

        public override void Exit()
        {
            base.Exit();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
