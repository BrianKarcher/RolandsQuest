using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Circle Rotation")]
    public class CircleRotation : ActionBase
    {
        [SerializeField]
        private float _rotationVelocity = 5f;
        [SerializeField]
        private Transform _objectToSet = null;
        [SerializeField]
        private float _currentAngle;
        [SerializeField]
        private Vector3 _heading;
        private Quaternion _currentRotation;
        private PhysicsData _physicsData;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (!Application.isPlaying)
                return;
            //var physicsComponent = GetEntity().Components.GetComponent<PhysicsComponent>();
            //if (physicsComponent == null)            
            //    throw new Exception("Physics component required");
            
            if (_physicsData == null)
                MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponent.UniqueId,
                    Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);

            // Set default heading to right if not currently set
            if (_physicsData.Heading.sqrMagnitude < .0001f)
                _physicsData.Heading = Vector2.right;

            var newAngle = Vector2.Angle(Vector2.right, _physicsData.Heading);

            // Quick and easy workaround for the dot product issue of the max angle being 180 degrees
            if (_physicsData.Heading.y < 0)
                _currentAngle = 360f - newAngle;
            else
                _currentAngle = newAngle;            
        }

        // Called once per physics update
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!Application.isPlaying)
                return;
            _currentAngle += _rotationVelocity / 50;

            _currentRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
            //_heading = _currentRotation.eulerAngles;
            // TODO Set the new heading in the Physics Component
            _heading = _currentRotation * Vector3.right;
            if (_physicsData == null)
            {
                MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponent.UniqueId,
                    Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);
            }
            _physicsData.Heading = _heading;
            SendMessageToSpriteBase(0f, Telegrams.HeadingChanged, new Vector2D(_heading.x, _heading.y));

            //_currentRotation.
            if (_objectToSet != null)
            {
                _objectToSet.transform.localRotation = _currentRotation;
            }
        }
    }
}
