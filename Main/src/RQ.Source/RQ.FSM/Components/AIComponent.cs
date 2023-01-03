using RQ.Common.Components;
using RQ.Messaging;
using RQ.Physics;
using System.Collections.Generic;
using UnityEngine;
using RQ.Common.Container;
using RQ.Enum;
using RQ.Common;
using RQ.Model.AI;
using System;
using RQ.FSM.Components;
using RQ.Model.Physics;
using RQ.Entity.Components;

namespace RQ.FSM.V2
{
    [AddComponentMenu("RQ/Components/AI")]
    public class AIComponent : ComponentPersistence<AIComponent>, IAIComponent
    {
        public enum TargetType
        {
            None = 0,
            MainCharacter = 1
        }

        //[SerializeField]
        //private Seeker _seeker;
        //public Seeker Seeker { get { return _seeker; } set { _seeker = value; } }
        [SerializeField]
        private GameObject _pathFinder;
        private IPathFinder PathFinder;

        [SerializeField]
        private TargetType _targetType;

        [SerializeField]
        private Transform _target;
        public Transform Target { get { return _target; } set { _target = value; } }

        [SerializeField]
        private List<Transform> _targets;
        public List<Transform> Targets { get { return _targets; } set { _targets = value; } }

        [SerializeField]
        [Tag]
        public string[] TargetTags;

        public Location CurrentDestinationLocation { get; set; }
        public Location CurrentDestinationSubLocation { get; set; }

        [SerializeField]
        private List<Transform> _locations;
        public List<Transform> Locations { get { return _locations; } set { _locations = value; } }

        [SerializeField]
        private Transform _parent;
        public Transform Parent { get { return _parent; } set { _parent = value; } }

        [SerializeField]
        private List<Transform> _children;
        public List<Transform> Children { get { return _children; } set { _children = value; } }

        [SerializeField]
        private LayerMask _targetMask;
        public LayerMask TargetMask { get { return _targetMask; } set { _targetMask = value; } }

        [SerializeField]
        private LayerMask[] _avoidLayersMasks;
        public LayerMask[] AvoidLayersMasks { get { return _avoidLayersMasks; } set { _avoidLayersMasks = value; } }

        [SerializeField]
        private Transform _follow;
        public Transform Follow { get { return _follow; } set { _follow = value; } }

        [SerializeField]
        private TargetingData _targetingData;
        public TargetingData TargetingData { get { return _targetingData; } set { _targetingData = value; } }

        [SerializeField]
        private Vector2D _homePosition;
        [SerializeField]
        private LevelLayer _homeLevelLayer;
        [SerializeField]
        private float _homeSickDistance;
        private float _homeSickDistanceSq;
        [SerializeField]
        private float _runHomeSpeed;

        [SerializeField]
        private bool _setHomePositonToStartPosition;

        private List<Transform> _targetsMinusParent;
        private List<Transform> _locationsMinusParent;

        private long _setAITargetId, _setWaypointsId, _setWaypointsId2;
        private Action<Telegram2> _setAITargetDelegate, _setWaypointsDelegate;

        public override void Awake()
        {
            base.Awake();
            if (_pathFinder != null)
                PathFinder = _pathFinder.GetComponent<IPathFinder>();
            if (PathFinder != null)
                PathFinder.OnPathComplete += ProcessPath;
            _homeSickDistanceSq = _homeSickDistance * _homeSickDistance;
            _targetingData = new TargetingData();
            if (_homePosition.isZero() || _setHomePositonToStartPosition)
                _homePosition = this.transform.position;
            _setAITargetDelegate = (data) =>
            {
                Target = data.ExtraInfo as Transform;
            };
            _setWaypointsDelegate = (data) =>
            {
                TargetingData.Waypoints = data.ExtraInfo as Transform;
            };
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (PathFinder != null)
                PathFinder.OnPathComplete -= ProcessPath;
            if (_parent != null)
            {
                var parentRepo = _parent.gameObject.GetComponent<IComponentRepository>();
                MessageDispatcher2.Instance.DispatchMsg("ChildDied", 0f, _componentRepository.UniqueId, parentRepo.UniqueId, _componentRepository);
            }
            if (_children != null)
            {
                for (int i = 0; i < _children.Count; i++)
                {
                    var childRepo = _children[i].gameObject.GetComponent<IComponentRepository>();
                    MessageDispatcher2.Instance.DispatchMsg("ParentDied", 0f, _componentRepository.UniqueId, childRepo.UniqueId, _componentRepository);
                }
            }
        }

