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
    public class CarryObjectAtom : AtomActionBase
    {
        public string MessageToLiftableOnAbort;
        public string ButtonHeldActionName;
        private GameObject LiftableObject;
        //public float MaxDistance;
        //private PhysicsComponent _physicsComponent;
        //private AnimationComponent _animationComponent;
        //private int _obstacleLayerMask;
        //private GameObject _liftableObject;
        //private int _layerMask;
        //private RaycastHit[] _raycastHit = null;
        private bool _exitedGracefully;
        private Rewired.Player _player;
        private PlayerComponent _playerComponent;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _exitedGracefully = false;
            if (_player == null)
                _player = Rewired.ReInput.players.GetPlayer(0); // get the player by id
            if (_playerComponent == null)
                _playerComponent = entity.Components.GetComponent<PlayerComponent>();
        }

        public override void End()
        {
            var repo = LiftableObject.GetComponent<IComponentRepository>();
            if (!_exitedGracefully)
            {
                MessageDispatcher2.Instance.DispatchMsg(MessageToLiftableOnAbort, 0f, null,
                    repo.UniqueId, null);
            }
            _playerComponent.SetLiftingObject(null);
        }

        public override AtomActionResults OnUpdate()
        {
            if (!_player.GetButton(ButtonHeldActionName))
            {
                _exitedGracefully = true;
                _isRunning = false;
            }
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public void SetLiftableObject(GameObject liftableObject)
        {
            LiftableObject = liftableObject;
        }
    }
}
