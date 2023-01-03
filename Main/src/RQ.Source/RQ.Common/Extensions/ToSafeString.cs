using RQ.Messaging;
using RQ.Serialization;
using UnityEngine;
namespace RQ.Extensions
{
    public static class Extensions
    {
        public static string ToSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        public static T DeserializeComponent<T>(this EntitySerializedData entitySerializedData, MessagingObject source) where T : class
        {
            object data;
            if (!entitySerializedData.ComponentData.TryGetValue(source.GetName(), out data))
                return default(T);
            return Persistence.DeserializeObject<T>(data);
            //return (T)data;
        }

        public static void SerializeComponent(this EntitySerializedData entitySerializedData, MessagingObject source, object data)
        {
            entitySerializedData.ComponentData.Add(source.GetName(), data);
        }
    }
}
