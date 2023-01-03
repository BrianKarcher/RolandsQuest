using RQ.Common;
using RQ.Enums;
using RQ.FSM.V2;
using RQ.Messaging;
using RQ.Model.Interfaces;
using RQ.Physics.Components;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Controller.Actions
{
    [AddComponentMenu("RQ/Action/Enable Collider")]
    public class EnableCollider : ActionBase, ICollisionModifier
    {
        [SerializeField]
        private bool _allColliders = true;

        [SerializeField]
        [CollisionComponentTypeAttribute]
        private readonly List<CollisionComponent> _collisionComponentTypes = null;

        [SerializeField]
        private bool _enableOnEnter = false;

        [SerializeField]
        private bool _enableOnExit = false;

        public override void Act(Component otherRigidBody)
        {
            base.Act(otherRigidBody);
            Process(_enableOnEnter);
            //Log.Info("Entering Animator Act for entity " + GetEntity().name);
            //if (_animationComponent == null)
            //    return;
            //if (!String.IsNullOrEmpty(_animationType))
            //{
            //    _animationComponent.Animate(_animationType);
            //}
            //_animationComponent.Data.IsAnimationComplete = false;
            //var spriteAnimator = _animationComponent.GetSpriteAnimator() as SpriteAnimator;
            //var animationLength = spriteAnimator.GetCurrentClipLength();
            //SendMessageToSelf(animationLength, Enums.Telegrams.AnimationComplete, UniqueId);
        }

        private void Process(bool enable)
        {
            if (_allColliders)
            {
                var collisionComponents = GetEntity().Components.GetComponents<CollisionComponent>();
                foreach (var collisionComponent in collisionComponents)
                {
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, collisionComponent.UniqueId,
                        Telegrams.SetEnabled, enable);
                }
                //var uniqueId = GetEntity().UniqueId;
                //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, uniqueId, Telegrams.EnableCollider,
                //    enable);
            }
            else
            {
                foreach (var collider in _collisionComponentTypes)
                {
                    MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId, collider.UniqueId, 
                        Telegrams.SetEnabled, enable);
                }
            }
        }

        public override void ActExit(Component otherRigidBody)
        {
            base.ActExit(otherRigidBody);
            Process(_enableOnExit);
            //Log.Info("Exiting Animator act for entity " + GetEntity().name);
        }

        // TODO Make this generic
        //public IEnumerable<ICollisionComponent> GetCollisionComponents()
        public IList<IBaseObject> GetCollisionComponents()
        {
            var state = GetComponent<StateBase>();
            var stateMachine = state.StateMachine;
            if (stateMachine == null)
            {
                Debug.Log("Could not locate State Machine");
                return null;
            }

            var componentRepository = stateMachine.GetComponentRepository();

            if (componentRepository == null)
                return null;

            var collisionComponents = componentRepository.Components.GetComponents<ICollisionComponent>();

            return collisionComponents;
        }
    }
}
