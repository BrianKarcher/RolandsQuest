using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class SetMaxSpeedAtom : AtomActionBase
    {
        public float _speed;
        public bool _resetToOriginal;
        public string PhysicsAffectorName = "";

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            var physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            var physicsData = physicsComponent.GetPhysicsData();
            if (string.IsNullOrEmpty(PhysicsAffectorName))
            {
                if (_resetToOriginal)
                    _speed = physicsData.OriginalMaxSpeed;
                physicsData.MaxSpeed = _speed;
            }
            else
            {
                var physicsAffector = physicsComponent.GetPhysicsAffector(PhysicsAffectorName);

                if (_resetToOriginal)
                    _speed = physicsAffector.OriginalMaxSpeed;
                physicsAffector.MaxSpeed = _speed;
            }
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
            return AtomActionResults.Success;
            //return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
