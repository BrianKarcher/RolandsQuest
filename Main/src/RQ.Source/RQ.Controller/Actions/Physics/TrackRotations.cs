using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Track Rotations")]
    public class TrackRotations : ActionBase
    {
        [SerializeField]
        private float _triggerOnDegrees = 720f;
        //[SerializeField]
        //private string _entityVariable = "Entity_Spin";
        // Resets will be handled by the messaging system
        [SerializeField]
        private float _resetTimeDuration = 10f;
        [SerializeField]
        private float _rotationUpdateTimeDuration = 1f;
        //[SerializeField]
        //private float _lastResetTime = 0f;
        [SerializeField]
        private Vector2 _currentHeading;
        // TODO Take off serialization when done testing
        [SerializeField]
        private float _degreeCount;
        private string _physicsComponentId;
        private PhysicsData _physicsData;
        //private string _entityStatsComponentId;
        //private EntityStatsData _entityStatsData;

        //public override void Init()
        //{
        //    base.Init();
        //    //var entityStatsComponent = GetEntity().Components.GetComponent<EntityStatsComponent>();
        //    //_entityStatsComponentId = entityStatsComponent.UniqueId;
        //    //_entityStatsData = entityStatsComponent.GetEntityStats();
        //}

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (!Application.isPlaying)
                return;
            //var physicsComponent = GetEntity().Components.GetComponent<PhysicsComponent>();
            //if (physicsComponent == null)            
            //    throw new Exception("Physics component required");

            GetPhysicsData();

            SetCurrentHeading();

            SetResetTimer();
            SetRotationTimer();
            //var newAngle = Vector2.Angle(Vector2.right, _physicsData.Heading);

            // Quick and easy workaround for the dot product issue of the max angle being 180 degrees
            //if (_physicsData.Heading.y < 0)
            //    _currentAngle = 360f - newAngle;
            //else
            //    _currentAngle = newAngle;            
        }

        private void GetPhysicsData()
        {
            _physicsComponentId = _physicsComponent.UniqueId;

            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, _physicsComponentId,
                Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);
        }

        // Called once per physics update
        //public override void FixedUpdate()
        //{
        //    base.FixedUpdate();
        //    if (!Application.isPlaying)
        //        return;
        //    _currentAngle += _rotationVelocity / 50;

        //    _currentRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
        //    //_heading = _currentRotation.eulerAngles;
        //    // TODO Set the new heading in the Physics Component
        //    _heading = _currentRotation * Vector3.right;
        //    _physicsData.Heading = _heading;
        //    SendMessageToSpriteBase(0f, Telegrams.HeadingChanged, new Vector2D(_heading.x, _heading.y));

        //    //_currentRotation.
        //    if (_objectToSet != null)
        //    {
        //        _objectToSet.transform.localRotation = _currentRotation;
        //    }
        //}

        private void SetResetTimer()
        {
            MessageDispatcher.Instance.DispatchMsg(_resetTimeDuration, this.UniqueId, this.UniqueId,
                Enums.Telegrams.ProcessStateEvent, "Reset");
        }

        private void SetRotationTimer()
        {
            MessageDispatcher.Instance.DispatchMsg(_rotationUpdateTimeDuration, this.UniqueId, this.UniqueId,
                Enums.Telegrams.ProcessStateEvent, "LogRotation");
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Telegrams.ProcessStateEvent:
                    if (msg.ExtraInfo == "Reset")
                    {
                        ProcessReset();
                    }
                    else if (msg.ExtraInfo == "LogRotation")
                    {
                        LogRotation();
                    }
                    break;
            }

            return false;
        }

        private void ProcessReset()
        {
            Debug.Log("Reset called");
            _degreeCount = 0f;
            SetCurrentHeading();
            SetResetTimer();
        }

        private void LogRotation()
        {
            float angle = Vector2.Angle(_currentHeading, _physicsData.Heading);
            _degreeCount += angle;
            Debug.Log("Log Rotation: " + _degreeCount);
            if (_degreeCount >= _triggerOnDegrees)
            {
                Complete();
                return;
            }
            SetCurrentHeading();
            SetRotationTimer();
        }

        private void SetCurrentHeading()
        {
            _currentHeading = _physicsData.Heading;
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            GetPhysicsData();
            SetResetTimer();
            SetRotationTimer();
        }
    }
}
