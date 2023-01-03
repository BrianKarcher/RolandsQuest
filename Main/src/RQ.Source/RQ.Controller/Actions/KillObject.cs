using RQ.Entity.Components;
using RQ.Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Kill Object")]
    public class KillObject : ActionBase
    {
        public Transform Transform;
        public List<Transform> GameObjects;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //GameObject.DestroyObject(GameObject);
            if (Transform == null)
            {
                var entity = GetEntity();
                if (entity != null)
                    Transform = entity.transform;
            }
            if (Transform != null)
                Kill(Transform);
            if (GameObjects != null)
            {
                foreach (var go in GameObjects)
                {
                    Kill(go);
                }
            }
        }

        private void Kill(Transform go)
        {
            if (go == null)
                return;
            var repo = go.GetComponent<IComponentRepository>();
            if (repo == null)
            {
                Debug.LogError(this.name + " Could not locate objet to kill");
                return;
            }
            MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, this.UniqueId, repo.UniqueId, null);
        }

        //public override void Serialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var killObjectData = new KillObjectData();
        //    if (Transform != null)
        //    {
        //        var repo = Transform.GetComponent<IComponentRepository>();
        //        killObjectData.KillObjectUniqueId = repo.UniqueId;
        //    }
        //    if (GameObjects != null)
        //    {
        //        killObjectData.KillObjectUniqueIds = new List<string>();
        //        foreach (var go in GameObjects)
        //        {
        //            if (go == null)
        //                continue;
        //            var repo = go.GetComponent<IComponentRepository>();
        //            killObjectData.KillObjectUniqueIds.Add(repo.UniqueId);
        //        }
        //    }
        //    base.SerializeComponent(entitySerializedData, killObjectData);
        //}

        //public override void Deserialize(EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var killObjectData = base.DeserializeComponent<KillObjectData>(entitySerializedData);
        //    if (killObjectData == null)
        //        return;
        //    if (!string.IsNullOrEmpty(killObjectData.KillObjectUniqueId))
        //    {
        //        Transform = EntityContainer._instance.GetEntity(killObjectData.KillObjectUniqueId).transform;
        //    }

        //    if (killObjectData.KillObjectUniqueIds != null)
        //    {
        //        GameObjects = new List<Transform>();
        //        foreach (var uniqueid in killObjectData.KillObjectUniqueIds)
        //        {
        //            var newGO = EntityContainer._instance.GetEntity(uniqueid).transform;
        //            GameObjects.Add(newGO);
        //        }                
        //    }
        //}
    }
}
