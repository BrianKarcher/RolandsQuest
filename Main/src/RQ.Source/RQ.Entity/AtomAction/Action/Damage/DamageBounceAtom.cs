using RQ.Animation;
using RQ.Common.UniqueId;
using RQ.Entity.AtomAction;
using RQ.Entity.Common;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using System;
using UnityEngine;

namespace RQ.AI.Action
{
    [Serializable]
    public class DamageBounceAtom : AtomActionBase
    {
        //[SerializeField]
        //private float _bounceTime;
        private DamageEntityInfo _damageInfo;
        private PhysicsComponent _physicsComponent;
        private DamageComponent _damageComponent;
        private AnimationComponent _animationComponent;
        public bool UseDamageColor = true;
        private long _damageBounceCompleteIndex;
        private IPhysicsAffector _damageBounceAffector;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            
            _physicsComponent = entity.Components.GetComponent<PhysicsComponent>();
            _damageComponent = entity.Components.GetComponent<DamageComponent>();
            _animationComponent = entity.Components.GetComponent<AnimationComponent>();

            //Debug.Log("Ouch! That hurt!");
            MessageDispatcher.Instance.DispatchMsg(0f, _entity.UniqueId, _physicsComponent.UniqueId,
                Enums.Telegrams.StopMovement, null);

            MessageDispatcher.Instance.DispatchMsg(0f, _entity.UniqueId, _damageComponent.UniqueId,
                Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);

            var physicsData = _physicsComponent.GetPhysicsData();

            //physicsData.InputForce.SetToZero();
            _physicsComponent.GetPhysicsAffector("Input").Stop();

            var force = physicsData.DamagedBounceForce;

            var bounceDir = _physicsComponent.GetWorldPos2D() - _damageInfo.DamageSourceLocation;
            var bounceDirNormalized = bounceDir.normalized;
            var initialBounceForce = bounceDirNormalized * force;
            _damageBounceAffector = _physicsComponent.GetPhysicsAffector("DamageBounce");

            // Bounces are an initial velocity
            //_physicsComponent.AddVelocity(initialBounceForce);
            _physicsComponent.AddForce(initialBounceForce, ForceMode.Impulse);
            //_physicsComponent.AddForce(initialBounceForce);
            //_damageBounceAffector.Velocity = initialBounceForce;
            // Create a constant force in the opposite direction in the force of the drag
            _damageBounceAffector.Force = bounceDirNormalized * physicsData.DamageDrag * -1;

            if (_damageComponent.UseDamageColor && UseDamageColor)
            {
                _animationComponent.GetSpriteAnimator().SetColor("Damage", _damageComponent.DamageColor);
            }
        }

        //public override void StartListening(IComponentRepository entity)
        //{
        //    base.StartListening(entity);
        //    //_damageBounceCompleteIndex = MessageDispatcher2.Instance.StartListening("DamageBounceComplete", entity.UniqueId, (data) =>
        //    //{
        //    //    _isRunning = false;
        //    //});
        //}

        //public override void StopListening(IComponentRepository entity)
        //{
        //    base.StopListening(entity);
        //    //MessageDispatcher2.Instance.StopListening("DamageBounceComplete", entity.UniqueId, _damageBounceCompleteIndex);
        //}

        public override void End()
        {
            base.End();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
            //_damageBounceAffector.Force = Vector2D.Zero();
            //_damageBounceAffector.Velocity = Vector2D.Zero();
            _damageBounceAffector.Stop();
            MessageDispatcher.Instance.DispatchMsg(0f, _entity.UniqueId, _damageComponent.UniqueId,
                Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);
            _damageInfo.IsDamaged = false;
            //StateMachine.GetStateInfo().IsDamaged = false;
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
            //    Enums.Telegrams.SetDamageInfo, null);
            //_physicsComponent.DamageInfo = null;
            if (_damageComponent.UseDamageColor && UseDamageColor)
            {
                _animationComponent.GetSpriteAnimator().RemoveColor("Damage");
                //_animationComponent.GetSpriteAnimator().SetColor(Color.white);
            }
        }

        public override AtomActionResults OnUpdate()
        {
            return _isRunning ? AtomActionResults.Running : AtomActionResults.Success;
        }
    }
}
