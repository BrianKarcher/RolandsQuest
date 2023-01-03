using Newtonsoft.Json;

namespace RQ.Common.Serialization
{
    public static class JsonSerializer
    {
        public static string Serialize<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
