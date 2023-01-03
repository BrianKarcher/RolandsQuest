using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using System;
using RQ.Animation;
using RQ.Common.Container;
using RQ.Physics.Components;
using UnityEngine;
using RQ.Entity;

namespace RQ.AI.Action
{
    [Serializable]
    public class CarryObjectFollowParentAtom : AtomActionBase
    {
        //public string MessageToLiftableOnAbort;
        //public string ButtonHeldActionName;
        //private GameObject ParentObject;
        //public float MaxDistance;
        //private PhysicsComponent _physicsComponent;
        //private AnimationComponent _animationComponent;
        //private int _obstacleLayerMask;
        //private GameObject _liftableObject;
        //private int _layerMask;
        //private RaycastHit[] _raycastHit = null;
        //private bool _exitedGracefully;
        //private Rewired.Player _player;
        private PlayerComponent _playerComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            //_exitedGracefully = false;
            //if (_player == null)
            //    _player = Rewired.ReInput.players.GetPlayer(0); // get the player by id
            //if (_playerComponent == null)
            //    _playerComponent = entity.Components.GetComponent<PlayerComponent>();
        }

        //public override void End()
        //{
        //    var repo = ParentObject.GetComponent<IComponentRepository>();
        //    if (!_exitedGracefully)
        //    {
        //        MessageDispatcher2.Instance.DispatchMsg(MessageToLiftableOnAbort, 0f, null,
        //            repo.UniqueId, null);
        //    }
        //    _playerComponent.SetLiftingObject(null);
        //}

        //public override onfix

        public override AtomActionResults OnUpdate()
        {
            var liftingObjectPosition = _playerComponent.GetLiftingObjectPosition();
            _entity.transform.position = new Vector3(liftingObjectPosition.x, liftingObjectPosition.y,
                _entity.transform.position.z);
            //_playerComponent
            //if (!_player.GetButton(ButtonHeldActionName))
            //{
            //    _exitedGracefully = true;
            //    _isRunning = false;
            //}
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetParentObject(GameObject parentObject)
        {
            var ParentObject = parentObject;
            var repo = ParentObject.GetComponent<IComponentRepository>();
            _playerComponent = repo.Components.GetComponent<PlayerComponent>();
        }
    }
}
