using UnityEngine;
using RQ.Extensions;
using RQ.Enum;
using RQ.Physics.Components;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics;
using RQ.Enums;
using RQ.Animation;
using RQ.Model.ObjectPool;

namespace RQ.Controller.Actions
{
    public enum ShootTarget
    {
        StraightShot = 0,
        ToTarget = 1,
        Random = 2,
        ToLocation = 3,
        ManualVelocity = 4
    }

    [AddComponentMenu("RQ/Action/Shoot Object")]
    public class ShootObject : ActionBase
    {
        [SerializeField]
        private Transform _objectToShoot = null;
        [SerializeField]
        private Vector2D _offset;
        [SerializeField]
        private float _delay = 0f;
        [SerializeField]
        private ShootTarget _shootTarget = ShootTarget.Random;
        [SerializeField]
        private Transform _target = null;
        [SerializeField]
        private bool _lookToTarget = false;
        [SerializeField]
        private Vector2D _value = Vector2D.Zero();
        [SerializeField]
        private float _minSpeed = 0f;
        [SerializeField]
        private float _maxSpeed = 0f;
        [SerializeField]
        private bool _sameLevel = true;
        [SerializeField]
        private LevelLayer _objectLevel = LevelLayer.LevelOne;
        [SerializeField]
        private string _objectPoolName = null;

        private AnimationComponent _animationComponent;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            //object1.layer = LayerMask.NameToLayer("My Layer");
            //var gameObject = otherRigidBody.GetComponent<IComponentRepository>();
            var gameObject = GetEntity();
            for (int i = 0; i < _animationComponents.Count; i++)
            {
                var aniamtionComponent = _animationComponents[i] as AnimationComponent;
                if (aniamtionComponent.IsMain())
                {
                    _animationComponent = aniamtionComponent;
                    break;
                }
            }

            SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId,
                TelegramEarlyTermination.ChangeScenes);

            if (_lookToTarget)
            {
                LookToTarget();
            }
        }

        private void LookToTarget()
        {
            var vectorToTarget = (Vector2D)_target.transform.position - this.transform.position;
            var directionToTarget = vectorToTarget.GetDirection();
            foreach (var animationComponent in _animationComponents)
            {
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, animationComponent.UniqueId,
                    Telegrams.SetFacingDirection, directionToTarget);
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
                        ProcessShoot();
                        //sprite.ProcessAttack();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessShoot()
        {
            var velocity = CalculateVelocity();

            //var angleToRotate = _animationComponents.FirstOrDefault(i => i.IsMain()).GetFacingDirection().GetDirectionAngle();

            //var position = _physicsComponent.GetPos() + RotateAroundAxis(_offset.ToVector3(_physicsComponent.GetPos().z), angleToRotate, new Vector3(0, 0, 1));

            var position = _physicsComponent.GetWorldPos() + _offset.ToVector3(0f);

            //var newObject = ((Transform)GameObject.Instantiate(transform, position, transform.rotation)).GetComponent<IEntityUI>();
            //IComponentRepository newObject = null;
            //if (string.IsNullOrEmpty(_objectPoolName))
            //{
            //    newObject = (GameObject.Instantiate(_objectToShoot, position, transform.rotation) as Transform).GetComponent<IComponentRepository>();
            //}
            //else
            //{
            //    var newGO = ObjectPool.Instance.PullGameObjectFromPool(_objectPoolName, position, Quaternion.identity);
            //    newObject = newGO.GetComponent<IComponentRepository>();
            //    newObject.Reset();
            //}
            var newObject = ObjectPool.InstantiateFromPool(_objectPoolName, _objectToShoot, position, transform.rotation).GetComponent<IComponentRepository>();
            //Transform t;
            //t.
            //var newSprite = newObject.GetEntity() as ISprite;

            if (newObject != null)
            {
                var newPhysicsComponent = newObject.Components.GetComponent<PhysicsComponent>();
                //var newCollisionComponent = newObject.Components.GetComponent<CollisionComponent>();
                var thisLevel = GetLevel();
                if (newPhysicsComponent != null)
                    newPhysicsComponent.SetVelocity(velocity.ToVector3(0));
                // Set the new object to the same level as this one
                //var collisionComponents = newObject.Components.GetComponents<CollisionComponent>();
                //newObject.SendMessageToComponents<CollisionComponent>(0f, this.UniqueId, Telegrams.SetLevelHeight,
                //    thisLevel);
                var otherFloorComponent = newObject.Components.GetComponent<FloorComponent>();
                otherFloorComponent?.SetFloor((int)thisLevel);

                var direction = velocity.GetDirection();
                newObject.SendMessageToComponents<AnimationComponent>(0f, this.UniqueId, Telegrams.SetFacingDirection,
                    direction);
                //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, newObject.UniqueId,
                //    Telegrams.SetFacingDirection, direction);
                //newCollisionComponent.GetLevel
            }
        }

        private LevelLayer GetLevel()
        {
            var floorComponent = GetEntity().Components.GetComponent<FloorComponent>();
            if (_sameLevel)
                return floorComponent.GetLevel();
            else
                return _objectLevel;
        }

        public Vector3 RotateVectorAroundAxis(Vector3 v, float a, Vector3 axis)
        {
            //if (bUseRadians) a *= MathUtil.RAD_TO_DEG;
            var q = Quaternion.AngleAxis(a, axis);
            return q * v;
        }

        private Vector2D CalculateVelocity()
        {
            var speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);

            Vector2D velocity = Vector2D.Zero();

            switch (_shootTarget)
            {
                case ShootTarget.StraightShot:
                    //velocity = _animationComponents.FirstOrDefault(i => i.IsMain()).GetFacingDirectionVector();
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
                    velocity = _value - _physicsComponent.GetWorldPos();
                    break;
                case ShootTarget.ToTarget:
                    velocity = _target.transform.position - _physicsComponent.GetWorldPos();
                    break;
                case ShootTarget.ManualVelocity:
                    velocity = _value;
                    break;
            }

            velocity = Vector2D.Vec2DNormalize(velocity) * speed;

            return velocity;
        }
    }
}
