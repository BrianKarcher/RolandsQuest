using RQ.AI;
using RQ.Entity.AtomAction;
using RQ.Entity.Components;
using RQ.Messaging;
using RQ.Physics.Components;
using RQ.Physics.SteeringBehaviors;
using System;
using UnityEngine;

namespace RQ.Animation.BasicAction.Action
{
    [Serializable]
    public class EnableColliderAtom : AtomActionBase
    {
        public bool _enableOnEnter;
        public bool _enableOnExit;
        public string name;

        public override void Start(IComponentRepository entity)
        {
            base.Start(entity);
            Process(_enableOnEnter);
        }

        public override void End()
        {
            Process(_enableOnExit);
        }

        public override AtomActionResults OnUpdate()
        {
            return AtomActionResults.Success;
        }

        private void Process(bool enable)
        {
            var collisionComponent = _entity.Components.GetComponent<CollisionComponent>(name);
            if (collisionComponent == null)
            {
                Debug.LogError("(EnableColliderAtom) Could not locate collider " + name);
                return;
            }
            if (enable == true)
            {
                //foreach (var collisionComponent in collisionComponents)
                //{
                    collisionComponent.gameObject.SetActive(true);
                //}
            }
            //MessageDispatcher2.Instance.DispatchMsg("EnableCollider", 0f, _entity.UniqueId, collisionComponent.UniqueId, enable);
            collisionComponent.EnableColliders(enable);
        }

    }
}
