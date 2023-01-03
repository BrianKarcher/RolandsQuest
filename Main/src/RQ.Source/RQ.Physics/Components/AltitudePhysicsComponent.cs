using System;
using RQ.Common.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Physics;
using RQ.Serialization;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Physics/Altitude Physics")]
    public class AltitudePhysicsComponent : ComponentPersistence<AltitudePhysicsComponent>
    {
        [SerializeField]
        private AltitudeData _altitudeData = new AltitudeData();

        private AltitudeData _initialAltitudeData;

        private Rigidbody2D _rigidBody;

        private long _setAirVelocityId, _setAltitudeId;

        private Action<Telegram2> _setAirVelocityDelegate;
        private Action<Telegram2> _setAltitudeDelegate;

        public override void Awake()
        {
            base.Awake();
            _rigidBody = GetComponent<Rigidbody2D>();
            _initialAltitudeData = new AltitudeData();
            _altitudeData.CopyTo(_initialAltitudeData);
            _setAirVelocityDelegate = (data) =>
            {
                var airVelocitySplit = ((string) data.ExtraInfo).Split(',');
                float.TryParse(airVelocitySplit[0], out var airVelocityX);
                float.TryParse(airVelocitySplit[1], out var airVelocityY);
                var airVelocity = new Vector2D(airVelocityX, airVelocityY);
                _altitudeData.AirVelocity = airVelocity;
                if (!airVelocity.isZero())
                    _altitudeData.IsAirborn = true;
                else
                    _altitudeData.IsAirborn = false;
                //Stop();
            };
            _setAltitudeDelegate = (data) =>
            {
                var altitudeSplit = ((string) data.ExtraInfo).Split(',');
                float.TryParse(altitudeSplit[0], out var altitudeX);
                float.TryParse(altitudeSplit[1], out var altitudeY);
                var altitude = new Vector2D(altitudeX, altitudeY);
                _altitudeData.Altitude = altitude;
            };
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_altitudeData.IsAirborn)
            {
                _altitudeData.AirVelocity += _altitudeData.Gravity;
                // FixedUpdate gets called 50 times per second
                _altitudeData.Altitude += _altitudeData.AirVelocity / 50f;
                if (_altitudeData.Altitude.y < 0)
                {
                    MessageDispatcher2.Instance.DispatchMsg("HitGround", 0f, _componentRepository.UniqueId, _componentRepository.UniqueId, null);
                    if (_altitudeData.StopWhenHitGround)
                    {
                        _altitudeData.Altitude = new Vector2(_altitudeData.Altitude.x, 0);
                        _altitudeData.AirVelocity = Vector2.zero;
                        _altitudeData.IsAirborn = false;
                    }
                }
                
                SetPosition(_altitudeData.Altitude);
            }
        }

        public override void Reset()
        {
            base.Reset();
            _initialAltitudeData.CopyTo(_altitudeData);
        }

        private void SetPosition(Vector2D localPosition)
        {
            transform.localPosition = localPosition.ToVector3(transform.localPosition.z);
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.StopMovement:
                    Stop();
                    break;
            }
            return false;
        }

        public void Stop()
        {
            _altitudeData.AirVelocity.SetToZero();
            _altitudeData.Gravity.SetToZero();
        }

        public override void StartListening()
        {
            base.StartListening();
            _setAirVelocityId =
                MessageDispatcher2.Instance.StartListening("SetAirVelocity", _componentRepository.UniqueId, _setAirVelocityDelegate);
            //_componentRepository.StartListening("SetAirVelocity", this.UniqueId, _setAirVelocityDelegate);
            _setAltitudeId =
                MessageDispatcher2.Instance.StartListening("SetAltitude", _componentRepository.UniqueId, _setAltitudeDelegate);
            //_componentRepository.StartListening("SetAltitude", this.UniqueId, _setAltitudeDelegate);
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetAirVelocity", _componentRepository.UniqueId, _setAirVelocityId);
            //_componentRepository.StopListening("SetAirVelocity", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetAltitude", _componentRepository.UniqueId, _setAltitudeId);
            //_componentRepository.StopListening("SetAltitude", this.UniqueId);
        }

        public AltitudeData GetAltitudeData()
        {
            return _altitudeData;
        }

        /// <summary>
        /// The reason EntityData is a parameter instead of a return type is because multiple layers of derived objects
        /// may populate different parts of this object
        /// </summary>
        /// <param name="entityData"></param>
        public override void Serialize(EntitySerializedData entityData)
        {
            base.Serialize(entityData);
            base.SerializeComponent(entityData, _altitudeData);
        }

        public override void Deserialize(EntitySerializedData entityData)
        {
            base.Deserialize(entityData);
            _altitudeData = base.DeserializeComponent<AltitudeData>(entityData);            
        }
    }
}
