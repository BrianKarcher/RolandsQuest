using RQ.Animation;
using RQ.Entity.Components;
using RQ.Enum;
using RQ.Enums;
using RQ.Extensions;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    public enum ShootTarget
    {
        StraightShot = 0,
        ToTarget = 1,
        Random = 2,
        ToLocation = 3
    }

    [AddComponentMenu("RQ/States/State/Shoot Object")]
    public class ShootObject : AnimatorState
    {
        [SerializeField]
        private Transform _objectToShoot = null;
        [SerializeField]
        private Vector2D _offset;
        [SerializeField]
        private float _delay = 0f;
        [SerializeField]
        private bool _stopMoving = true;
        [SerializeField]
        private ShootTarget _shootTarget = ShootTarget.Random;
        //[SerializeField]
        //private Transform _target = null;
        [SerializeField]
        private bool _lookToTarget = false;
        [SerializeField]
        private Vector2D _location = Vector2D.Zero();
        [SerializeField]
        private float _minSpeed = 0f;
        [SerializeField]
        private float _maxSpeed = 0f;
        [SerializeField]
        private bool _sameLevel = true;
        [SerializeField]
        private LevelLayer _objectLevel = LevelLayer.LevelOne;
        public string ObjectPoolName;

        public override void Enter()
        {
            base.Enter();
            if (_stopMoving)
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Telegrams.StopMovement, null);
                //_physicsComponent.Stop();

            SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId,
                TelegramEarlyTermination.ChangeScenes);
            //SendLocalMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId, Enums.TelegramEarlyTermination.ChangeScenes);
            //MessageDispatcher.Instance.DispatchMsg(_delay, _sprite.MessageHandlerID(), _sprite.MessageHandlerID(),
            //    Enums.Telegrams.ProcessStateEvent, ID, Enums.TelegramEarlyTermination.ChangeScenes);
        }

        protected override void StartAnimation()
        {
            // Look to the target, if necessary, before starting the animation
            if (_lookToTarget)
            {
                var vectorToTarget = (Vector2D)_aiComponent.Target.position - this.transform.position;
                var directionToTarget = vectorToTarget.GetDirection();
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _animationComponent.UniqueId,
                    Telegrams.SetFacingDirection, directionToTarget);
            }
            base.StartAnimation();
        }

        public override void Exit()
        {
            base.Exit();
            //MessageDispatcher.Instance.RemoveFromQueue(i => i.ReceiverId == this.UniqueId && 
            //    i.Msg == Telegrams.ProcessStateEvent);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
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
            var angleToRotate = _animationComponent.GetFacingDirection().GetDirectionAngle();
            var position = _physicsComponent.GetWorldPos() + RotateAroundAxis(_offset.ToVector3(_physicsComponent.GetWorldPos().z), angleToRotate, new Vector3(0, 0, 1));
            //var newObject = (GameObject.Instantiate(_objectToShoot, position, transform.rotation) as Transform).GetComponent<IComponentRepository>();
            //IComponentRepository newObject = null;
            //if (string.IsNullOrEmpty(ObjectPoolName))
            //{
            //    newObject = (GameObject.Instantiate(_objectToShoot, position, transform.rotation) as Transform).GetComponent<IComponentRepository>();
            //}
            //else
            //{
            //    var newGO = ObjectPool.Instance.PullGameObjectFromPool(ObjectPoolName, position, Quaternion.identity);
            //    newObject = newGO.GetComponent<IComponentRepository>();
            //    newObject.Reset();
            //}
            var newObject = ObjectPool.InstantiateFromPool(ObjectPoolName, _objectToShoot, position, transform.rotation).GetComponent<IComponentRepository>();


            if (newObject != null)
            {
                var newPhysicsComponent = newObject.Components.GetComponent<PhysicsComponent>();
                //var newCollisionComponent = newObject.Components.GetComponent<CollisionComponent>();
                var thisLevel = GetLevel();
                newPhysicsComponent.SetVelocity(velocity.ToVector3(0));
                // Set the new object to the same level as this one
                //newObject.SendMessageToComponents<CollisionComponent>(0f, this.UniqueId, Telegrams.SetLevelHeight,
                //    thisLevel);
                var otherFloorComponent = newObject.Components.GetComponent<FloorComponent>();
                otherFloorComponent?.SetFloor((int)thisLevel);

                var direction = velocity.GetDirection();
                newObject.SendMessageToComponents<AnimationComponent>(0f, this.UniqueId, Telegrams.SetFacingDirection,
                    direction);
            }
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
                    velocity = _location - _physicsComponent.GetWorldPos();
                    break;
                case ShootTarget.ToTarget:
                    velocity = _aiComponent.Target.position - _physicsComponent.GetWorldPos();
                    break;
            }

            velocity = Vector2D.Vec2DNormalize(velocity) * speed;

            return velocity;
        }
    }
}
