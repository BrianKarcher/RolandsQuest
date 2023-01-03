using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Messaging;
using RQ.Model.Serialization;
using RQ.Physics;
using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Sprite
{
    /// <summary>
    /// Adjusts the Z position of the sprite slightly based on its Y position in world coordinates
    /// Used to place objects behind other ojects based on their height on the map
    /// </summary>
    [AddComponentMenu("RQ/Common/Adjust Depth By Y")]
    public class AdjustDepthByY : ComponentPersistence<AdjustDepthByY>
    {
        /// <summary>
        /// The foot position is used to determine the Y position on the map.
        /// </summary>
        public float FootPositionY;
        private float _oldAdjustedZ;
        private string _physicsComponentId;
        private PhysicsData _physicsData;
        private float _originalZ;

        //public override void Awake()
        //{
        //    base.Awake();
        //    //_originalZ = transform.localPosition.z;
        //}

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            _originalZ = this.transform.position.z;
            var physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();
            if (physicsComponent != null)
            {
                _physicsComponentId = physicsComponent.UniqueId;

                MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponentId,
                    Enums.Telegrams.GetPhysicsData, null);
            }
        }

        // Called once per physics update
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!Application.isPlaying)
                return;
            var gameConfig = GameDataController.Instance?.GetGameConfig();
            float yPosition = transform.position.y + FootPositionY;
            // Keep the adjustment minor, we subtract because the lower the number, the closer to the camera
            //float adjustedZ = _originalZ + (yPosition / 1000f);
            float newAdjustedZ = yPosition / 1000f;
            if (_oldAdjustedZ != newAdjustedZ)
            {
                if (_physicsData != null)
                {
                    _physicsData.ZDepthByY = newAdjustedZ;
                }
                else
                {
                    var z = gameConfig.GlobalSpriteZOffset + newAdjustedZ;
                    //this.transform.position = new Vector3(transform.position.x, transform.position.y,
                    //    _originalZ + newAdjustedZ);
                    this.transform.position = new Vector3(transform.position.x, transform.position.y,
                        z);
                }
                _oldAdjustedZ = newAdjustedZ;
            }
            //transform.SetLocalPosZ(adjustedZ);
            //transform.position = new Vector3(transform.position.x, transform.position.y, adjustedZ);
        }

        //public void SetOriginalZ(float z)
        //{
        //    _originalZ = z;
        //}

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.SetPhysicsData:
                    _physicsData = msg.ExtraInfo as PhysicsData;
                    break;
            }

            return false;
        }

        //public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Serialize(entitySerializedData);
        //    var data = new AdjustDepthByYData();
        //    data.OriginalZ = _originalZ;
        //    data.OldAdjustedZ = _oldAdjustedZ;
        //    data.FootPositionY = FootPositionY;
        //    base.SerializeComponent(entitySerializedData, data);
        //}

        //public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        //{
        //    base.Deserialize(entitySerializedData);
        //    var data = base.DeserializeComponent<AdjustDepthByYData>(entitySerializedData);
        //    _oldAdjustedZ = data.OldAdjustedZ;
        //    _originalZ = data.OriginalZ;
        //    FootPositionY = data.FootPositionY;
        //}
    }
}