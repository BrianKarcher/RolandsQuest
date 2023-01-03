using RQ.Common;
using RQ.Common.Components;
using RQ.Messaging;
using RQ.Model.Serialization;
using RQ.Physics;
using RQ.Physics.Components;
using RQ.Physics.Wind;
using UnityEngine;

namespace RQ.Physics.Components
{
    /// <summary>
    /// Adjusts the Z position of the sprite slightly based on its Y position in world coordinates
    /// Used to place objects behind other ojects based on their height on the map
    /// </summary>
    [AddComponentMenu("RQ/Components/Physics/Takes Wind")]
    public class TakesWind : ComponentPersistence<TakesWind>
    {
        private bool _inWindZone = false;
        private WindArea _windZone;
        [Tag]
        public string WindTag;
        private Vector2 _windDirectionNormalized;

        PhysicsComponent _physicsComponent;

        public override void Start()
        {
            _physicsComponent = _componentRepository.Components.GetComponent<PhysicsComponent>();

        }

        public void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == WindTag)
            {
                _windZone = coll.gameObject.GetComponent<WindArea>();
                if (_windZone != null)
                {
                    _inWindZone = true;
                    _windDirectionNormalized = _windZone.Direction.normalized;
                }
            }
        }

        public void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (other.gameObject.tag == WindTag)
            {
                _windZone = other.gameObject.GetComponent<WindArea>();
                if (_windZone != null)
                {
                    _inWindZone = true;
                    _windDirectionNormalized = _windZone.Direction.normalized;
                }
            }
        }

        public void OnCollisionExit(UnityEngine.Collision other)
        {
            if (other.gameObject.tag == WindTag)
            {
                _inWindZone = false;
            }
        }

        public void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.tag == WindTag)
            {
                _inWindZone = false;
            }
        }

        public override void FixedUpdate()
        {
            if (_inWindZone)
            {
                _physicsComponent.AddForce(_windDirectionNormalized * _windZone.Strength);
            }
        }
    }
}