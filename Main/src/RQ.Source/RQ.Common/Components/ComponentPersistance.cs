using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Serialization;
using System;
using RQ.Common.Controllers;
using UnityEngine;

namespace RQ.Common.Components
{
    public class ComponentPersistence<T> : ComponentBase<T>, ISerializableObject where T : class, IComponentBase
    {
        public override void Awake()
        {
            //if (!Application.isPlaying)
            //    return;
            // This is called prior to the base.Awake so we set the UniqueId before the listeners get set
            if (GameDataController.Instance.LoadingEntity != null)
                DeserializeUniqueIds(GameDataController.Instance.LoadingEntity);
            base.Awake();
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {                
                case Telegrams.Serialize:
                    var entitySerializedData = msg.ExtraInfo as EntitySerializedData;
                    Serialize(entitySerializedData);
                    break;
                case Telegrams.Deserialize:
                    var entityDeserializedData = msg.ExtraInfo as EntitySerializedData;
                    Deserialize(entityDeserializedData);
                    break;                
            }
            return false;
        }

        public virtual void Serialize(EntitySerializedData entitySerializedData)
        {
            var name = GetName();
            //var mapping = new StateMapping()
            //{
            //    Name = name,
            //    UniqueId = UniqueId
            //};
            if (entitySerializedData.UniqueIdMappings.ContainsKey(name))
                throw new Exception("Component " + this.name + "(" + name + ") already exists in the dictionary for entity " + _componentRepository.name + "(" + _componentRepository.GetName() + ")");
            entitySerializedData.UniqueIdMappings.Add(name, UniqueId);
            //entitySerializedData.TransformData = this.transform.Serialize();
            //if (componentBaseData != null)
            //    componentBaseData.enabled = gameObject.activeSelf;
        }

        protected void SerializeComponent(EntitySerializedData entitySerializedData, object data)
        {
            entitySerializedData.ComponentData.Add(GetName(), data);
        }

        public virtual void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        {
            var name = GetName();
            if (!entitySerializedData.UniqueIdMappings.ContainsKey(name))
                return;
            var mapping = entitySerializedData.UniqueIdMappings[name];
            SetUniqueId(mapping, true);
        }

        public virtual void Deserialize(EntitySerializedData entitySerializedData)
        {
            //this.transform.Deserialize(entitySerializedData.TransformData);            
            //if (componentBaseData != null)
            //    gameObject.SetActive(componentBaseData.enabled);
        }

        protected T DeserializeComponent<T>(EntitySerializedData entitySerializedData)
        {
            object data;
            if (!entitySerializedData.ComponentData.TryGetValue(GetName(), out data))
                return default(T);
            return Persistence.DeserializeObject<T>(data);
            //return (T)data;
        }

        public void SendMessageToSpriteBase(float delay, Telegrams msg, object extraInfo, 
            TelegramEarlyTermination earlyTermination = TelegramEarlyTermination.None)
        {
            if (_componentRepositoryId == null)
            {
                return;
            }
            MessageDispatcher.Instance.DispatchMsgWithEarlyTermination(delay, this.UniqueId, _componentRepositoryId, 
                msg, extraInfo, null, earlyTermination);
        }
    }
}
