using RQ.Common;
using RQ.Common.Components;
using RQ.Model;
using RQ.Model.Serialization;
using RQ.Extensions;
using RQ.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.Components
{
    /// <summary>
    /// Place this wherever a gameObject's state needs to be persisted to the save file.
    /// </summary>
    [AddComponentMenu("RQ/Components/Game Object State Persistence")]
    public class GameObjectStatePersistenceComponent : ComponentPersistence<GameObjectStatePersistenceComponent>
    {
        public override void Serialize(EntitySerializedData entityData)
        {
            base.Serialize(entityData);
            GameObjectStateData _gameObjectStateData = new GameObjectStateData();
            _gameObjectStateData.TransformData = this.transform.Serialize();
            _gameObjectStateData.enabled = gameObject.activeSelf;
            _gameObjectStateData.ObjectData = new Dictionary<string, object>();
            SerializeObject<IAction>(entityData);

            base.SerializeComponent(entityData, _gameObjectStateData);
        }

        private void SerializeObject<T>(EntitySerializedData entityData) where T : ISerializableObject
        {
            var actions = GetComponents<T>();
            if (actions == null)
                return;

            foreach (var action in actions)
            {
                action.Serialize(entityData);
                //entitySerializedData.ComponentData.Add(GetName(), data);
            }
        }

        public override void Deserialize(EntitySerializedData entityData)
        {
            base.Deserialize(entityData);
            Deserialize<IAction>(entityData);
        }

        private void Deserialize<T>(EntitySerializedData entityData) where T : ISerializableObject
        {
            var actions = GetComponents<T>();
            if (actions == null)
                return;

            foreach (var action in actions)
            {
                action.Deserialize(entityData);
            }
        }

        public override void DeserializeUniqueIds(EntitySerializedData entitySerializedData)
        {
            base.DeserializeUniqueIds(entitySerializedData);
            DeserializeUniqueIds<IAction>(entitySerializedData);
            //DeserializeUniqueIds<IStateTransitionCondition>(entitySerializedData);
        }

        private void DeserializeUniqueIds<T>(EntitySerializedData entityData) where T : ISerializableObject
        {
            var actions = GetComponents<T>();
            if (actions == null)
                return;

            foreach (var action in actions)
            {
                action.DeserializeUniqueIds(entityData);
            }
        }

        //private void DeserializeActions(EntitySerializedData entityData)
        //{

        //}

        //private void DeserializeConditions(EntitySerializedData entityData)
        //{

        //}


    }
}