        public void ProcessPath(PathfindingData data)
        {
            OnPathComplete?.Invoke(data);
        }

        public override void Start()
        {
            base.Start();
            switch (_targetType)
            {
                case TargetType.MainCharacter:
                    _target = EntityContainer._instance?.GetMainCharacter()?.transform;
                    break;
            }
            SetupTargetTags();
        }

        public event Action<PathfindingData> OnPathComplete;

        private void SetupTargetTags()
        {
            if (TargetTags != null)
            {
                foreach (var tag in TargetTags)
                {
                    var entity = EntityContainer._instance.GetEntityFromTag(tag);
                    if (entity == null)
                        continue;
                    Target = entity.transform;
                    return;
                    //foreach (var entity in entities)
                    //{

                    //}
                }
            }
        }

        public override void StartListening()
        {
            base.StartListening();
            _setAITargetId = MessageDispatcher2.Instance.StartListening("SetAITarget", _componentRepository.UniqueId, _setAITargetDelegate);
            //_componentRepository.StartListening("SetAITarget", this.UniqueId, _setAITargetDelegate);
            _setWaypointsId =
                MessageDispatcher2.Instance.StartListening("SetWaypoints", _componentRepository.UniqueId, _setWaypointsDelegate);
            _setWaypointsId2 = MessageDispatcher2.Instance.StartListening("SetWaypoints", this.UniqueId, _setWaypointsDelegate);
            MessageDispatcher2.Instance.StartListening("ChildDied", _componentRepository.UniqueId, (data) =>
            {
                Debug.Log("(AIComponent) ChildDied message recorded in " + this.name);
                var child = data.ExtraInfo as ComponentRepository;
                if (child == null)
                    return;
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i] == child.transform)
                    {
                        _children.RemoveAt(i);
                        break;
                    }
                }
            });
            MessageDispatcher2.Instance.StartListening("ParentDied", _componentRepository.UniqueId, (data) =>
            {
                Debug.Log("(AIComponent) ParentDied message recorded in " + this.name);
                var parent = data.ExtraInfo as ComponentRepository;
                if (parent == null)
                    return;
                if (_parent == parent.transform)
                    _parent = null;
            });
            //_componentRepository.StartListening("SetWaypoints", this.UniqueId, _setWaypointsDelegate, true);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetAITarget", _componentRepository.UniqueId, _setAITargetId);
            //_componentRepository.StopListening("SetAITarget", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetWaypoints", _componentRepository.UniqueId, _setWaypointsId);
            MessageDispatcher2.Instance.StopListening("SetWaypoints", this.UniqueId, _setWaypointsId2);
            //_componentRepository.StopListening("SetWaypoints", this.UniqueId, true);
        }

        public bool RaycastCheck(string spriteBaseComponentUniqueId, int layerMask)
        {
            var thisPos = (Vector2)GetWorldPos();
            var otherPos = GetWorldPosition(spriteBaseComponentUniqueId);

            var direction = otherPos - thisPos;

            var raycast = UnityEngine.Physics.Raycast(thisPos, direction, direction.magnitude, layerMask);
            //var raycasts = Physics2D.RaycastAll(thisPos, direction, direction.magnitude).AsEnumerable();
            //raycasts = raycasts.Where(i => !_colliders.Contains(i.collider));
            //List<RaycastHit2D> newList = new List<RaycastHit2D>();
            //foreach (var raycast in raycasts)
            //{
            //    bool sameTransform = raycast.transform == gameObject.transform;
            //    if (!sameTransform)
            //        newList.Add(raycast);
            //}
            ////raycasts = raycasts.Where(i => i.transform != gameObject.transform);
            //// TODO Add the layer mask - don't want level one items hitting level two items
            //return newList;
            return raycast;
        }

        private Vector2 GetWorldPosition(string spriteBaseComponentUniqueId)
        {
            Vector2D otherPos = Vector2D.Zero();
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, spriteBaseComponentUniqueId,
                Enums.Telegrams.GetWorldPos, null, (pos) => otherPos = (Vector2D)pos);
            return otherPos;
        }

        //public override void OnEnable()
        //{
        //    base.OnEnable();

        //}

        //public override void OnDisable()
        //{
        //    base.OnDisable();
            
        //}

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.ChooseRandomTarget:
                    _target = GetRandomTargetMinusParent();
                    break;
                case Enums.Telegrams.ChooseRandomLocation:
                    ChooseRandomLocation((Vector2)msg.ExtraInfo, false);
                    if (msg.Act != null)
                        msg.Act(CurrentDestinationLocation);
                    break;
                case Enums.Telegrams.ChooseRandomSubLocation:
                    ChooseRandomLocation((Vector2)msg.ExtraInfo, true);
                    if (msg.Act != null)
                        msg.Act(CurrentDestinationSubLocation);
                    break;
                case Enums.Telegrams.GetCurrentDestinationLocation:
                    if (msg.Act != null)
                        msg.Act(CurrentDestinationLocation);
                    break;
            }

            return false;
        }

        public Transform GetRandomTargetMinusParent()
        {
            var targetsMinusParent = GetTargetsMinusParent();
            var targetCount = targetsMinusParent.Count;
            var pick = UnityEngine.Random.Range(0, targetCount);
            return targetsMinusParent[pick];
        }

        private void ChooseRandomLocation(Vector2D thisPosition, bool getSubLocations)
        {
            //var physicsComponent = base._componentRepository.Components.GetComponens<PhysicsComponent>();
            var targetsMinusParent = GetLocationsMinusParent();
            var targetCount = targetsMinusParent.Count;
            var pick = UnityEngine.Random.Range(0, targetCount);
            var location = targetsMinusParent[pick];
            //Location newLocation;
            CurrentDestinationLocation = location.GetComponent<Location>();
            if (getSubLocations)
            {
                var locations = location.GetComponentsInChildren<Location>();
                CurrentDestinationSubLocation = Vector2D.GetClosestBehaviourToPoint(thisPosition, locations);
            }
        }

        //private Location GetClosestLocationToPoint(Vector2D point, IEnumerable<Location> locations)
        //{
        //    Location closestLocation = null;
        //    float closestDistance = 5000f;
        //    foreach (var location in locations)
        //    {                
        //        var distance = Vector2D.Vec2DDistanceSq(location.transform.position, 
        //            point);
        //        if (distance < closestDistance)
        //        {
        //            closestLocation = location;
        //            closestDistance = distance;
        //        }
        //    }
        //    return closestLocation;
        //}

        private IList<Transform> GetTargetsMinusParent()
        {
            // Only ever create this list once
            if (_targetsMinusParent == null)
            {
                _targetsMinusParent = new List<Transform>();
            }
            _targetsMinusParent.Clear();
            foreach (var target in _targets)
            {
                if (target == _parent)
                    continue;
                _targetsMinusParent.Add(target);
            }

            return _targetsMinusParent;
            //return _targets.Where(i => i != _parent).ToList();
        }

        private IList<Transform> GetLocationsMinusParent()
        {
            // Only ever create this list once
            if (_locationsMinusParent == null)
            {
                _locationsMinusParent = new List<Transform>();
            }
            _locationsMinusParent.Clear();
            foreach (var target in _locations)
            {
                if (target == _parent)
                    continue;
                _locationsMinusParent.Add(target);
            }

            return _locationsMinusParent;
            //return _locations.Where(i => i != _parent).ToList();
        }

        //public Seeker GetSeeker()
        //{
        //    return _seeker;
        //}

        public void StartPathProcessing(Vector3 pos, Vector3 target, int graphMask)
        {
            PathFinder.StartPathProcessing(pos, target, graphMask);
            //_seeker.StartPath(pos, target, null, graphMask);
        }

        public virtual Transform GetFollow()
        {
            return _follow;
        }

        public Vector2D GetHomePosition()
        {
            return _homePosition;
        }

        public void SetHomePosition(Vector2D newPos)
        {
            _homePosition = newPos;
        }

        public LevelLayer GetHomeLevelLayer()
        {
            return _homeLevelLayer;
        }

        public float GetHomeSickDistanceSq()
        {
            return _homeSickDistanceSq;
        }

        public float GetRunHomeSpeed()
        {
            return _runHomeSpeed;
        }

        public Transform GetTarget()
        {
            if (!GetIsStarted())
                SetupTargetTags();
            return _target;
        }

        public Transform GetWaypoints()
        {
            return TargetingData.Waypoints;
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var aiData = new AIData();
            
        //    aiData.CurrentDestinationLocation = CurrentDestinationLocation.GetRepoId();
        //    aiData.CurrentDestinationSubLocation = CurrentDestinationSubLocation.GetRepoId();
        //    aiData.Follow = Follow.GetRepoId();

        //    if (Locations != null)
        //        aiData.Locations = Locations.Select(i => i.GetRepoId()).ToArray();
        //    if (TargetingData.Waypoints != null)
        //        aiData.WaypointUniqueId = TargetingData.Waypoints.GetRepoId();
        //    aiData.Parent = Parent.GetRepoId();
        //    aiData.Target = Target.GetRepoId();
        //    //aiData.TargetMask = TargetMask;
        //    aiData.TargetMaskInt = _targetMask.value;
        //    if (Targets != null)
        //        aiData.Targets = Targets.Select(i => i.GetRepoId()).ToArray();
        //    aiData.HomePosition = _homePosition;

        //    base.SerializeComponent(entitySerializedData, aiData);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    if (!entitySerializedData.ComponentData.ContainsKey(GetName()))
        //        return;

        //    var aiData = base.DeserializeComponent<AIData>(entitySerializedData);            

        //    var entityContainer = EntityContainer._instance;

        //    if (!string.IsNullOrEmpty(aiData.CurrentDestinationLocation))
        //        CurrentDestinationLocation = entityContainer.GetEntity(aiData.CurrentDestinationLocation).transform.GetComponent<Location>();
        //    if (!string.IsNullOrEmpty(aiData.CurrentDestinationSubLocation))
        //        CurrentDestinationSubLocation = entityContainer.GetEntity(aiData.CurrentDestinationSubLocation).transform.GetComponent<Location>();
        //    if (!string.IsNullOrEmpty(aiData.Follow))
        //        Follow = entityContainer.GetEntity<IComponentRepository>(aiData.Follow).transform;
        //    if (aiData.Locations != null)
        //        Locations = aiData.Locations.Select(i => entityContainer.GetEntity(i).transform).ToList();
        //    if (!string.IsNullOrEmpty(aiData.Parent))
        //        Parent = entityContainer.GetEntity(aiData.Parent).transform;
        //    if (!string.IsNullOrEmpty(aiData.Target))
        //        Target = entityContainer.GetEntity(aiData.Target).transform;
        //    _targetMask.value = aiData.TargetMaskInt;
        //    if (aiData.Targets != null)
        //        Targets = aiData.Targets.Select(i => entityContainer.GetEntity(i).transform).ToList();
        //    _homePosition = aiData.HomePosition;
        //    if (!string.IsNullOrEmpty(aiData.WaypointUniqueId))
        //        TargetingData.Waypoints = EntityContainer._instance.GetEntity<IComponentRepository>(aiData.WaypointUniqueId).transform;
        //    else
        //        TargetingData.Waypoints = null;
        //}
    }
}
