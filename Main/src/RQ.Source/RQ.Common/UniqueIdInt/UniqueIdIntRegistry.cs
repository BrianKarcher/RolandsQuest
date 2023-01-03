using System;
using System.Collections.Generic;

namespace RQ.Common.UniqueIdInt
{
    public static class UniqueIdIntRegistry
    {
        public static Dictionary<String, Int32> Mapping = new Dictionary<String, int>();

        public static void Deregister(String id)
        {
            Mapping.Remove(id);
        }

        public static void Register(String id, Int32 value)
        {
            if (!Mapping.ContainsKey(id))
                Mapping.Add(id, value);
        }

        public static Int32 GetInstanceId(string id)
        {
            return Mapping[id];
        }
    }
}
