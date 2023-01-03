using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class DragMaxSpeedAtom : AtomActionBase
    {
        public float Speed;
        private BasicPhysicsData _physicsData;
        private IBasicPhysicsComponent _physicsComponent;
        private IPhysicsAffector _physicsAffector;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _physicsData = _physicsComponent.GetPhysicsData();
            //_physicsAffector = _physicsComponent.GetPhysicsAffector("Drag");
            //_physicsAffector.Enabled = true;
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    _killSelfIndex = MessageDispatcher2.Instance.StartListening("AnimationComplete", entity.UniqueId, (data) =>
        //    {
        //        //var animation = _animComponent.Get
        //        if ((string)data.ExtraInfo != AnimationType)
        //            return;
        //        _isRunning = false;
        //    });
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    //MessageDispatcher2.Instance.StopListening("AnimationComplete", entity.UniqueId, _animationCompleteIndex);
        //}

        //public override void End()
        //{
        //}

        public override AtomActionResults OnUpdate()
        {

            //var newSpeed = _physicsAffector.MaxSpeed -= Speed * Time.deltaTime;
            //if (newSpeed < 0)
            //    newSpeed = 0;
            //_physicsAffector.MaxSpeed = newSpeed;

            var newSpeed = _physicsData.MaxSpeed -= Speed * Time.deltaTime;
            if (newSpeed < 0)
                newSpeed = 0;
            _physicsData.MaxSpeed = newSpeed;

            //_physicsData
            if (newSpeed == 0)
                return AtomActionResults.Success;
            else
                return AtomActionResults.Running;

                //return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }

        public override void End()
        {
            base.End();
            //_physicsAffector.MaxSpeed = 0f;
            //_physicsAffector.Enabled = false;
        }
    }
}
