using RQ.Physics;
using System.Collections.Generic;
using UnityEngine;
using RQ.Extensions;
using RQ.Messaging;
using RQ.Animation;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action 2/Physics/Follow Path")]
    public class FollowPath2Action : ActionBase
    {
        [SerializeField]
        private Transform _waypoints = null;

        //[SerializeField]
        //private bool _looped = false;

        //[SerializeField]
        //private PathType _pathType = PathType.Pingpong;

        //[SerializeField]
        //private PathWalkingDirection _pathWalkingDirection = PathWalkingDirection.Forwards;

        //[SerializeField]
        //private DirectionMode _facingDirectionMode = DirectionMode.Automatic;
        
        //[SerializeField]
        //private Direction _facingDirection = Direction.Right;

        //private RQ.Physics.SteeringBehaviors.FollowPath _behavior;
        //private List<Vector2D> _path;
        [SerializeField]
        protected AnimationComponent _animationComponent;

        [SerializeField]
        private string _conditionMessage;

        private List<Vector3> _path = new List<Vector3>();

        //private ISprite _sprite;

        //public 

        public override void InitAction()
        {
            base.InitAction();
            //_sprite = EntityUIBase.GetEntity(entity);
            //if (_sprite == null)
            //    throw new Exception("FSM - Sprite not set.");
            if (_animationComponent == null)
            {
                var animationComponents = GetEntity().Components.GetComponents<AnimationComponent>();
                if (animationComponents != null)
                {
                    for (int i = 0; i < animationComponents.Count; i++)
                    {
                        var aniamtionComponent = animationComponents[i] as AnimationComponent;
                        if (aniamtionComponent.IsMain())
                        {
                            _animationComponent = aniamtionComponent;
                            break;
                        }
                    }
                    //_animationComponent = animationComponents.FirstOrDefault(i => i.IsMain());
                }
            }

            //_behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;

        }


        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);

            CreatePath();

            MessageDispatcher2.Instance.DispatchMsg("SetPath", 0f, this.UniqueId, _physicsComponent.UniqueId,
                _path);

            MessageDispatcher2.Instance.DispatchMsg(_conditionMessage, 0f, this.UniqueId, GetEntity().UniqueId, null);
            
            //if (_behavior.Path.GetWaypoints() == null)
            //    _behavior.SetPath(_path);
            //_behavior.Path.PathWalkingDirection = _pathWalkingDirection;
            //_behavior.Path.IsFinished = false;

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

            //_behavior.Path.PathType = _pathType;

            //_behavior.WaypointChanged = force => 
            //{
            //    WaypointChanged();
            //        //_physicsComponent.SendLocalMessageToAll(0f, Enums.Telegrams.VelocityChanged, _behavior.Path.CurrentWaypoint() - _physicsComponent.GetPos());
            //        //_sprite.SetFacingDirectionBasedOnVelocity(_behavior.Path.CurrentWaypoint() - _sprite.GetPos()); 
            //};

            //_behavior.TurnOn();
            //WaypointChanged();

            //sprite.GetSteering().TurnOn(behavior_type.follow_path);
            //sprite.GetSteering().
            //if (sprite == null)
            //    return;
            //base.Enter();
            //sprite.SetIsAirborn(true);
            //sprite.SetAirVelocity(sprite.GetJumpVelocity());
            //sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        private void CreatePath()
        {
            // TODO Change this to an Array to speed it up - we can easily calculate how many there will be.
            //var path = new List<Vector2D>();
            //foreach (var point in _waypoints.GetComponentsInChildrenOneDeep<Transform>())
            //{
            //    path.Add(point.position);
            //}
            //return path.ToArray();
            _waypoints.GetPositionsInChildrenOneDeep(_path);
        }

        //private void WaypointChanged()
        //{
        //    if (_facingDirectionMode == DirectionMode.Automatic)
        //    {
        //        var newVelocity = _behavior.Path.CurrentWaypoint() - _physicsComponent.GetPos();
        //        Debug.Log("FollowPath, (" + GetEntity().name + ") Velocity changed to " + newVelocity.ToString());
        //        SendMessageToSpriteBase(0f, Telegrams.VelocityChanged, newVelocity);
        //    }
        //}

        //public override void FixedUpdate()
        //{
        //    if (!_isRunning)
        //        return;

        //    base.FixedUpdate();
        //    if (_behavior.Path.IsFinished)
        //    {
        //        _behavior.TurnOff();
        //        MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
        //            Telegrams.StopMovement, null);
        //        //_physicsComponent.Stop();
        //    }
        //}

        //public override void ActExit(Component otherRigidBody)
        //{
        //    base.ActExit(otherRigidBody);
        //    _behavior.TurnOff();
        //    //sprite.GetSteering().TurnOff(behavior_type.wander);
        //}
    }
}
