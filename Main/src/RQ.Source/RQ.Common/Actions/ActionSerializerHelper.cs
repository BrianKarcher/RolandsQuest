using RQ.Model;
using RQ.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RQ.Common.Actions
{
    public class ActionSerializerHelper
    {
        public static void SerializeActions(EntitySerializedData entitySerializedData, GameObject gameObject,
            bool searchChildren = false)
        {
            var actions = GetActions(gameObject, searchChildren);
            foreach (var action in actions)
            {
                action.Serialize(entitySerializedData);
            }
        }

        public static void DeserializeActions(EntitySerializedData entitySerializedData, GameObject gameObject,
            bool searchChildren = false)
        {
            var actions = GetActions(gameObject, searchChildren);
            foreach (var action in actions)
            {
                action.Deserialize(entitySerializedData);
            }
        }

        public static void DeserializeActionUniqueIds(EntitySerializedData entitySerializedData, GameObject gameObject,
            bool searchChildren = false)
        {
            var actions = GetActions(gameObject, searchChildren);
            foreach (var action in actions)
            {
                action.DeserializeUniqueIds(entitySerializedData);
            }
        }

        private static IAction[] GetActions(GameObject gameObject, bool searchChildren)
        {
            if (searchChildren)
                return gameObject.GetComponentsInChildren<IAction>();
            else
                return gameObject.GetComponents<IAction>();
        }
    }
}
