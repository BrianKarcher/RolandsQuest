using RQ.Common.Components;
using RQ.Enums;
using RQ.Messaging;
using RQ.Model.Messaging;
using RQ.Physics.Components;
using UnityEngine;

namespace Assets.Source
{
    /// <summary>
    /// Relays triggers and collisions on this game object to the whole sprite
    /// </summary>
    [AddComponentMenu("RQ/Components/Physics/Collision Processor Component")]
    public class CollisionProcessor : ComponentPersistence<CollisionProcessor>
    {
        public void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (other.collider.tag == "Player")
            {
                Debug.Log($"{_componentRepository.name} collided with {other.collider.tag}");
                int i = 1;
            }
            var id = UniqueId;
            ContactPoint contact = other.contacts.Length == 0 ? new ContactPoint() : other.contacts[0];
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                OtherCollider = other.collider,
                CollisionCollider = other,
                ThisTag = this.tag,
                HitPosition = contact.point
            };
            SendMessageToSpriteBase(0f, Telegrams.CollisionEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("CollisionEnter", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.CollisionEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.CollisionEnter, other);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var id = UniqueId;
            // OnTriggerEnter does not have the exact hit position, so just use the point in between instead
            var hitPosition = (other.transform.position + transform.position) / 2;
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                //OtherCollider = other,
                //TriggerCollider = other,
                ThisTag = this.tag,
                HitPosition = hitPosition
            };
            SendMessageToSpriteBase(0f, Telegrams.TriggerEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("TriggerEnter", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.TriggerEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.TriggerEnter, other);
        }

        public void OnTriggerEnter(Collider other)
        {
            var id = UniqueId;
            // OnTriggerEnter does not have the exact hit position, so just use the point in between instead
            var hitPosition = (other.transform.position + transform.position) / 2;
            var _ourCollisionComponent = GetComponent<CollisionComponent>();
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                OtherCollider = other,
                ThisTag = this.tag,
                HitPosition = hitPosition,
                OurCollisionComponent = _ourCollisionComponent
                //AreWeADamageCollider = _ourCollisionComponent?.GetCollisionData()?.DamageCollider
            };
            SendMessageToSpriteBase(0f, Telegrams.TriggerEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("TriggerEnter", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.TriggerEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.TriggerEnter, other);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            var id = UniqueId;
            // OnTriggerEnter does not have the exact hit position, so just use the point in between instead
            var hitPosition = (other.transform.position + transform.position) / 2;
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                //TriggerCollider = other,
                ThisTag = this.tag,
                HitPosition = hitPosition
            };
            //SendMessageToSpriteBase(0f, Telegrams.TriggerEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("TriggerExit", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.TriggerEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.TriggerEnter, other);
        }

        public void OnTriggerExit(Collider other)
        {
            var id = UniqueId;
            // OnTriggerEnter does not have the exact hit position, so just use the point in between instead
            var hitPosition = (other.transform.position + transform.position) / 2;
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                OtherCollider = other,
                ThisTag = this.tag,
                HitPosition = hitPosition
            };
            //SendMessageToSpriteBase(0f, Telegrams.TriggerEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("TriggerExit", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.TriggerEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.TriggerEnter, other);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            var id = UniqueId;
            var contact = other.contacts.Length == 0 ? new ContactPoint2D() : other.contacts[0];
            //var contact = other.contacts.FirstOrDefault();
            CollisionMessageData data = new CollisionMessageData()
            {
                CollisionComponentUniqueId = id,
                //CollisionCollider = other,
                ThisTag = this.tag,
                HitPosition = contact.point
            };
            SendMessageToSpriteBase(0f, Telegrams.CollisionEnter, data);
            MessageDispatcher2.Instance.DispatchMsg("CollisionEnter", 0f, this.UniqueId,
                _componentRepository.UniqueId, data);
            //base.SendLocalMessageToAll(0f, Telegrams.CollisionEnter, other);
            //MessageDispatcher.Instance.DispatchMsg(0f, id, id, Telegrams.CollisionEnter, other);
        }
    }
}
