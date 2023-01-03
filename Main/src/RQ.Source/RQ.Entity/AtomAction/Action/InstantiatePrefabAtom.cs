using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using System;
using RQ.Enum;
using RQ.FSM.V2;
using RQ.Model.ObjectPool;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class InstantiatePrefabAtom : AtomActionBase
    {
        public GameObject _gameObject;
        public Vector3 _offset = Vector3.zero;
        public bool _rotateOffsetByFacingDirection = false;
        public bool _rotateHorizontalOffsetByFacingDirection = false;
        public bool _rotateNewObjectByFacingDirection = false;
        //[SerializeField]
        //private float _delay = 0f;
        //[SerializeField]
        //private bool _killWhenAnimationCompletes = false;
        //public string AnimationType;
        private AnimationComponent _animComponent;
        private FloorComponent _floorComponent;
        private GameObject _newPrefab;

        public float Delay = 0.0f;

        public bool SetAIParent = false;
        //private IComponentRepository _entity;
        //private bool _isRunning;
        //private long _killSelfIndex;
        public string ObjectPoolName;

        public bool OnlyIfNoChild = false;

        private long _instantiatePrefabId;

        private Action<Telegram2> _actDelegateAction = null;
        private float _startTime;

        public override void Start(IComponentRepository entity)
        {
            // Need to cache delegate before calling Start - Start calls StartListening.
            // Cache the delegate
            if (_actDelegateAction == null)
                _actDelegateAction = Act;
            base.Start(entity);
            _animComponent = _entity.Components.GetComponent<AnimationComponent>();
            _floorComponent = entity.Components.GetComponent<FloorComponent>();
            if (Delay == 0f)
            {
                Act(new Telegram2());
                _isRunning = false;
            }            
            _startTime = Time.time;
            //MessageDispatcher2.Instance.DispatchMsg("InstantiatePrefab", Delay, entity.UniqueId, entity.UniqueId, null);
        }

        public override void StartListening(IComponentRepository entity)
        {
            base.StartListening(entity);
            _instantiatePrefabId =
                MessageDispatcher2.Instance.StartListening("InstantiatePrefab", entity.UniqueId, _actDelegateAction);
        }

        public override void StopListening(IComponentRepository entity)
        {
            base.StopListening(entity);
            MessageDispatcher2.Instance.StopListening("InstantiatePrefab", entity.UniqueId, _instantiatePrefabId);
        }

        public GameObject GetPrefab()
        {
            return _newPrefab;
        }

        private void Act(Telegram2 telegram2)
        {
            //GameObject.Instantiate(_gameObject, _componentRepository.transform.position, Quaternion.identity)
            //entity.Instantiate(_gameObject, entity.transform.position, Quaternion.identity);
            //_animComponent = _entity.Components.GetComponent<AnimationComponent>();
            Direction facingDirection = Direction.None;
            if (_animComponent != null)
                facingDirection = _animComponent.GetFacingDirection();
            Vector3 newOffset = _offset;
            if (OnlyIfNoChild)
            {
                var aiComponent = _entity.Components.GetComponent<AIComponent>();
                if (aiComponent != null)
                {
                    if (aiComponent.Children.Count != 0)
                        return;
                }
            }
            if (_rotateOffsetByFacingDirection && _animComponent != null)
            {
                var facingDirectionAngle = facingDirection.GetDirectionAngle();
                var rotationQuat = Quaternion.AngleAxis(facingDirectionAngle, Vector3.forward);
                newOffset = rotationQuat * newOffset;
            }
            if (_rotateHorizontalOffsetByFacingDirection)
            {
                if (facingDirection == Direction.Left)
                {
                    // Merely flip the X value if need to flip to the left
                    newOffset = new Vector3(-newOffset.x, newOffset.y, newOffset.z);
                    //var facingDirectionAngle = facingDirection.GetDirectionAngle();
                    //var rotationQuat = Quaternion.AngleAxis(90f, Vector3.forward);
                    //newOffset = rotationQuat * newOffset;
                }
            }
            Quaternion rotation;
            if (_rotateNewObjectByFacingDirection)
            {
                var facingDirectionAngle = facingDirection.GetDirectionAngle();
                rotation = Quaternion.AngleAxis(facingDirectionAngle, Vector3.forward);
            }
            else
            {
                rotation = Quaternion.identity;
            }
            //_newPrefab = GameObject.Instantiate(_gameObject, entity.transform.position + newOffset, Quaternion.identity) as GameObject;
            _newPrefab = ObjectPool.InstantiateFromPool(ObjectPoolName, _gameObject, _entity.transform.position + newOffset, rotation);
            var newRepo = _newPrefab.GetComponent<IComponentRepository>();
            var thisLevel = GetLevel();
            var otherFloorComponent = newRepo.Components.GetComponent<FloorComponent>();
            otherFloorComponent?.SetFloor((int)thisLevel);
            if (SetAIParent)
            {
                var otherAIComponent = newRepo.Components.GetComponent<AIComponent>();
                otherAIComponent.Parent = _entity.transform;
            }

            if (_animComponent != null)
            {
                Debug.Log("InstantiatePrefab setting facing direction to " + facingDirection);
                MessageDispatcher2.Instance.DispatchMsg("SetFacingDirection", 0f, _entity.UniqueId, newRepo.UniqueId,
                    facingDirection);
            }
            //MessageDispatcher.Instance.DispatchMsg(0f, entity.UniqueId, newRepo.UniqueId,
            //    Telegrams.SetFacingDirection, facingDirection);

            _isRunning = false;
        }

        private LevelLayer GetLevel()
        {
            //if (_sameLevel)
                return _floorComponent == null ? LevelLayer.LevelOne : _floorComponent.GetLevel();
            //else
            //    return _objectLevel;
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _killSelfIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
        //    {
        //        //var animation = _animComponent.Get
        //        if ((string)data.ExtraInfo != AnimationType)
        //            return;
        //        _isRunning = false;
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        //}

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {
            float currentTime = Time.time;
            float timeLapsed = currentTime - _startTime;
            if (timeLapsed > Delay)
            {
                Act(new Telegram2());
                _isRunning = false;
            }
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetGameObject(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
    }
}
