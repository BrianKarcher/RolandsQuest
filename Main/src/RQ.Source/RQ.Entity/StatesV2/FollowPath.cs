using System.Collections.Generic;
using RQ.Physics;
using UnityEngine;
using RQ.Extensions;
using RQ.Physics.SteeringBehaviors;
using RQ.Enums;
using RQ.Messaging;
using RQ.Physics.Pathfinding;
using RQ.Model.Physics;
using RQ.Common.Container;
using RQ.Entity.Components;
using RQ.Physics.Components;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Follow Path")]
    public class FollowPath : AnimatorState
    {
        [SerializeField]
        private Transform _waypoints = null;

        [SerializeField]
        private bool _useEntityWaypoints = false;

        [SerializeField]
        private PathType _pathType = PathType.Pingpong;

        [SerializeField]
        private PathWalkingDirection _pathWalkingDirection = PathWalkingDirection.Forwards;

        //[SerializeField]
        //private DirectionMode _facingDirectionMode = DirectionMode.Automatic;
        
        [SerializeField]
        private Direction _facingDirection = Direction.Right;

        [SerializeField]
        private FollowPathData Data;

        private List<Vector3> _path = new List<Vector3>();

        private PhysicsComponent _mainPlayerPhysicsComponent;

        //private int _currentWaypoint = 0;

        private RQ.Physics.SteeringBehaviors.FollowPath _behavior;

        public override void SetupState()
        {
            base.SetupState();
            _behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
        }

        public override void Enter()
        {
            base.Enter();
            if (_useEntityWaypoints)
            {
                _waypoints = _aiComponent.GetWaypoints();
            }
            //if (_waypoints != null)
            //{
                var path = CreatePath();
                _behavior.SetPath(path);
                _behavior.Path.SetCurrentWaypoint(Data.CurrentWaypoint);
            //}
            var mainCharacter = EntityContainer._instance.GetMainCharacter();
            _mainPlayerPhysicsComponent = mainCharacter.Components.GetComponent<PhysicsComponent>();

            _behavior.Path.PathWalkingDirection = _pathWalkingDirection;
            _behavior.Path.IsFinished = false;

            //if (_facingDirectionMode == DirectionMode.Manual)
            //{
            //    _animationComponent.SetFacingDirection(_facingDirection);
            //    _animationComponent.ProcessDirectionChange(_facingDirection);
            //    _animationComponent.GetSpriteAnimator().Render(_facingDirection);
            //}
            //if (_direction == PathDirection.Forwards)
            //{
            //    _behavior.Path.SetCurrentWaypoint(0);
            //}
            //else
            //{
            //    _behavior.Path.SetCurrentWaypoint(_path.Count - 1);
            //}
            //if (_looped)
            //    _behavior.Path.LoopOn();
            //else
            //    _behavior.Path.LoopOff();

            _behavior.Path.PathType = _pathType;

            //_behavior.WaypointChanged = force => 
            //{
            //    WaypointChanged();
            //        //_physicsComponent.SendLocalMessageToAll(0f, Enums.Telegrams.VelocityChanged, _behavior.Path.CurrentWaypoint() - _physicsComponent.GetPos());
            //        //_sprite.SetFacingDirectionBasedOnVelocity(_behavior.Path.CurrentWaypoint() - _sprite.GetPos()); 
            //};
            _steering.TurnOn(behavior_type.follow_path);
            //WaypointChanged();
        }

        private List<Vector3> CreatePath()
        {
            _waypoints.GetPositionsInChildrenOneDeep(_path);
            return _path;
            //var transformList = _waypoints.GetComponentsInChildrenOneDeep<Transform>();
            //var path = new Vector2[transformList.Count];
            //for (int i = 0; i < transformList.Count; i++)
            //{
            //    path[i] = transformList[i].position;
            //}
            //return path;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //if (_facingDirectionMode == DirectionMode.FacePlayer)
            //{
            //    FacePlayer();
            //}
            if (_behavior.Path.IsFinished)
            {
                _steering.TurnOff(behavior_type.follow_path);
                MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
                    Telegrams.StopMovement, null);
                //_physicsComponent.Stop();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _behavior.TurnOff();
            Data.CurrentWaypoint = _behavior.Path.GetCurWaypointIndex();
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    if (_waypoints != null)
        //        Data.WaypointUniqueId = _waypoints.GetRepoId();
        //    base.SerializeComponent(entitySerializedData, Data);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    Data = base.DeserializeComponent<FollowPathData>(entitySerializedData);
            
        //    if (!string.IsNullOrEmpty(Data.WaypointUniqueId))
        //        _waypoints = EntityContainer._instance.GetEntity<IComponentRepository>(Data.WaypointUniqueId).transform;
        //    else
        //        _waypoints = null;
        //}
    }
}
