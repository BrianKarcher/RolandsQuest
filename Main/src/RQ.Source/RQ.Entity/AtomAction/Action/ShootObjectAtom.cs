using RQ.Animation;
using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Enum;
using RQ.Enums;
using RQ.Extensions;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics;
using RQ.Physics.Components;
using System.Collections;
using RQ.Model.ObjectPool;
using UnityEngine;

namespace RQ.AI.Action
{
    public enum ShootTarget
    {
        StraightShot = 0,
        ToTarget = 1,
        Random = 2,
        ToLocation = 3
    }

    public class ShootObjectAtom : AtomActionBase
    {
        [SerializeField]
        [UniqueIdentifier]
        public string _uniqueId;
        [SerializeField]
        public Transform _objectToShoot = null;
        [SerializeField]
        public Vector2 _offset;
        [SerializeField]
        public float _delay = 0f;
        [SerializeField]
        public ShootTarget _shootTarget = ShootTarget.Random;
        [SerializeField]
        public Vector2 _location;
        [SerializeField]
        [HideInInspector]
        public float _minSpeed = 0f;
        [SerializeField]
        [HideInInspector]
        public float _maxSpeed = 0f;
        [SerializeField]
        public bool _sameLevel = true;
        [SerializeField]
        public LevelLayer _objectLevel = LevelLayer.LevelOne;
        public string ObjectPoolName;

        private AnimationComponent _animationComponent;
        private PhysicsComponent _physicsComponent;
        private AIComponent _aIComponent;
        private CollisionComponent _collisionComponent;
        private FloorComponent _floorComponent;
        private long _shoootObjectMessageIndex;
        private IComponentRepository _entity;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity;

            _animationComponent = entity.Components.GetComponent<AnimationComponent>();
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _aIComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _floorComponent = entity.Components.GetComponent<FloorComponent>();
        }

        public IEnumerator ShootObject()
        {
            yield return new WaitForSeconds(_delay);
            ProcessShoot(_entity);
            _isRunning = false;
        }

        private void ProcessShoot(IComponentRepository entity)
        {
            var velocity = CalculateVelocity();

            var angleToRotate = _animationComponent.GetFacingDirection().GetDirectionAngle();

            var position = _physicsComponent.GetWorldPos() + RotateAroundAxis(new Vector3(_offset.x, _offset.y, 0f), angleToRotate, new Vector3(0, 0, 1));

            //var newObject = (GameObject.Instantiate(_objectToShoot, position, entity.transform.rotation) as Transform).GetComponent<IComponentRepository>();
            //IComponentRepository newObject = null;
            //if (string.IsNullOrEmpty(ObjectPoolName))
            //{
            //    newObject = (GameObject.Instantiate(_objectToShoot, position, entity.transform.rotation) as Transform).GetComponent<IComponentRepository>();
            //}
            //else
            //{
            //    var newGO = ObjectPool.Instance.PullGameObjectFromPool(ObjectPoolName, position, Quaternion.identity);
            //    newObject = newGO.GetComponent<IComponentRepository>();
            //    newObject.Reset();
            //}
            Transform newObject = null;
            if (!ObjectPool.Instance.IsInPool(ObjectPoolName))
            {
                newObject = GameObject.Instantiate(_objectToShoot, position, entity.transform.rotation) as Transform;
            }
            else
            {
                newObject = ObjectPool.InstantiateFromPool(ObjectPoolName, _objectToShoot, position, entity.transform.rotation);
            }
            IComponentRepository repo = null;
            if (newObject != null)
                repo = newObject.GetComponent<IComponentRepository>();
            if (repo != null)
            {
                var newPhysicsComponent = repo.Components.GetComponent<PhysicsComponent>();
                var thisLevel = GetLevel();
                newPhysicsComponent?.SetVelocity(velocity);
                //newPhysicsComponent.GetPhysicsAffector("Steering").Velocity = velocity.ToVector3(0);
                //newPhysicsComponent.SetVelocity(velocity.ToVector3(0));
                // Set the new object to the same level as this one
                //newObject.SendMessageToComponents<CollisionComponent>(0f, this._uniqueId, Telegrams.SetLevelHeight,
                //    thisLevel);
                var otherFloorComponent = repo.Components.GetComponent<FloorComponent>();
                otherFloorComponent?.SetFloor((int)thisLevel);
                var direction = velocity.GetDirection();
                repo.SendMessageToComponents<AnimationComponent>(0f, this._uniqueId, Telegrams.SetFacingDirection,
                    direction);
            }
            _isRunning = false;
        }

        private LevelLayer GetLevel()
        {
            if (_sameLevel)
                return _floorComponent.GetLevel();
            else
                return _objectLevel;
        }

        public Vector3 RotateAroundAxis(Vector3 v, float a, Vector3 axis)
        {
            var q = Quaternion.AngleAxis(a, axis);
            return q * v;
        }

        private Vector2 CalculateVelocity()
        {
            var speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);

            Vector2D velocity = Vector2D.Zero();

            switch (_shootTarget)
            {
                case ShootTarget.StraightShot:
                    velocity = _animationComponent.GetFacingDirectionVector();
                    break;
                case ShootTarget.Random:
                    //targetVector = new Vector2D(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                    var angle = UnityEngine.Random.Range(0f, 360f);
                    var x = Mathf.Cos(angle);
                    var y = Mathf.Sin(angle);
                    velocity = new Vector2D(x, y);
                    break;
                case ShootTarget.ToLocation:
                    velocity = _location - (Vector2)_physicsComponent.GetWorldPos();
                    break;
                case ShootTarget.ToTarget:
                    velocity = _aIComponent.Target.position - _physicsComponent.GetWorldPos();
                    break;
            }

            velocity = Vector2D.Vec2DNormalize(velocity) * speed;

            return velocity;
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
