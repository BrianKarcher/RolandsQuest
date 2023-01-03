using RQ.Common.Components;
using RQ.Messaging;
using RQ.Model.Serialization;
using System;
using UnityEngine;

namespace RQ.Sprite
{
    /// <summary>
    /// Rotates the game object to the facing direction. Can be used for things like colliders
    /// </summary>
    [AddComponentMenu("RQ/Components/Physics/Rotate To Facing Direction")]
    public class RotateToFacingDirectionComponent : ComponentPersistence<RotateToFacingDirectionComponent>
    {
        private RotateToFacingDirectionData RotateToFacingDirectionData { get; set; }
        private long _facingDirectionChangedId;
        private Action<Telegram2> _facingDirectionChangedDelegate;
        //private Direction _direction;
        public override void Awake()
        {
            base.Awake();
            RotateToFacingDirectionData = new RotateToFacingDirectionData();
            _facingDirectionChangedDelegate = (data) =>
            {
                RotateToFacingDirectionData.Direction = (Direction)data.ExtraInfo;
                RotateToDirection();
            };
        }

        public override void StartListening()
        {
            base.StartListening();
            _facingDirectionChangedId = MessageDispatcher2.Instance.StartListening("FacingDirectionChanged", _componentRepository.UniqueId, 
                _facingDirectionChangedDelegate);
            //_componentRepository.StartListening("FacingDirectionChanged", this.UniqueId, );
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("FacingDirectionChanged", _componentRepository.UniqueId, _facingDirectionChangedId);
            //_componentRepository.StopListening("FacingDirectionChanged", this.UniqueId);
        }

        private void RotateToDirection()
        {
            var angle = RotateToFacingDirectionData.Direction.GetDirectionAngle();
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                angle);
        }

        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            base.SerializeComponent(entitySerializedData, RotateToFacingDirectionData);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            RotateToFacingDirectionData = base.DeserializeComponent<RotateToFacingDirectionData>(entitySerializedData);
            RotateToDirection();
        }
    }
}