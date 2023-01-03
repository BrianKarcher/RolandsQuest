using RQ.Model.Physics;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Entity.StatesV2.Physics
{
    [AddComponentMenu("RQ/States/State/Physics/Simple Movement")]
    public class SimpleMovementState : AnimatorState
    {
        [SerializeField]
        protected Vector2D _velocity;
        [SerializeField]
        protected bool _setAltitude = false;
        [SerializeField]
        protected Vector2D _altitude;
        [SerializeField]
        protected Vector2D _airVelocity;
        [SerializeField]
        protected Vector2D _gravity;

        protected AltitudeData _altitudeData;

        //private ISprite _sprite;

        //public override void SetEntity(Transform entity)
        //{
        //    base.SetEntity(entity);
        //    //_sprite = EntityUIBase.GetEntity(entity);
        //    ////sprite = entity.GetComponent<ISprite>();
        //    ////if (sprite == )
        //    ////var entityUIBase = entity.GetComponent<EntityUIBase>();
        //    ////_sprite = entityUIBase.GetRQObject() as ISprite;
        //    //if (_sprite == null)
        //    //    throw new Exception("FSM - Sprite not set.");
        //}

        public override void Enter()
        {
            base.Enter();

            //_sprite.Stop();
            //sprite.GetSteering().TurnOn(behavior_type.wander);
        }

        public override void SetupState()
        {
            base.SetupState();
            var altitudeComponent = _componentRepository.Components.GetComponent<AltitudePhysicsComponent>();
            _altitudeData = altitudeComponent.GetAltitudeData();
            //var physicsData = _physicsComponent.GetPhysicsData();
            _physicsComponent.SetVelocity((Vector2)_velocity);

            SetAltitudeData(_altitudeData);
        }

        public virtual void SetAltitudeData(AltitudeData altitudeData)
        {
            if (_setAltitude)
                altitudeData.Altitude = _altitude;
            altitudeData.AirVelocity = _airVelocity;
            altitudeData.Gravity = _gravity;
        }

        public override void Exit()
        {
            base.Exit();
            //sprite.GetSteering().TurnOff(behavior_type.wander);
        }
    }
}
