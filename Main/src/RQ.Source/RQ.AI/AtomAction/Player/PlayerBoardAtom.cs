using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class PlayerBoardAtom : AtomActionBase
    {
        private GameObject _boardPlatform;
        private Joint joint;

        private IComponentRepository _carryObjectRepo;
        private IComponentRepository _boardPlatformRepo;
        //public TweenToColorInfo _overlayColor = null;
        //private float _endTime;
        private long _boardId;
        private Action<Telegram2> _boardDel;
        private SpriteCommonComponent _spriteCommonComponent;

        public override void Start(IComponentRepository entity)
        {
            if (_boardDel == null)
            {
                _boardDel = (data) =>
                {
                    // Record the repo of the boarded platform you are exiting so we can dock the player there.
                    var boardPlatformGo = data.ExtraInfo as GameObject;
                    _boardPlatformRepo = boardPlatformGo.GetComponent<IComponentRepository>();
                    _isRunning = false;
                };
            }

            base.Start(entity);

            _boardPlatformRepo = _boardPlatform.GetComponent<IComponentRepository>();
            if (_boardPlatformRepo == null)
            {
                Debug.LogError("No board repo.");
                return;
            }
            var boardAiComponent = _boardPlatformRepo.Components.GetComponent<AIComponent>();
            if (boardAiComponent == null)
            {
                Debug.LogError($"Board component does not have an AI Component to get carry object");
                return;
            }
            var platformWaypointComponent = _boardPlatformRepo.Components.GetComponent<WaypointComponent>();
            
            var carryObject = boardAiComponent.Target;
            // Move the raft to the boarding platform waypoint
            carryObject.position = platformWaypointComponent._waypoints[1].transform.position;
            _carryObjectRepo = carryObject.GetComponent<IComponentRepository>();
            MessageDispatcher2.Instance.DispatchMsg("Boarded", 0f, _entity.UniqueId, _carryObjectRepo.UniqueId, null);
            entity.transform.position = carryObject.position + new Vector3(0f, 0.12f);
            var otherRigidBody = carryObject.GetComponent<Rigidbody>();
            joint = entity.gameObject.AddComponent<FixedJoint>();
            //var joint = _target.GetComponent<Joint>();
            //joint.
            joint.connectedBody = otherRigidBody;
            var raftPhysicsComponent = _carryObjectRepo.Components.GetComponent<PhysicsComponent>();
            raftPhysicsComponent.Stop();
            //MessageDispatcher2.Instance.DispatchMsg("TweenToColor", 0f, string.Empty, "Graphics Engine", _overlayColor);
            //_endTime = UnityEngine.Time.time + _overlayColor.Delay + _overlayColor.Duration;
            //MessageDispatcher2.Instance.
            if (_spriteCommonComponent == null)
                _spriteCommonComponent = entity.Components.GetComponent<SpriteCommonComponent>();
            _spriteCommonComponent.SetTrackSafeTiles(false);
        }

        public override void End()
        {
            MessageDispatcher2.Instance.DispatchMsg("IsUnboarded", 0f, _entity.UniqueId, _carryObjectRepo.UniqueId, null);
            var physicsComponent = _entity.Components.GetComponent<PhysicsComponent>();
            var boardPlatformWaypoints = _boardPlatformRepo.Components.GetComponent<WaypointComponent>();
            // TODO Fix this
            physicsComponent.SetFeetWorldPosition(boardPlatformWaypoints._waypoints[0].transform.position);
            GameObject.Destroy(joint);
            _spriteCommonComponent.SetTrackSafeTiles(true);
            //MessageDispatcher2.Instance.DispatchMsg("ChargingComplete", 0f, _entity.UniqueId, _entity.UniqueId, null);
            //var physicsComponent = _entity.Components.GetComponent<PhysicsComponent>();
            //var physicsData = physicsComponent.GetPhysicsData();
            //physicsData.MaxSpeed = physicsData.OriginalMaxSpeed;
            //var steeringAffector = physicsComponent.GetPhysicsAffector("Steering");
            //if (steeringAffector != null)
            //    steeringAffector.MaxSpeed = steeringAffector.OriginalMaxSpeed;
        }

        public override void StartListening(IComponentRepository repo)
        {
            _boardId = MessageDispatcher2.Instance.StartListening("Board", _entity.UniqueId, _boardDel);
        }

        public override void StopListening(IComponentRepository repo)
        {
            MessageDispatcher2.Instance.StopListening("Board", _entity.UniqueId, _boardId);
        }

        public override AtomActionResults OnUpdate()
        {
            //if (_endTime > UnityEngine.Time.time)
            //    return AtomActionResults.Success;
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetBoardPlatform(GameObject boardPlatform)
        {
            _boardPlatform = boardPlatform;
        }
    }
}
