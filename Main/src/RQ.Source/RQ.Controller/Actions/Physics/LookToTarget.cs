using RQ.Enums;
using RQ.Messaging;
using RQ.Physics;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Physics/Look To Target")]
    public class LookToTarget : ActionBase
    {
        [SerializeField]
        private float _rotationSpeed = 80f;
        //private float _rotationVelocity = 5f;
        [SerializeField]
        private Transform _objectToSet = null;
        //[SerializeField]
        //private float _currentAngle;
        [SerializeField]
        private Vector2D _heading;
        //private Quaternion _currentRotation;
        private string _physicsComponentId;
        private PhysicsData _physicsData;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            if (!Application.isPlaying)
                return;
            //var physicsComponent = GetEntity().Components.GetComponent<PhysicsComponent>();
            //if (physicsComponent == null)            
            //    throw new Exception("Physics component required");
            
            _physicsComponentId = _physicsComponent.UniqueId;

            MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponentId,
                Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);

            // Set default heading to right if not currently set
            if (_physicsData.Heading.sqrMagnitude < .0001f)
                _physicsData.Heading = Vector2.right;

            _heading = _physicsData.Heading;

            //var newAngle = Vector2.Angle(Vector2.right, _physicsData.Heading);

            // Quick and easy workaround for the dot product issue of the max angle being 180 degrees
            //if (_physicsData.Heading.y < 0)
            //    _currentAngle = 360f - newAngle;
            //else
            //    _currentAngle = newAngle;

            //_currentRotation = Quaternion.LookRotation(_physicsData.Heading.ToVector3(0));
        }

        // Called once per physics update
        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;
            if (_physicsData == null)
            {
                MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponentId,
                    Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);
            }
            var from = _aiComponent.Target.position;
            var to = _physicsComponent.GetWorldPos();
            var directionToTarget = from - to;
            var destAngle = GetAngle(directionToTarget);
            var currentAngle = GetAngle(_physicsData.Heading);
            //Quaternion headingRotation = Quaternion.LookRotation(_physicsData.Heading.ToVector3(0), Vector3.forward);
            //Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.forward);
            var headingVector = _physicsData.Heading;
            //Quaternion headingRotation = Quaternion.LookRotation(headingVector, Vector3.forward);
            //headingRotation.x = 0;
            //headingRotation.y = 0;
            //Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.forward);
            //lookRotation.x = 0;
            //lookRotation.y = 0;

            //var currentAngle = Quaternion.Angle(Quaternion.LookRotation(Vector3.right), headingRotation);
            //var destAngle = Quaternion.Angle(headingRotation, lookRotation);
            var angle = Vector2.Angle(_physicsData.Heading, directionToTarget);
            //var angle = Mathf.Abs(destAngle - currentAngle);
            //angle = Mathf.Clamp(angle, -_rotationSpeed, _rotationSpeed);
            //currentAngle += angle;

            var timeToComplete = angle / _rotationSpeed;
            var donePercentage = Mathf.Min(1f, Time.deltaTime / timeToComplete);
            //var newAngle = currentAngle + angle * donePercentage;

            // rotate towards a direction, but not immediately (rotate a little every frame)
            // The 3rd parameter is a number between 0 and 1, where 0 is the start of the rotation and 1 is the end rotation
            //var _currentRotation = Quaternion.Slerp(headingRotation, lookRotation, donePercentage);
            //var _currentRotation = Quaternion.Slerp(headingRotation, lookRotation, Time.deltaTime * .008f);

            //var _currentRotation = Quaternion.AngleAxis(currentAngle * Time.deltaTime, Vector3.forward);
            //var _currentRotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            var _currentRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            var _destinationRotation = Quaternion.AngleAxis(destAngle, Vector3.forward);
            var newRotation = Quaternion.Slerp(_currentRotation, _destinationRotation, donePercentage);

            //var currentQuaternion = Quaternion.AngleAxis(_currentAngle, Vector2.right);
            //Quaternion.Slerp(currentQuaternion, rot, _maxAngleTurn / 50);

            //_currentRotation = Quaternion.Slerp(_currentRotation, rot, _maxAngleTurn / 50);

            //var angleToTarget = Vector2.Angle(_heading, vectorToTarget);
            //_currentAngle = Mathf.MoveTowardsAngle(_currentAngle, angleToTarget, _maxAngleTurn);

            //_currentAngle += _rotationVelocity / 50;

            //_currentRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
            //_heading = _currentRotation.eulerAngles;
            // TODO Set the new heading in the Physics Component
            _heading = newRotation * Vector3.right;
            _physicsData.Heading = _heading;
            SendMessageToSpriteBase(0f, Telegrams.HeadingChanged, new Vector2D(_heading.x, _heading.y));

            //_currentRotation.
            if (_objectToSet != null)
            {
                _objectToSet.transform.localRotation = _currentRotation;
            }
        }

        //// Called once per physics update
        //public override void Update()
        //{
        //    base.Update();
        //    if (!Application.isPlaying)
        //        return;
        //    var from = _aiComponent.Target.position;
        //    var to = _physicsComponent.GetPos();
        //    var directionToTarget = (from - to).normalized;
        //    //Quaternion headingRotation = Quaternion.LookRotation(_physicsData.Heading.ToVector3(0), Vector3.forward);
        //    //Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.forward);
        //    var headingVector = _physicsData.Heading.ToVector3(0);
        //    Quaternion headingRotation = Quaternion.LookRotation(headingVector, Vector3.forward);
        //    headingRotation.x = 0;
        //    headingRotation.y = 0;
        //    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget, Vector3.forward);
        //    lookRotation.x = 0;
        //    lookRotation.y = 0;

        //    //var currentAngle = Quaternion.Angle(Quaternion.LookRotation(Vector3.right), headingRotation);
        //    var destAngle = Quaternion.Angle(headingRotation, lookRotation);
        //    //var angle = destAngle - currentAngle;
        //    //angle = Mathf.Clamp(angle, -_rotationSpeed, _rotationSpeed);
        //    //currentAngle += angle;

        //    var timeToComplete = destAngle / _rotationSpeed;
        //    var donePercentage = Mathf.Min(1f, Time.deltaTime / timeToComplete);

        //    // rotate towards a direction, but not immediately (rotate a little every frame)
        //    // The 3rd parameter is a number between 0 and 1, where 0 is the start of the rotation and 1 is the end rotation
        //    //var _currentRotation = Quaternion.Slerp(headingRotation, lookRotation, donePercentage);
        //    var _currentRotation = Quaternion.Slerp(headingRotation, lookRotation, Time.deltaTime * .008f);
            
        //    //var _currentRotation = Quaternion.AngleAxis(currentAngle * Time.deltaTime, Vector3.forward);

        //    //var currentQuaternion = Quaternion.AngleAxis(_currentAngle, Vector2.right);
        //    //Quaternion.Slerp(currentQuaternion, rot, _maxAngleTurn / 50);

        //    //_currentRotation = Quaternion.Slerp(_currentRotation, rot, _maxAngleTurn / 50);

        //    //var angleToTarget = Vector2.Angle(_heading, vectorToTarget);
        //    //_currentAngle = Mathf.MoveTowardsAngle(_currentAngle, angleToTarget, _maxAngleTurn);

        //    //_currentAngle += _rotationVelocity / 50;

        //    //_currentRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
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

        private float GetAngle(Vector2 dir)
        {
            //Vector3 dir = player.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            return angle;
            if (dir.y < 0)
                return 360f - angle;
            else
                return angle;
            //return angle;

            var newAngle = Vector2.Angle(Vector2.right, dir);

            // Quick and easy workaround for the dot product issue of the max angle being 180 degrees
            if (dir.y < 0)
                return 360f - newAngle;
            else
                return newAngle;    
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            _physicsComponentId = _physicsComponent.UniqueId;
            MessageDispatcher.Instance.DispatchMsg(0F, this.UniqueId, _physicsComponentId,
                Enums.Telegrams.GetPhysicsData, null, (physicsData) => _physicsData = (PhysicsData)physicsData);
            _heading = _physicsData.Heading;
        }
    }
}
