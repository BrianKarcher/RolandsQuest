using RQ.Animation;
using RQ.Common.UniqueId;
using RQ.Entity.Components;
using RQ.Enum;
using RQ.Enums;
using RQ.Extensions;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.AI.Action
{
    public enum SetVelocityTargetEnum
    {
        StraightShot = 0,
        ToTarget = 1,
        Random = 2,
        ToLocation = 3,
        AwayFromTarget = 4,
        ToDamageDealer = 5,
        AwayFromSteeringVector = 6,
        ManualVelocity = 7
    }

    //[AddComponentMenu("RQ/States/State/Shoot Object")]
    public class SetVelocityAtom : IAtomAction
    {
        private Vector2 _manualVelocity;
        [SerializeField]
        public Vector2 _offset;
        [SerializeField]
        public float _delay = 0f;
        [SerializeField]
        public string _physicsComponentName;
        //[SerializeField]
        //private bool _stopMoving = true;
        [SerializeField]
        public SetVelocityTargetEnum _shootTarget = SetVelocityTargetEnum.Random;
        //[SerializeField]
        //private Transform _target = null;
        //[SerializeField]
        //private bool _lookToTarget = false;
        [SerializeField]
        public Vector2 _location;
        [SerializeField]
        public float _minSpeed = 0f;
        [SerializeField]
        public float _maxSpeed = 0f;
        [SerializeField]
        public bool _sameLevel = true;
        [SerializeField]
        public LevelLayer _objectLevel = LevelLayer.LevelOne;
        public string _physicsAffector;

        private AnimationComponent _animationComponent;
        private PhysicsComponent _physicsComponent;
        private AIComponent _aIComponent;
        private CollisionComponent _collisionComponent;
        private FloorComponent _floorComponent;
        private long _shoootObjectMessageIndex;
        private bool _isRunning;
        private IComponentRepository _entity;
        private Vector2 _currentVelocity;
        //private IPhysicsAffector _steeringAffector;

        public void Start(IComponentRepository entity)
        {
            _entity = entity;
            //base.Enter();
            //if (_stopMoving)
            //    MessageDispatcher.Instance.DispatchMsg(0f, this._uniqueId, _physicsComponent.UniqueId,
            //        Telegrams.StopMovement, null);
            //_physicsComponent.Stop();

            _animationComponent = entity.Components.GetComponent<AnimationComponent>();
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);
            _aIComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _floorComponent = entity.Components.GetComponent<FloorComponent>();

            //ProcessShoot(entity);
            //SendMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId,
            //    TelegramEarlyTermination.ChangeScenes);
            //SendLocalMessageToSelf(_delay, Enums.Telegrams.ProcessStateEvent, UniqueId, Enums.TelegramEarlyTermination.ChangeScenes);
            //MessageDispatcher.Instance.DispatchMsg(_delay, _sprite.MessageHandlerID(), _sprite.MessageHandlerID(),
            //    Enums.Telegrams.ProcessStateEvent, ID, Enums.TelegramEarlyTermination.ChangeScenes);            
            //StartListening(entity);
            //MessageDispatcher2.Instance.DispatchMsg("ShootObject", _delay, this._uniqueId, entity.UniqueId, this._uniqueId);

            //_isRunning = false;
            _currentVelocity = CalculateVelocity();
            //_steeringAffector = _physicsComponent.GetPhysicsAffector(_physicsAffector);
        }

        //protected void LookToTarget()
        //{
        //    // Look to the target, if necessary, before starting the animation
        //    if (_lookToTarget)
        //    {
        //        var vectorToTarget = (Vector2D)_aiComponent.Target.position - this.transform.position;
        //        var directionToTarget = vectorToTarget.GetDirection();
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _animationComponent.UniqueId,
        //            Telegrams.SetFacingDirection, directionToTarget);
        //    }
        //}

        public void End()
        {
            //StopListening(_entity);
            //MessageDispatcher.Instance.RemoveFromQueue(i => i.ReceiverId == this.UniqueId && 
            //    i.Msg == Telegrams.ProcessStateEvent);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }

        //private void ProcessShoot(IComponentRepository entity)
        //{
        //    var velocity = CalculateVelocity();
        //    //Debug.LogWarningFormat("Shooting object at {0}", velocity.ToString());

        //    var angleToRotate = _animationComponent.GetFacingDirection().GetDirectionAngle();

        //    var position = _physicsComponent.GetPos() + RotateAroundAxis(_offset.ToVector3(_physicsComponent.GetPos().z), angleToRotate, new Vector3(0, 0, 1));

        //    //var newObject = ((Transform)GameObject.Instantiate(transform, position, transform.rotation)).GetComponent<IEntityUI>();

        //    var newObject = (GameObject.Instantiate(_objectToShoot, position, entity.transform.rotation) as Transform).GetComponent<IComponentRepository>();
        //    //Transform t;
        //    //t.
        //    //var newSprite = newObject.GetEntity() as ISprite;

        //    if (newObject != null)
        //    {
        //        var newPhysicsComponent = newObject.Components.GetComponent<PhysicsComponent>();
        //        //var newCollisionComponent = newObject.Components.GetComponent<CollisionComponent>();
        //        var thisLevel = GetLevel();
        //        newPhysicsComponent.SetVelocity(velocity.ToVector3(0));
        //        // Set the new object to the same level as this one
        //        newObject.SendMessageToComponents<CollisionComponent>(0f, this._uniqueId, Telegrams.SetLevelHeight,
        //            thisLevel);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, newObject.UniqueId,
        //        //    Telegrams.SetLevelHeight, thisLevel);
        //        newObject.SendMessageToComponents<AnimationComponent>(0f, this._uniqueId, Telegrams.VelocityChanged,
        //            velocity);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, newObject.UniqueId,
        //        //    Telegrams.VelocityChanged, velocity);
        //        var direction = velocity.GetDirection();
        //        newObject.SendMessageToComponents<AnimationComponent>(0f, this._uniqueId, Telegrams.SetFacingDirection,
        //            direction);
        //        //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, newObject.UniqueId,
        //        //    Telegrams.SetFacingDirection, direction);
        //        //newCollisionComponent.GetLevel
        //    }
        //}

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
                case SetVelocityTargetEnum.StraightShot:
                    velocity = _animationComponent.GetFacingDirectionVector();
                    break;
                case SetVelocityTargetEnum.Random:
                    //targetVector = new Vector2D(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                    var angle = UnityEngine.Random.Range(0f, 360f);
                    var x = Mathf.Cos(angle);
                    var y = Mathf.Sin(angle);
                    velocity = new Vector2D(x, y);
                    break;
                case SetVelocityTargetEnum.ToLocation:
                    velocity = _location - (Vector2)_physicsComponent.GetWorldPos();
                    break;
                case SetVelocityTargetEnum.ToTarget:
                    velocity = _aIComponent.Target.position - _physicsComponent.GetWorldPos();
                    break;
                case SetVelocityTargetEnum.AwayFromSteeringVector:
                    velocity = _physicsComponent.GetWorldPos() - _physicsComponent.GetSteering().GetTarget3();
                    break;
                case SetVelocityTargetEnum.ToDamageDealer:
                    var damageComponent = _entity.Components.GetComponent<DamageComponent>();
                    var damageInfo = damageComponent.GetDamageInfo();
                    velocity = damageInfo.DamagedByEntity.transform.position - _entity.transform.position;
                    break;
                case SetVelocityTargetEnum.ManualVelocity:
                    return _manualVelocity;
                case SetVelocityTargetEnum.AwayFromTarget:
                    velocity = _physicsComponent.GetWorldPos() - _aIComponent.Target.position;
                    break;
            }

            velocity = Vector2D.Vec2DNormalize(velocity) * speed;

            return velocity;
        }

        public AtomActionResults OnUpdate()
        {
            _physicsComponent.SetVelocity(_currentVelocity);
            //_physicsComponent.SetVelocity(_currentVelocity.ToVector3(0f));
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _manualVelocity = velocity;
        }
    }
}
