using RQ.Entity.Components;
using RQ.Entity.Data;
using RQ.Enum;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.Pathfinding;
using System;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Smart Follow")]
    public class SmartFollow : AnimatorState
    {
        public enum FollowTarget
        {
            Target = 0,
            Ranodm = 1,
            RandomLocation = 2,
            RandomSubLocation = 3,
            CurrentDestinationLocation = 4,
            Follow = 5,
            Home = 6
        }

        [SerializeField]
        private FollowTarget _followTarget = FollowTarget.Follow;
        private SmartFollowData Data { get; set; }
        //[SerializeField]
        //private Transform _objectToShoot;
        [SerializeField]
        private Vector2D _offset = Vector2D.Zero();
        //[SerializeField]
        //private float _delay;
        //[SerializeField]
        //private bool _stopMoving;
        //[SerializeField]
        //private ShootTarget _shootTarget;
        //[SerializeField]
        //private Transform _target;
        //[SerializeField]
        //private Vector2D _location;
        //[SerializeField]
        //private float _minSpeed;
        //[SerializeField]
        //private float _maxSpeed;

        // This helps to prevent sprite jitter by not letting them follow a path that is too short
        [SerializeField]
        private float _minDistanceForPath = .5f;
        [SerializeField]
        private float _refreshTimer = 1f;

        private RQ.Physics.SteeringBehaviors.FollowPath _behavior;
        
        //private Seeker _seeker;
        /** Current path which is followed */
        //protected Path path;
        protected Vector2D[] path;
        //private ISprite _sprite;
        //private Transform _followTransform;
        //private PhysicsComponent _followPhysicsComponent;
        //private CollisionComponent _followCollisionComponent;
        private bool _calculatingPath = false;

        private float _nextStuckCheck;
        private Vector2D _lastCheckedPosition;
        //private int _graphMask;
        //private AIComponent _aiComponent;
        //public override void Awake()
        //{
        //    base.Awake();
        //}

        public override void SetupState()
        {
            base.SetupState();
            if (!Application.isPlaying)
                return;
            //_sprite = EntityUIBase.GetEntity(entity);
            if (_physicsComponent == null)
                throw new Exception("FSM - Sprite not set.");
            _behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
            //_aiComponent = _spriteBase.Components.GetComponent<AIComponent>();
            //_seeker = _aiComponent.GetSeeker();
            
            Data = new SmartFollowData();
            Data.Path = null;

            _nextStuckCheck = 0f;

            _behavior.WaypointChanged = force =>
            {
                if (_behavior.Path.IsFinished)
                    StateMachine.GetStateInfo().IsComplete = true;
            };
        }

        //private const string StateName = "ShootObject";

        public override void Enter()
        {
            base.Enter();

            Data.Path = null;
            //_calculatingPath = false;
            


            //_seeker.
            //We should search from the current position
            //Enables the first and third graphs to be included, but not the rest
            // Graph 1
            //int graphMask = (1 << 0);// | (1 << 2);
            //int graphMask = (1 << 0) | (1 << 1);
            // Graph 2
            //int graphMask = (1 << 1);
            // Graph 3
            StateMachine.GetStateInfo().IsStuck = false;
            //var followEntity = EntityUIBase.GetEntity(_followTransform.gameObject);

            _aiComponent.OnPathComplete += OnPathComplete;
            //_seeker.pathCallback += OnPathComplete;

            
            //if (_path == null)
            //    _path = new List<Vector2D>();

            InitiateFollow();
            //if (_stopMoving)
            //    _sprite.Stop();
            //MessageDispatcher.Instance.DispatchMsg(_delay, _sprite.ID(), _sprite.ID(),
            //    Enums.Telegrams.ProcessStateEvent, ID, Enums.TelegramEarlyTermination.ChangeScenes);
            if (_refreshTimer != 0f)
            {
                MessageDispatcher2.Instance.DispatchMsg("Refresh", _refreshTimer, this.UniqueId, this.UniqueId, null);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _behavior.TurnOff();
            //_steering.GetSteeringBehavior(behavior_type.wall_avoidance).TurnOff();
            //Release the previous path
            if (path != null)
            {
                //path.Release(this);
                path = null;
            }
            //_seeker.
            //Make sure we receive callbacks when paths complete
            _aiComponent.OnPathComplete -= OnPathComplete;
            //_seeker.pathCallback -= OnPathComplete;
            StateMachine.GetStateInfo().IsStuck = false;
            MessageDispatcher2.Instance.RemoveMessages("Refresh", this.UniqueId);
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }

        public override void StartListening()
        {
            base.StartListening();
            MessageDispatcher2.Instance.StartListening("Refresh", this.UniqueId, (data) =>
                {
                    InitiateFollow();
                    if (_refreshTimer != 0f)
                    {
                        MessageDispatcher2.Instance.DispatchMsg("Refresh", _refreshTimer, 
                            this.UniqueId, this.UniqueId, null);
                    }
                });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("Refresh", this.UniqueId, -1);
        }

        private void InitiateFollow()
        {
            //if (_calculatingPath)
            //    return;

            //var followPhysicsComponent = _followPhysicsComponent;
            //if (path != null) path.Release(this);
            path = null;

            //var followEntity = EntityUIBase.GetEntity(followTransform.gameObject);
            LevelLayer? targetLevelLayer;
            var targetPosition = GetTargetPosition(out targetLevelLayer);

            var entityLevelLayer = _floorComponent.GetLevel();
            if (targetLevelLayer == null)
                targetLevelLayer = entityLevelLayer;

            int graphMask = Graph.GetGraphMask(entityLevelLayer, targetLevelLayer);
            //_behavior.Path.IsFinished = false;

            _calculatingPath = true;
            _aiComponent.StartPathProcessing(GetFeetPosition().ToVector3(0), targetPosition.ToVector3(0), graphMask);
            //_seeker.StartPath(GetFeetPosition().ToVector3(0), targetPosition.ToVector3(0), null, graphMask);
        }

        private Vector2D GetTargetPosition(out LevelLayer? levelLayer)
        {
            levelLayer = null;
            switch (_followTarget)
            {
                case FollowTarget.Home:
                    levelLayer = _aiComponent.GetHomeLevelLayer();
                    return _aiComponent.GetHomePosition();
                case FollowTarget.Follow:
                    var followTransform = _aiComponent.GetFollow();
                    return GetEntityPosition(followTransform, out levelLayer);
                case FollowTarget.Target:
                    var transform = _aiComponent.Target;
                    return GetEntityPosition(transform, out levelLayer);
                default:
                    return new Vector2D(0f, 0f);
            }
        }

        private Vector2D GetEntityPosition(Transform entity, out LevelLayer? levelLayer)
        {
            levelLayer = null;
            var followRepo = entity.GetComponent<IComponentRepository>();
            if (followRepo == null)
                throw new Exception("Smart Follow cannot find Follow target");
            var followPhysicsComponent = followRepo.Components.GetComponent<PhysicsComponent>();
            var followFloorComponent = followRepo.Components.GetComponent<FloorComponent>();
            if (followFloorComponent != null)
                levelLayer = followFloorComponent.GetLevel();
            //Vector2D targetPosition;
            if (followPhysicsComponent == null)
            {
                return (Vector2D) entity.position; // not a sprite
            }
            else
            {
                return (Vector2D)followPhysicsComponent.GetFeetWorldPosition() + _offset;
            }       
        }

        //private PhysicsComponent GetFollowEntity()
        //{            
        //    //return EntityUIBase.GetEntity(_followTransform.gameObject);
        //    return _followTransform.GetComponent<PhysicsComponent>();
        //}

        public override void FixedUpdate()
        {
            if (!_isSetup)
                return;
            if (_calculatingPath)
                return;

            base.FixedUpdate();
            //if (_behavior.Path.IsFinished)
            //{
            //    // Reiniate the follow to keep close to the followee
            //    InitiateFollow();
            //    return;
            //    //_behavior.TurnOff();
            //    //_sprite.Stop();
            //}

            if (Time.time > _nextStuckCheck)
            {
                if (Vector2D.Vec2DDistance(GetFeetPosition(), _lastCheckedPosition) < .16f)
                {
                    // stuck, recalculate position
                    //InitiateFollow();
                    StateMachine.GetStateInfo().IsStuck = true;
                    return;
                }
                _lastCheckedPosition = GetFeetPosition();
                _nextStuckCheck = Time.time + 1;
            }

            if (Data == null || Data.Path == null)
                return;
            //for (int i = 0; i < Data.Path.Count - 1; i++)
            //{
                // TODO - Move to OnDrawGizmos or something
                //Debug.DrawLine(Data.Path[i], Data.Path[i + 1], Color.red, 10, false);
            //}

            //foreach (var vector in _path)
            //{

            //}
        }

        private Vector2D GetFeetPosition()
        {
            return (Vector2D)_physicsComponent.GetFeetWorldPosition() + _offset;
        }

        //private Vector3 GetFeetPosition3D()
        //{
        //    return _physicsComponent.GetFeetPosition3D() + _offset.ToVector3(0);
        //}

        public virtual void OnPathComplete(PathfindingData newPath)
        {
            //Replace the old path
            //Vector2D tempPath = 
            //path = newPath.path.Select(i => (Vector2D)i).ToArray();
            if (Data.Path != null)
            {
                // We are replacing it, so better to release it back to the pool
                ObjectPool.Instance.ReleaseToPool(ObjectPoolType.Vector3List, Data.Path);
            }
            Data.Path = newPath.Path;
                //foreach (var vpath in path.vectorPath)
                //{
                //    Data.Path.Add(vpath);
                //}

                if (Data.Path.Count == 0 || Vector2D.Vec2DDistanceSq(Data.Path[0], Data.Path[Data.Path.Count - 1]) < _minDistanceForPath * _minDistanceForPath)
                {
                    // Distance is too short, indicate we do not want this path
                    //p.Release(this);
                    path = null;
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                        Telegrams.StopMovement, null);
                    //_physicsComponent.Stop();
                    _behavior.Path.IsFinished = true;
                    _behavior.TurnOff();
                    StateMachine.GetStateInfo().IsStuck = true;
                    return;
                }

            //Reset some variables
            //if (_behavior.Path.GetWaypoints() == null)
            _lastCheckedPosition = GetFeetPosition();
            _nextStuckCheck = Time.time + 1;
            _behavior.SetPath(Data.Path);
            _behavior.Path.PathWalkingDirection = RQ.Physics.SteeringBehaviors.PathWalkingDirection.Forwards;
            _behavior.Path.IsFinished = false;
            _behavior.Path.PathType = PathType.Once;
            //_behavior.Path
            _behavior.TurnOn();
            //_steering.GetSteeringBehavior(behavior_type.wall_avoidance).TurnOn();

            //currentWaypointIndex = 0;
            //targetReached = false;

            //The next row can be used to find out if the path could be found or not
            //If it couldn't (error == true), then a message has probably been logged to the console
            //however it can also be got using p.errorLog
            //if (p.error)

            //if (closestOnPathCheck)
            //{
            //    Vector3 p1 = Time.time - lastFoundWaypointTime < 0.3f ? lastFoundWaypointPosition : p.originalStartPoint;
            //    Vector3 p2 = GetFeetPosition();
            //    Vector3 dir = p2 - p1;
            //    float magn = dir.magnitude;
            //    dir /= magn;
            //    int steps = (int)(magn / pickNextWaypointDist);


            //    for (int i = 0; i <= steps; i++)
            //    {
            //        CalculateVelocity(p1);
            //        p1 += dir;
            //    }

            //}
        }

        //public override bool OnMessage(Telegram telegram)
        //{
        //    if (base.OnMessage(telegram))
        //        return true;

        //    switch (telegram.Msg)
        //    {
        //        case Enums.Telegrams.ProcessStateEvent:
        //            if (telegram.ExtraInfo.ToString() == ID)
        //            {
        //                ProcessShoot();
        //                //sprite.ProcessAttack();
        //                return true;
        //            }
        //            break;
        //    }
        //    return false;
        //}

        //private void ProcessShoot()
        //{
        //    var velocity = CalculateVelocity();

        //    var angleToRotate = _sprite.GetFacingDirection().GetDirectionAngle();

        //    var position = _sprite.GetPos() + RotateAroundAxis(_offset.ToVector3(_sprite.GetPos().z), angleToRotate, new Vector3(0, 0, 1));

        //    //var newObject = ((Transform)GameObject.Instantiate(transform, position, transform.rotation)).GetComponent<IEntityUI>();
        //    var newObject = _sprite.Instantiate(_objectToShoot, position, transform.rotation).GetComponent<IEntityUI>();
        //    //Transform t;
        //    //t.
        //    var newSprite = newObject.GetEntity() as ISprite;

        //    if (newSprite != null)
        //        newSprite.SetVelocity(velocity.ToVector3(0));
        //}


        //public Vector3 RotateAroundAxis(Vector3 v, float a, Vector3 axis)
        //{
        //    //if (bUseRadians) a *= MathUtil.RAD_TO_DEG;
        //    var q = Quaternion.AngleAxis(a, axis);
        //    return q * v;
        //}

        //private Vector2D CalculateVelocity()
        //{
        //    var speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);

        //    Vector2D velocity = Vector2D.Zero();

        //    switch (_shootTarget)
        //    {
        //        case ShootTarget.StraightShot:
        //            velocity = _sprite.GetFacingDirectionVector();
        //            break;
        //        case ShootTarget.Random:
        //            //targetVector = new Vector2D(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        //            var angle = UnityEngine.Random.Range(0f, 360f);
        //            var x = Mathf.Cos(angle);
        //            var y = Mathf.Sin(angle);
        //            velocity = new Vector2D(x, y);
        //            break;
        //        case ShootTarget.ToLocation:
        //            velocity = _location - _sprite.GetPos();
        //            break;
        //        case ShootTarget.ToTarget:
        //            velocity = _target.transform.position - _sprite.GetPos();
        //            break;
        //    }

        //    velocity = Vector2D.Vec2DNormalize(velocity) * speed;

        //    return velocity;
        //}

        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            //if (!entitySerializedData.Datas.ContainsKey(GetName()))
            base.SerializeComponent(entitySerializedData, Data);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            Data = base.DeserializeComponent<SmartFollowData>(entitySerializedData);
            
            //Data = Persistence.DeserializeObject<SmartFollowData>(entitySerializedData.Datas[GetName()]);
        }
    }
}
