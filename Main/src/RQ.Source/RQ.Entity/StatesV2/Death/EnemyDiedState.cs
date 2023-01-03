using RQ.Messaging;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Death/Enemy Died")]
    public class EnemyDiedState : DiedState
    {
        [SerializeField]
        private GameObject _enemyDeath;

        public string ObjectPoolName;

        public override void Enter()
        {
            base.Enter();
            MessageDispatcher2.Instance.DispatchMsg("Kill", 0f, this.UniqueId,
                _componentRepository.UniqueId, null);
            //GameObject.Instantiate(_enemyDeath, _componentRepository.transform.position, Quaternion.identity);
            ObjectPool.InstantiateFromPool(ObjectPoolName, _enemyDeath, _componentRepository.transform.position, Quaternion.identity);
            Complete();
        }
    }
}
