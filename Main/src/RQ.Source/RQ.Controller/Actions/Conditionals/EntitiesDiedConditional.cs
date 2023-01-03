using RQ.Entity.Common;
using UnityEngine;
using System.Collections.Generic;
using RQ.Common.Container;
using RQ.Physics.Components;
using RQ.Messaging;
using RQ.Serialization;
using RQ.Model.Serialization;

namespace RQ.Controller.Actions.Conditionals
{
    [AddComponentMenu("RQ/Action/Conditional/Entities Died")]
    public class EntitiesDiedConditional : ConditionalBase
    {
        [SerializeField]
        private List<SpriteBaseComponent> _entities;
        //private List<string> _entitiesAlive;

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            foreach (var entity in _entities)
            {
                //var damageComponent = entity.Components.GetComponent<DamageComponent>();
                //if (damageComponent == null)
                //    continue;
                MessageDispatcher2.Instance.DispatchMsg("AddEntityDeathNotification", 0f,
                    this.UniqueId, entity.UniqueId, this.UniqueId);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var entity in _entities)
            {
                var damageComponent = entity.Components.GetComponent<DamageComponent>();
                if (damageComponent == null)
                    continue;
                MessageDispatcher2.Instance.DispatchMsg("RemoveEntityDeathNotification", 0f,
                    this.UniqueId, damageComponent.UniqueId, this.UniqueId);
            }
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("EntityDied", this.UniqueId, (data) =>
            {
                Debug.Log("Entity Died");
                var entityUniqueId = (string)data.ExtraInfo;
                _entities.RemoveAll(i => i.UniqueId == entityUniqueId);
            });

        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("EntityDied", this.UniqueId, -1);
        }

        public override bool Check()
        {
            if (_entities == null || _entities.Count == 0)
            {
                Debug.Log("All Entities died");
                return true;
            }

            return false;
        }

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var entitiesDiedData = new EntitiesDiedData();
        //    entitiesDiedData.EntitiesUniqueIds = new List<string>();
        //    foreach (var entity in _entities)
        //    {
        //        entitiesDiedData.EntitiesUniqueIds.Add(entity.UniqueId);
        //    }
        //    base.SerializeComponent(entitySerializedData, entitiesDiedData);
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    if (_entities == null)
        //        _entities = new List<SpriteBaseComponent>();
        //    _entities.Clear();

        //    var data = base.DeserializeComponent<EntitiesDiedData>(entitySerializedData);

        //    if (data == null)
        //        return;
        //    foreach (var uniqueId in data.EntitiesUniqueIds)
        //    {
        //        var entity =EntityContainer._instance.GetEntity<SpriteBaseComponent>(uniqueId);
        //        _entities.Add(entity);
        //    }

        //    //var entities = _entities.Where(i => i != null);
        //    //_entities = entities == null ? null : entities.ToList();
        //    //_entities = _entities.Where(i => EntityContainer._instance.GetEntity(i.UniqueId))
        //}
    }
}
