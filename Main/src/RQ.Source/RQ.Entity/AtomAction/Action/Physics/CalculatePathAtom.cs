using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Enum;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.ObjectPool;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.Pathfinding;
using RQ.Physics.SteeringBehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class CalculatePathAtom : AtomActionBase
    {
        public enum DestinationEnum
        {
            Target = 0,
            Ranodm = 1,
            RandomLocation = 2,
            RandomSubLocation = 3,
            CurrentDestinationLocation = 4,
            Follow = 5,
            Home = 6
        }

        /** Determines how often it will search for new paths.
         * If you have fast moving targets or AIs, you might want to set it to a lower value.
         * The value is in seconds between path requests.
         */
        public float repathRate = 0.5F;
        public DestinationEnum _followTarget = DestinationEnum.Target;
        private AIComponent _aiComponent;
        public string _physicsComponentName;
        private List<Vector3> path;
        public List<Vector3> Path { get { return path; } set { path = value; } }
        public Vector2 _offset;
        public float _minDistanceForPath = .5f;
        private CollisionComponent _collisionComponent;
        private FloorComponent _floorComponent;
        private PhysicsComponent _physicsComponent;
        private SteeringBehaviorManager _steering;
        private RQ.Physics.SteeringBehaviors.FollowPath _behavior;
        private System.Action Failed;
        private bool isSearching;
        /** Time when the last path request was sent */
        protected float lastRepath = -9999;
        //private SteeringBehaviorManager _steering;
        //public string _physicsComponentName;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            isSearching = false;
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>(_physicsComponentName);
            _aiComponent = entity.Components.GetComponent<AIComponent>();
            _collisionComponent = entity.Components.GetComponent<CollisionComponent>();
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _steering = _physicsComponent.GetSteering() as SteeringBehaviorManager;
            _floorComponent = entity.Components.GetComponent<FloorComponent>();
            //_steering.TurnOn(Physics.behavior_type.seek);
            _behavior = _steering.GetSteeringBehavior(behavior_type.follow_path) as RQ.Physics.SteeringBehaviors.FollowPath;
            if (physicsComponent == null)
            {
                Debug.Log("(SeekAtom) Unable to locate physics component " + _physicsComponentName);
                return;
            }
            _aiComponent.OnPathComplete += OnPathComplete;

            

            //if (_path == null)
            //    _path = new List<Vector2D>();
            if (repathRate == 0f)
                InitiateFollow();
            else
            {
                var mb = entity as MonoBehaviour;
                mb.StartCoroutine(RepeatTrySearchPath());
            }
            //_steering = physicsComponent.GetSteering();
        }

        /** Tries to search for a path every #repathRate seconds.
 * \see TrySearchPath
 */
        protected IEnumerator RepeatTrySearchPath()
        {
            while (true)
            {
                TrySearchPath();
                yield return new WaitForSeconds(repathRate);
            }
        }

        /** Tries to search for a path.
         * Will search for a new path if there was a sufficient time since the last repath and both
         * #canSearchAgain and #canSearch are true and there is a target.
         *
         * \returns The time to wait until calling this function again (based on #repathRate)
         */
        public float TrySearchPath()
        {
            if (Time.time - lastRepath >= repathRate && !isSearching)
            {
                InitiateFollow();
                return repathRate;
            }
            else
            {
                //StartCoroutine (WaitForRepath ());
                float v = repathRate - (Time.time - lastRepath);
                return v < 0 ? 0 : v;
            }
        }

        private void InitiateFollow()
        {
            lastRepath = Time.time;
            isSearching = true;
            //if (_calculatingPath)
            //    return;

            //var followPhysicsComponent = _followPhysicsComponent;
            //if (path != null) path.Release(this);
            //Data.Path = null;
            path = null;

            //var followEntity = EntityUIBase.GetEntity(followTransform.gameObject);
            LevelLayer? targetLevelLayer;
            var targetPosition = GetTargetPosition(out targetLevelLayer);

            var entityLevelLayer = _floorComponent.GetLevel();
            if (targetLevelLayer == null)
                targetLevelLayer = entityLevelLayer;

            int graphMask = Graph.GetGraphMask(entityLevelLayer, targetLevelLayer);
            //_behavior.Path.IsFinished = false;

            //Debug.Log("Calculating Path from " + GetFeetPosition() + " to " + targetPosition.ToVector3(0));
            //_calculatingPath = true;
            _aiComponent.StartPathProcessing(GetFeetPosition(), targetPosition.ToVector3(0), graphMask);
            //_seeker.StartPath(GetFeetPosition().ToVector3(0), targetPosition.ToVector3(0), null, graphMask);
        }

        public override void End()
        {
            //Make sure we receive callbacks when paths complete
            _aiComponent.OnPathComplete -= OnPathComplete;
            if (repathRate != 0f)
            { 
                var mb = _entity as MonoBehaviour;
                mb.StopCoroutine(RepeatTrySearchPath());
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
            //return AtomActionResults.Success;
        }

        private Vector2D GetTargetPosition(out LevelLayer? levelLayer)
        {
            levelLayer = null;
            switch (_followTarget)
            {
                case DestinationEnum.Home:
                    levelLayer = _aiComponent.GetHomeLevelLayer();
                    return _aiComponent.GetHomePosition();
                case DestinationEnum.Follow:
                    var followTransform = _aiComponent.GetFollow();
                    return GetEntityPosition(followTransform, out levelLayer);
                case DestinationEnum.Target:
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
            var floorCollisionComponent = followRepo.Components.GetComponent<FloorComponent>();
            if (floorCollisionComponent != null)
                levelLayer = floorCollisionComponent.GetLevel();
            //Vector2D targetPosition;
            if (followPhysicsComponent == null)
            {
                return (Vector2D)entity.position; // not a sprite
            }
            else
            {
                return followPhysicsComponent.GetFeetWorldPosition() + _offset;
            }
        }

        private Vector2 GetFeetPosition()
        {
            return _physicsComponent.GetFeetWorldPosition() + _offset;
        }

        public virtual void OnPathComplete(PathfindingData newPath)
        {
            isSearching = false;
            if (path != null)
            {
                // We are replacing it, so better to release it back to the pool
                ObjectPool.Instance.ReleaseToPool(ObjectPoolType.Vector3List, path);
            }
            //Replace the old path
            path = newPath.Path;
            //foreach (var vpath in path.vectorPath)
            //{
            //    Data.Path.Add(vpath);
            //}

            if (path.Count == 0 || (path[0] - path[path.Count - 1]).sqrMagnitude < _minDistanceForPath * _minDistanceForPath)
            {
                // Distance is too short, indicate we do not want this path
                //p.Release(this);
                path = null;
                //MessageDispatcher.Instance.DispatchMsg(0f, string.Empty, _physicsComponent.UniqueId,
                //    Telegrams.StopMovement, null);
                //_physicsComponent.Stop();
                //_behavior.Path.IsFinished = true;
                //_behavior.TurnOff();
                //StateMachine.GetStateInfo().IsStuck = true;
                Failed?.Invoke();
                return;
            }

            //Reset some variables
            //if (_behavior.Path.GetWaypoints() == null)
            //_lastCheckedPosition = GetFeetPosition();
            //_nextStuckCheck = Time.time + 1;

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
            MessageDispatcher2.Instance.DispatchMsg("NewPath", 0f, _entity.UniqueId, _entity.UniqueId, newPath);
            
            //}

            _isRunning = false;
        }

        public void SetFailed(System.Action failed)
        {
            Failed = failed;
        }
    }
}
