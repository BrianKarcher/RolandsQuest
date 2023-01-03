using RQ.Messaging;
using RQ.Serialization;
using System;

namespace RQ.Controller.Actions.Conditionals
{
    public class ConditionalBase : MessagingObject
    {
        public OperatorEnum Operator;

        public virtual bool Check()
        {
            return false;
        }

        //public virtual void Act()
        //{

        //}

        public virtual void InitCondition()
        {

        }

        public virtual void Serialize(EntitySerializedData entitySerializedData)
        { }

        public virtual void Deserialize(EntitySerializedData entitySerializedData)
        { }

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
