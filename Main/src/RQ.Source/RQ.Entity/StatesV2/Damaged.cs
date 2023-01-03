using RQ.AI.Action;
using RQ.Entity.Common;
using RQ.Messaging;
using RQ.Physics;
using UnityEngine;

namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Entity/Damaged")]
    public class Damaged : AnimatorState
    {
        //protected DamageComponent _damageComponent;
        //private DamageEntityInfo _damageInfo;
        private DamageBounceAtom _damageBounceAtom;
        public bool UseDamageColor = true;
        //private ISprite _sprite;

        //public override void SetupState()
        //{
        //    base.SetupState();
        //}

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
            //    Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);
        }

        public override void Enter()
        {
            base.Enter();
            _damageBounceAtom = new DamageBounceAtom();
            _damageBounceAtom.UseDamageColor = UseDamageColor;
            _damageBounceAtom.Start(GetComponentRepository());
            //Debug.Log("Ouch! That hurt!");
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
            //    Enums.Telegrams.StopMovement, null);
            ////_physicsComponent.Stop(); // Stop the steering behaviors from affecting the sprite
            ////base.GetEntityStats();

            ////_spriteBaseComponent.
            ////var entityStats = _spriteBaseComponent.GetEntityStats();
            ////var damageInfo = _physicsComponent.DamageInfo;
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
            //    Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);
            ////if (_entityStats != null)
            ////{
            ////    _entityStats.CurrentHP -= _damageInfo.DamageAmount;
            ////    return;
            ////}

            //var physicsData = _physicsComponent.GetPhysicsData();

            //physicsData.InputForce.SetToZero();

            //var force = physicsData.DamagedBounceForce;

            //var bounceDir = _physicsComponent.GetPos() - _damageInfo.DamageSourceLocation;
            //var newVelocity = Vector2D.Vec2DNormalize(bounceDir) * force;
            //physicsData.ExternalVelocity = newVelocity;
            //// Create a constant force in the opposite direction in the force of the drag
            //physicsData.ExternalForce = Vector2D.Vec2DNormalize(newVelocity) * -1 * physicsData.DamageDrag;
            ////_sprite.GetPhysicsData().ExternalForce = newForce;

            ////_sprite.Stop();
            ////sprite.GetSteering().TurnOn(behavior_type.wander);
            //if (_damageComponent.UseDamageColor && UseDamageColor)
            //{
            //    AnimationComponent.GetSpriteAnimator().SetColor(_damageComponent.DamageColor);
            //}

            //MessageDispatcher.Instance.DispatchMsg(_sprite.DamageTime, _sprite.ID(), _sprite.ID(),
            //    Enums.Telegrams.ChangeState, ID, Enums.TelegramEarlyTermination.ChangeScenes);
        }

        public override void Exit()
        {
            base.Exit();
            _damageBounceAtom.End();
            ////sprite.GetSteering().TurnOff(behavior_type.wander);
            //_physicsComponent.GetPhysicsData().ExternalForce = Vector2D.Zero();
            //_physicsComponent.GetPhysicsData().ExternalVelocity = Vector2D.Zero();
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _damageComponent.UniqueId,
            //    Enums.Telegrams.GetDamageEntityInfo, null, (damageInfo) => _damageInfo = damageInfo as DamageEntityInfo);
            //_damageInfo.IsDamaged = false;
            ////StateMachine.GetStateInfo().IsDamaged = false;
            ////MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponent.UniqueId,
            ////    Enums.Telegrams.SetDamageInfo, null);
            ////_physicsComponent.DamageInfo = null;
            //if (_damageComponent.UseDamageColor && UseDamageColor)
            //{
            //    _animationComponent.GetSpriteAnimator().SetColor(Color.white);
            //}
        }
    }
}
