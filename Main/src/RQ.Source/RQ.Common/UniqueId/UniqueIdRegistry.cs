using System;
using System.Collections.Generic;

namespace RQ.Common.UniqueId
{
    public static class UniqueIdRegistry
    {
        public static Dictionary<String, Int32> Mapping = new Dictionary<String, int>();

        public static void Deregister(String id)
        {
            //Debug.Log("UniqueIdRegistry - Removing id " + id);
            if (Mapping.ContainsKey(id))
                Mapping.Remove(id);
        }

        public static void Register(String id, Int32 value)
        {
            if (!Mapping.ContainsKey(id))
            {
                //Debug.Log("UniqueIdRegistry - Adding id " + id + " value " + value);
                Mapping.Add(id, value);
            }
        }

        public static Int32 GetInstanceId(string id)
        {
            if (!Mapping.ContainsKey(id))
            {
                //Debug.Log("UniqueIdRegistry - cannot find id " + id + " in Update.");
                return 0;
            }
            return Mapping[id];
        }

        public static void Clear()
        {
            Mapping.Clear();
        }

        public static int Count()
        {
            return Mapping.Count;
        }
    }
}
