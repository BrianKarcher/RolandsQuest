using RQ.Physics.Components;
using UnityEngine;

namespace RQ.Controller.Triggers
{
    public class AdjustZTrigger : MonoBehaviour
    {
        public float zOffset;
        public void OnTriggerEnter2D(Collider2D other)
        {
            var entity = other.GetComponent<PhysicsComponent>();
            if (entity != null)
            {
            }
        }
    }
}
