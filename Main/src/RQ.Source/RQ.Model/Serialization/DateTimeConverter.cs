using Newtonsoft.Json;
using System;

namespace RQ.Model.Serialization
{
    public class DateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime dateTime = (DateTime) value;
            var dateTimeString = dateTime.ToString("MM-dd-yyyy hh:mm:ss");

            //writer.WriteValue(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(value)).ToString(_dateFormat));
            writer.WriteValue(dateTimeString);

            //JToken t = JToken.FromObject(value);

            //if (t.Type != JTokenType.Object)
            //{
            //    t.WriteTo(writer);
            //}
            //else
            //{
            //    JObject o = (JObject)t;
            //    IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            //    o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

            //    o.WriteTo(writer);
            //}
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateTimeString = (string)reader.Value;
            DateTime dateTime;
            DateTime.TryParse(dateTimeString, out dateTime);
            return dateTime;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}
