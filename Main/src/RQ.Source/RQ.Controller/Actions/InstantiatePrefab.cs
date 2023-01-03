using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Instantiate Prefab")]
    public class InstantiatePrefab : ActionBase
    {
        [SerializeField]
        private float _delay = 0f;
        public GameObject GameObject;
        public GameObject SpawnPoint;
        public Vector3 Position;
        public Quaternion Rotation;
        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        [SerializeField]
        public PrefabLocation _location;
        //[SerializeField]
        //private Transform _usableTokenPosition;
        [SerializeField]
        private bool _setParent = false;
        [SerializeField]
        private bool _killOnExit = false;
        private GameObject _instantiatedGO = null;
        public string ObjectPoolName;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (GameObject != null)
            {
                SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId);
            }
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            if (_killOnExit)
                GameObject.Destroy(_instantiatedGO);
        }

        public Vector3 GetLocation()
        {
            switch (_location)
            {
                case PrefabLocation.SpecificPosition:
                    return Position;
                case PrefabLocation.Object:
                    return GetEntity().transform.position;
                case PrefabLocation.CurrentDestinationLocation:
                    var entity = GetEntity();
                    var aiComponent = entity.Components.GetComponent<AIComponent>();
                    return aiComponent.CurrentDestinationLocation.transform.position;
                case PrefabLocation.Spawnpoint:
                    return SpawnPoint.transform.position;
                //case PrefabLocation.Transform:
                //    return _usableTokenPosition.position;
                default:
                    return Vector3.zero;
            }
        }

        public override bool HandleMessage(Telegram telegram)
        {
            if (base.HandleMessage(telegram))
                return true;

            switch (telegram.Msg)
            {
                case Enums.Telegrams.ProcessStateEvent:
                    if (telegram.ExtraInfo.ToString() == UniqueId)
                    {
                        Vector3 position;
                        //if (_setParent)
                        //{

                        //}
                        //else
                        //{
                            position = GetLocation() + _offset;
                        //}
                        //Vector3 position = SpawnPoint == null ? Position : SpawnPoint.transform.position;
                        //_instantiatedGO = Instantiate(GameObject, position, Rotation) as GameObject;
                        //IComponentRepository newObject = null;
                        _instantiatedGO = ObjectPool.InstantiateFromPool(ObjectPoolName, GameObject, position, Rotation);
                        //if (string.IsNullOrEmpty(ObjectPoolName))
                        //{
                        //    _instantiatedGO = Instantiate(GameObject, position, Rotation) as GameObject;
                        //}
                        //else
                        //{
                        //    _instantiatedGO = ObjectPool.Instance.PullGameObjectFromPool(ObjectPoolName, position, Quaternion.identity);
                        //    var repo = _instantiatedGO.GetComponent<IComponentRepository>();
                        //    repo.Reset();
                        //}

                        if (_setParent)
                        {
                            if (SpawnPoint != null)
                                _instantiatedGO.transform.parent = SpawnPoint.transform;
                            else
                            {
                                _instantiatedGO.transform.parent = GetEntity().transform;
                            }
                        }
                        //Instantiate(_transform, _physicsComponent.GetPos() + _offset, transform.rotation);
                        //StateMachine.GetStateInfo().IsComplete = true;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public enum PrefabLocation
        {
            SpecificPosition = 0,
            Object = 1,
            CurrentDestinationLocation = 2,
            Spawnpoint = 3
            //Transform
        }
    }
}
