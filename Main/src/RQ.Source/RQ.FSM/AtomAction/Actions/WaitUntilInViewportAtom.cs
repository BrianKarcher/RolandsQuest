using RQ.AI;
using RQ.Common.Container;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model;
using RQ.Model.Interfaces;
using RQ.Physics;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class WaitUntilInViewportAtom : AtomActionBase
    {
        private bool _isInViewport = false;
        private ICameraClass _camera;
        private IBasicPhysicsComponent _physicsComponent;
        //private IGameController _gameController;
        //public string AnimationType;
        //private AnimationComponent _animComponent;
        //private IComponentRepository _entity;
        //private bool _isRunning;
        //private long _animationCompleteIndex;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _entity = entity;
            _isInViewport = false;
            _physicsComponent = entity.Components.GetComponent<IBasicPhysicsComponent>();
            _camera = FindCameraEntity();            
            //_camera = GameObject.FindObjectOfType<ICameraClass>();
        }

        private ICameraClass FindCameraEntity()
        {
            var entities = EntityContainer._instance.GetAllEntities();
            foreach (var entity in entities)
            {
                var cameraClass = entity.Value.Components.GetComponent<ICameraClass>();
                if (cameraClass != null)
                    return cameraClass;
            }
            return null;
        }

        //public void End()
        //{
        //    StopListening(_entity);
        //}

        public override AtomActionResults OnUpdate()
        {
            if (!_isInViewport)
                _isInViewport = isInViewport();
            return _isInViewport ? AtomActionResults.Success : AtomActionResults.Running;
        }

        private bool isInViewport()
        {
            //MessageDispatcher2.Instance.DispatchMsg("GetCamera", 0f, stateMachine.GetComponentRepository().UniqueId, "Game Controller", null);
            //var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            Vector2D pos = _physicsComponent.GetWorldPos();
            var isInViewport = _camera.IsPosInViewport(pos);
            //MessageDispatcher2.Instance.DispatchMsg("IsPosInViewport", 0f, this.UniqueId, "Game Controller", pos);
            return isInViewport; //GetIsConditionSatisfied(stateMachine);
        }
    }
}
