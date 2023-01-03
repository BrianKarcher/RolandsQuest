using RQ.Animation;
using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.Enums;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Interfaces;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using RQ.Common;
using UnityEngine;

namespace RQ.Controller.Actions
{
    public abstract class ActionBase : MessagingObject, IAction
    {
        protected IBasicPhysicsComponent _physicsComponent;
        protected DamageComponent _damageComponent;
        protected CollisionComponent _collisionComponent;
        protected SteeringBehaviorManager _steering;
        private IComponentRepository _entity;
        protected AIComponent _aiComponent;
        protected IList<IBaseObject> _animationComponents;

        //protected EntityStatsData _entityStats;
        private IState _state;

        // TODO Serialize this field
        private bool _isComplete = false;
        protected bool _isRunning = false;

        public virtual void InitAction()
        {
            if (_entity == null)
                return;
            _physicsComponent = _entity.Components.GetComponent<IBasicPhysicsComponent>();
            _damageComponent = _entity.Components.GetComponent<DamageComponent>();
            _collisionComponent = _entity.Components.GetComponent<CollisionComponent>();
            //_spriteBaseComponent = entity.GetComponent<SpriteBaseComponent>();
            _aiComponent = _entity.Components.GetComponent<AIComponent>();
            _animationComponents = _entity.Components.GetComponents<AnimationComponent>();
            if (_physicsComponent != null)
            {
                _steering = _physicsComponent.GetSteering() as SteeringBehaviorManager;
                //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                //    Enums.Telegrams.GetSteering, null, (steering) => _steering = (SteeringBehaviorManager)steering);
            }
        }

        public void SetState(IState state)
        {
            _state = state;
            _entity = state.GetEntity();
        }

        public void SetComponentRepository(IComponentRepository entity)
        {
            _entity = entity;
        }

        protected IComponentRepository GetEntity()
        {
            return _entity;
        }

        public virtual void Act(Component otherRigidBody)
        {
            _isComplete = false;
            _isRunning = true;
        }
        public virtual void ActExit(Component otherRigidBody)
        {
            _isRunning = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo,
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _entity.UniqueId,
                msg, extraInfo, null, earlyTermination);
        }

        public void SendMessageToSelf(float delay, Telegrams msg, object extraInfo,
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, this.UniqueId,
                msg, extraInfo, null, earlyTermination);
        }

        public void Reset()
        {
            _isComplete = false;
        }

        protected void Complete()
        {
            _isComplete = true;
            if (_state != null)
                _state.Complete();
        }

        protected bool IsComplete()
        {
            return _isComplete;
        }

        public virtual void Serialize(EntitySerializedData entitySerializedData)
        { }

        public virtual void Deserialize(EntitySerializedData entitySerializedData)
        { }

        public virtual void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        {
            var name = GetName();
            if (!entitySerializedData.UniqueIdMappings.ContainsKey(name))
                return;
            var mapping = entitySerializedData.UniqueIdMappings[name];
            SetUniqueId(mapping, true);
        }

        protected void SerializeComponent(EntitySerializedData entitySerializedData, object data)
        {
            var key = GetName();
            if (entitySerializedData.ComponentData.ContainsKey(key))
                throw new Exception("Key " + key + " already exists in " + entitySerializedData.Name);
            entitySerializedData.ComponentData.Add(key, data);
        }

        protected T DeserializeComponent<T>(EntitySerializedData entitySerializedData) where T : class
        {
            object data;
            if (!entitySerializedData.ComponentData.TryGetValue(GetName(), out data))
                return default(T);
            return Persistence.DeserializeObject<T>(data);
            //return (T)data;
        }
    }
}
