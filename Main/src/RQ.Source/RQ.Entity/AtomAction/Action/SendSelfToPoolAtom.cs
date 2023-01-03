using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SendSelfToPoolAtom : AtomActionBase
    {
        public string PoolName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //var damageComponent = entity.Components.GetComponent<DamageComponent>();
            //if (damageComponent != null)
            //{
            //    var deathPrefab = damageComponent.GetDeathPrefab();
            //    if (deathPrefab != null)
            //        GameObject.Instantiate(deathPrefab, entity.transform.position, Quaternion.identity);
            //}
            //Debug.Log($"(SendSelfToPoolAtom) Sending {PoolName} to pool");
            //MessageDispatcher2.Instance.DispatchMsg("StopMovement", 0f, entity.UniqueId, entity.UniqueId, null);
            if (!ObjectPool.Instance.IsInPool(PoolName))
            {
                // Not in pool? Just destroy.
                GameObject.Destroy(entity.gameObject);
                return;
            }
            ObjectPool.Instance.ReleaseGameObjectToPool(PoolName, entity.gameObject);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
