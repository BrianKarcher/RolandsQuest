using RQ.Entity.Components;
using RQ.Enum;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Instantiate Prefab")]
    public class InstantiatePrefab : AnimatorState
    {
        [SerializeField]
        private float _delay = 0f;
        [SerializeField]
        private Transform _transform = null;
        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        [SerializeField]
        private bool _sameLevel = true;
        [SerializeField]
        private LevelLayer _objectLevel = LevelLayer.LevelOne;
        public string ObjectPoolName;

        public override void Enter()
        {
            base.Enter();
            //sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
            SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId);
            //SendLocalMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId);
            //MessageDispatcher.Instance.DispatchMsg(_delay, _entity.MessageHandlerID(), _entity.MessageHandlerID(),
            //    );
        }

        private LevelLayer GetLevel()
        {
            if (_collisionComponent == null)
            {
                Debug.LogError(_componentRepository.name + " has no Collision Component");
                return LevelLayer.LevelOne;
            }
            if (_sameLevel)
                return _floorComponent.GetLevel();
            else
                return _objectLevel;
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.ProcessStateEvent:
                    //if (telegram.ExtraInfo.ToString() == UniqueId)
                    //{
                        CreatePrefab();
                        Complete();
                        return true;
                    //}
                    break;
            }
            return false;
        }

        private void CreatePrefab()
        {
            //if (_transform.name.Contains("BombExplosion"))
            //{
            //    //Debug.LogError("Instantiating (BombExplosion)");
            //    int i = 1;
            //}
            //var newObject = (Instantiate(_transform, transform.position + _offset, transform.rotation) as Transform).GetComponent<IComponentRepository>();
            IComponentRepository newObject;
            if (ObjectPool.Instance.IsInPool(ObjectPoolName))
            {
                newObject = ObjectPool
                    .InstantiateFromPool(ObjectPoolName, _transform, transform.position + _offset, transform.rotation)
                    .GetComponent<IComponentRepository>();
            }
            else
            {
                newObject = (Instantiate(_transform, transform.position + _offset, transform.rotation) as Transform).GetComponent<IComponentRepository>();
            }
            //newObject = (Instantiate(_transform, transform.position + _offset, transform.rotation) as Transform).GetComponent<IComponentRepository>();

            //if (_transform.name.Contains("BombExplosion"))
            //{
            //    //Debug.LogError("Instantiated (BombExplosion)");
            //    int i = 1;
            //}
            if (newObject != null)
            {
                var thisLevel = GetLevel();
                var floorComponent = newObject.Components.GetComponent<FloorComponent>();
                floorComponent?.SetFloor((int)thisLevel);
            }
        }
    }
}
