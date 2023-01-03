using BehaviorDesigner.Runtime;
using RQ.Animation;
using RQ.Common.Components;
using RQ.Messaging;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Rotating")]
    public class RotatingComponent : ComponentPersistence<RotatingComponent>
    {
        private Behavior _behaviorTree;
        private AnimationComponent _animComponent;
        private long _rotateToId;

        public override void Start()
        {
            base.Start();
            _animComponent = _componentRepository.Components.GetComponent<AnimationComponent>();
        }

        public override void StartListening()
        {
            base.StartListening();
            _rotateToId = MessageDispatcher2.Instance.StartListening("RotateTo", _componentRepository.UniqueId, (data) =>
            {
                var destVector = (Vector2)data.ExtraInfo;
                //var facingDirection = 
                var newAngle = Vector2.Angle(Vector2.right, destVector);
                // Cross product helps us determine which direction vector is facing, since Dot product does not go
                // past 180
                Vector3 cross = Vector3.Cross(Vector2.right, destVector);

                if (cross.z < 0)
                    newAngle = 360 - newAngle;
                Debug.Log("Rotating to " + newAngle);
                var currentRotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
                gameObject.transform.localRotation = currentRotation;
            });
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("RotateTo", _componentRepository.UniqueId, _rotateToId);
        }
    }
}
